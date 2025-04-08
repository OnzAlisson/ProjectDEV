# ProjectDEV - Sistema de Gestão de Colaboradores e Unidades

Este é um projeto de backend que implementa um Sistema de Gestão de Colaboradores e Unidades, utilizando o PostgreSQL como banco de dados.

## Funcionalidades

- **Gestão de Usuários**: Cadastro, atualização e listagem de usuários com autenticação.
- **Gestão de Unidades**: Cadastro, atualização e listagem de unidades e seus colaboradores relacionados.
- **Gestão de Colaboradores**: Cadastro, atualização, remoção e listagem de colaboradores.
- **Autenticação JWT**: Implementação completa de autenticação via Bearer token.

## Configuração

### Pré-requisitos

- .NET 9.0
- PostgreSQL (ou Docker para executar PostgreSQL)

### Executando com Docker

1. Iniciar o banco de dados PostgreSQL usando o Docker Compose:
   ```
   docker-compose up -d
   ```

2. Executar a aplicação:
   ```
   dotnet run
   ```

### Configurações de Banco de Dados

As configurações de conexão com o banco de dados estão no arquivo `appsettings.json`. Por padrão, está configurado para:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=projectdev;Username=postgres;Password=postgres"
}
```

## API

### Autenticação

- **POST /api/auth/login**: Retorna um token JWT para autenticação.

### Usuários

- **POST /api/usuarios**: Cria um novo usuário.
- **PUT /api/usuarios/{id}**: Atualiza um usuário existente.
- **GET /api/usuarios**: Lista todos os usuários (opção de filtrar por status).

### Unidades

- **POST /api/unidades**: Cria uma nova unidade.
- **PUT /api/unidades/{id}**: Atualiza uma unidade existente.
- **GET /api/unidades**: Lista todas as unidades.
- **GET /api/unidades/colaboradores**: Lista todas as unidades com seus colaboradores.

### Colaboradores

- **POST /api/colaboradores**: Cria um novo colaborador.
- **PUT /api/colaboradores/{id}**: Atualiza um colaborador existente.
- **DELETE /api/colaboradores/{id}**: Remove um colaborador.
- **GET /api/colaboradores**: Lista todos os colaboradores.

## Estrutura do Projeto

- **Models**: Entidades do domínio.
- **DTOs**: Objetos de transferência de dados.
- **Controllers**: Endpoints da API.
- **Services**: Lógica de negócios.
- **Data**: Configuração e acesso ao banco de dados.

## Arquitetura

O projeto utiliza a arquitetura MVC (Model-View-Controller) com implementação do padrão de herança para as entidades do modelo. 