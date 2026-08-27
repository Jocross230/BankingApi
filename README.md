# Banking API

A secure RESTful Banking API built with ASP.NET Core, Dapper, and Microsoft SQL Server.

## Features

- User Registration
- User Login
- JWT Authentication
- Protected API Endpoints
- Account Information and Balance Retrieval
- Fund Transfers
- Transaction History
- Transaction Logging
- SQL Server Integration
- Dapper Data Access
- Swagger API Documentation

## Architecture

The application follows Separation of Concerns:

Controller → Service → Repository → Database

### Controllers
Handle HTTP requests and responses.

### Services
Contain business logic and validation.

### Repositories
Handle database operations using Dapper.

### Database

The application uses Microsoft SQL Server with the following tables:

- Users
- Accounts
- Transactions

## Technologies Used

- ASP.NET Core
- C#
- Dapper
- Microsoft SQL Server
- JWT Authentication
- BCrypt
- Swagger / OpenAPI

## API Endpoints

### Authentication

#### Register

POST /api/Auth/register

```json
{
  "fullName": "Josiah onyeje",
  "email": "josiah@gmail.com",
  "password": "Josiah2222#"
}
