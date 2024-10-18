
# SohatNotebook Project

---

## Overview

**SohatNotebook** is a web-based application designed to offer a robust platform for note-taking and management. The application provides a comprehensive API that allows users to create, manage, and organize their notes efficiently. This README will cover the project structure, detailed explanation of each API endpoint, features, and what makes this project unique.

## Project Structure

The project is organized into the following main components:

1. **SohatNotebook.Api**: This is the main API project that handles all the backend logic and database interactions.
2. **SohatNotebook.Entities**: This contains the entity models that represent the data structures used in the application.
3. **SohatNotebook.Services**: This layer contains the business logic and services that interface between the API controllers and the data entities.
4. **SohatNotebook.Web**: This is the front-end project, which provides the user interface for interacting with the application.

## API Endpoints

### 1. Authentication

- **POST /api/auth/register**
  - **Description**: Registers a new user.
  - **Request Body**: 
    ```json
    {
      "username": "string",
      "password": "string",
      "email": "string"
    }
    ```
  - **Response**:
    ```json
    {
      "userId": "string",
      "username": "string",
      "email": "string"
    }
    ```

- **POST /api/auth/login**
  - **Description**: Authenticates a user and returns a JWT token.
  - **Request Body**:
    ```json
    {
      "username": "string",
      "password": "string"
    }
    ```
  - **Response**:
    ```json
    {
      "token": "string"
    }
    ```

### 2. Notes

- **GET /api/notes**
  - **Description**: Retrieves all notes for the authenticated user.
  - **Response**:
    ```json
    [
      {
        "id": "string",
        "title": "string",
        "content": "string",
        "createdAt": "string",
        "updatedAt": "string"
      }
    ]
    ```

- **GET /api/notes/{id}**
  - **Description**: Retrieves a specific note by ID.
  - **Response**:
    ```json
    {
      "id": "string",
      "title": "string",
      "content": "string",
      "createdAt": "string",
      "updatedAt": "string"
    }
    ```

- **POST /api/notes**
  - **Description**: Creates a new note.
  - **Request Body**:
    ```json
    {
      "title": "string",
      "content": "string"
    }
    ```
  - **Response**:
    ```json
    {
      "id": "string",
      "title": "string",
      "content": "string",
      "createdAt": "string",
      "updatedAt": "string"
    }
    ```

- **PUT /api/notes/{id}**
  - **Description**: Updates an existing note by ID.
  - **Request Body**:
    ```json
    {
      "title": "string",
      "content": "string"
    }
    ```
  - **Response**:
    ```json
    {
      "id": "string",
      "title": "string",
      "content": "string",
      "createdAt": "string",
      "updatedAt": "string"
    }
    ```

- **DELETE /api/notes/{id}**
  - **Description**: Deletes a note by ID.
  - **Response**: 
    ```json
    {
      "message": "Note deleted successfully"
    }
    ```

### 3. User Profile

- **GET /api/users/profile**
  - **Description**: Retrieves the authenticated user's profile.
  - **Response**:
    ```json
    {
      "userId": "string",
      "username": "string",
      "email": "string",
      "createdAt": "string"
    }
    ```

- **PUT /api/users/profile**
  - **Description**: Updates the authenticated user's profile.
  - **Request Body**:
    ```json
    {
      "username": "string",
      "email": "string"
    }
    ```
  - **Response**:
    ```json
    {
      "userId": "string",
      "username": "string",
      "email": "string",
      "updatedAt": "string"
    }
    ```

## Features

- **User Authentication**: Secure user registration and login using JWT tokens.
- **CRUD Operations for Notes**: Users can create, read, update, and delete notes.
- **User Profile Management**: Users can view and update their profile information.
- **Responsive Web Interface**: A user-friendly front-end interface for managing notes.

## Unique Selling Points

- **Secure and Scalable**: Built with a focus on security and scalability, making it suitable for deployment in production environments.
- **Modern Technology Stack**: Utilizes the latest .NET technologies and follows best practices for API development.
- **User-Centric Design**: Prioritizes user experience with intuitive interfaces and seamless interactions.
- **Comprehensive Documentation**: Detailed API documentation to facilitate easy integration and usage.

## Getting Started

To get started with the SohatNotebook project, follow these steps:

1. **Clone the Repository**:
   ```bash
   git clone <repository_url>
   ```
2. **Navigate to the Project Directory**:
   ```bash
   cd SohatNotebook-master
   ```
3. **Build the Project**:
   ```bash
   dotnet build
   ```
4. **Run the Project**:
   ```bash
   dotnet run --project SohatNotebook.Api
   ```
5. **Access the Web Interface**:
   Open a web browser and navigate to `http://localhost:<port>` to access the SohatNotebook web interface.

## Contributing

We welcome contributions to the SohatNotebook project. Please fork the repository and submit pull requests for any features, bug fixes, or improvements.

## License

This project is licensed under the MIT License. See the LICENSE file for more details.

---
