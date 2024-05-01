# RestAPI example: Notes API

#### The project is an starter example of REST API based on .NET 8.

This API provides endpoints for user authentication, CRUD operations on notes, and user-related functionalities like changing passwords.
#### Paths 

1. Authentication:
    - /auth/signup: POST request for user signup
    - /auth/signin: POST request for user signin
2. Notes:
    - GET request to retrieve notes with various filtering options
    - POST request to create a new note
    - PUT request to update an existing note
    - DELETE request to delete a note by ID
3. User:
    - GET request to retrieve user information
    - POST request to change user password
#### Security

- Bearer token authentication using JWT Authorization header.

#### Tech stack and libraries

 * .NET 8
 * Entity Framework
 * PostgreSQL 
 * Automapper
 * FluentValidation
 * Identity Model JWT
 * Swagger
 * Docker
 * AutoFixture
 * Moq
 * Nunit
#### Setup instructions

 1. Install docker.
 2. Install .NET 8 SDK.
 3. Install developers certificates to able to run app locally. [Use this tutorial](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-8.0)
 4. Navigate to project ```src``` directory and run:
	```docker compose up```
#### Sample Usage
 
 1. Navigate to in the browser:
   ```https://localhost:5001/swagger/index.html```
 2. Try ```auth/signup``` out with sample credentials.
 3. After success response on 2 try ```auth/signin``` with the data from step 2. You should get ```token``` value in the response
 5. Click ```Authorize``` with the value: 'Bearer ```token```'
 6. Try GET POST PUT DELETE `/notes` endpoints.

#### Screenshot

![Note management swagger demo](screen.png)