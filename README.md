# Note Management API - REST API Example

This project is a starter example of a REST API built on .NET 8. It provides endpoints for user authentication, CRUD operations on notes, and user-related functionalities such as changing passwords.

## API Endpoints

### Authentication
- **POST** `/auth/signup`: User signup
- **POST** `/auth/signin`: User signin

### Notes
- **GET** `/notes`: Retrieve notes with various filtering options
- **POST** `/notes`: Create a new note
- **PUT** `/notes/{id}`: Update an existing note
- **DELETE** `/notes/{id}`: Delete a note by ID

### User
- **GET** `/user`: Retrieve user information
- **POST** `/user/password`: Change user password

## Security
- Bearer token authentication using JWT in the Authorization header.

## Tech Stack & Libraries
- **.NET 8**
- **Entity Framework** (ORM)
- **PostgreSQL** (Database)
- **AutoMapper** (Object mapping)
- **FluentValidation** (Validation framework)
- **Identity Model JWT** (Authentication)
- **Swagger** (API documentation)
- **Docker** (Containerization)
- **AutoFixture** (Test data generation)
- **Moq** (Mocking framework)
- **NUnit** (Unit testing framework)

## Setup Instructions

1. Install Docker.
2. Install .NET 8 SDK.
3. Install developer certificates to run the app locally. Use [this tutorial](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-8.0).
4. Navigate to the project `src` directory and run:

   ```
   docker compose up
	```

## Sample Usage

1. Navigate to https://localhost:6001/swagger/index.html in your browser.
2. Try the auth/signup endpoint with sample credentials.
3. Upon successful signup, try auth/signin using the same credentials. You should receive a token in the response.
4. Click Authorize and enter the token as Bearer {token}.

Test the GET, POST, PUT, DELETE endpoints for /notes.

## Screenshot

![Note management swagger demo](screen.png)
