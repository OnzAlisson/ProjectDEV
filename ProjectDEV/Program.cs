using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProjectDEV.Data;
using ProjectDEV.Services;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Validar JWT Key
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("Jwt:Key não configurada. Por favor, adicione uma chave segura no arquivo appsettings.json.");
}

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = 
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

// Configure PostgreSQL
try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("String de conexão não configurada");
    }
    
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));
    
    Console.WriteLine("Conexão com banco de dados configurada com sucesso");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao configurar conexão com banco de dados: {ex.Message}");
    throw;
}

// Register Services
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<UnidadeService>();
builder.Services.AddScoped<ColaboradorService>();

// Configuração do Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProjectDEV API",
        Version = "v1",
        Description = "API para Gestão de Colaboradores e Unidades"
    });

    // Configuração do JWT no Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Enable CORS - Configuração mais permissiva
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Usar CORS no início do pipeline
app.UseCors();

// Usar UseSwagger sem condição para ambiente
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProjectDEV API v1");
    c.RoutePrefix = "swagger";
    c.DocExpansion(DocExpansion.List);
    c.DefaultModelsExpandDepth(0);
    c.OAuthClientId("swagger-ui");
    c.OAuthAppName("Swagger UI");
    // Ativar debug para erros do Swagger
    c.ConfigObject.AdditionalItems["syntaxHighlight"] = false;
    c.ConfigObject.AdditionalItems["tryItOutEnabled"] = true;
});

// Configurar o pipeline básico
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Aplicar migrações automaticamente quando o aplicativo iniciar
try
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        Console.WriteLine("Banco de dados migrado com sucesso!");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao aplicar migrações: {ex.Message}");
    // Não lançar exceção para permitir que a aplicação continue funcionando
}

Console.WriteLine("Aplicação pronta para ser executada. URL do Swagger: /swagger");
app.Run();
