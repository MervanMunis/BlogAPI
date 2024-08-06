# BlogAPI Project

## Introduction
Welcome to the BlogAPI project! This project is a comprehensive blog management system designed to help authors and administrators manage blog posts, comments, and user interactions effectively. 
The project includes features such as post management, comment management, user authentication, and more. It is built with a focus on scalability, security, and ease of use.

## Project Overview
The BlogAPI project is a RESTful API built using ASP.NET Core. It provides endpoints for managing blog resources such as posts, comments, tags, authors, and categories.
The project also includes features for handling likes, bookmarks, and user roles and authentication. The API is designed to be used by different roles such as authors and administrators, 
each with specific permissions and capabilities.

## Technologies Used
This project utilizes a variety of technologies to ensure a robust and efficient system. Below is a detailed explanation of the key technologies used:

### ASP.NET Core
ASP.NET Core is a cross-platform, high-performance framework for building modern, cloud-based, internet-connected applications. It is used to build the RESTful API that powers the BlogAPI project.

### Entity Framework Core
Entity Framework Core (EF Core) is an open-source ORM (Object-Relational Mapper) for .NET. It allows developers to work with a database using .NET objects, eliminating the need for most data-access code.

### Identity Framework
The ASP.NET Core Identity framework is used to manage users, passwords, roles, and claims. It provides a complete, customizable authentication and authorization system. It integrates seamlessly with EF Core to handle the storage and retrieval of user-related data.

### JWT (JSON Web Tokens)
JWT is used for securely transmitting information between parties as a JSON object. It is used for authentication and authorization in the BlogAPI project.

### SQL Server
SQL Server is a relational database management system developed by Microsoft. It is used as the database for the BlogAPI project to store all the blog data.

### Swagger
Swagger is an open-source tool for documenting APIs. It provides a user-friendly interface to explore and test API endpoints. The BlogAPI project includes Swagger for API documentation and testing.

## Project Structure
The project is organized into several folders and files to maintain a clean and manageable structure. Here is an overview of the project structure:

```maths
  BlogAPI/ 
  ├── Auth/ 
  ├── Controllers/ 
  ├── Data/ 
  ├── DTOs 
  │ ├── Request    
  │ ├── Response     
  ├── Entities/ 
  │ ├── Models     
  │ ├── Enums     
  ├── Exceptions/ 
  ├── Migrations/ 
  ├── Services/ 
  ├── BlogAPI/ 
```

### Key Folders and Files
- **Auth/**: Contains authentication-related files.
- **Controllers/**: Contains the API controllers that handle HTTP requests.
- **DTOs/**: Contains Data Transfer Objects used for data encapsulation.
- **Data/**: Contains the database context and migration files.
- **Entities/**: Contains the entity models representing the database schema.
- **Exceptions/**: Contains custom exception classes.
- **Services/**: Contains the service classes implementing the business logic.

## Model Classes
The model classes represent the entities in the database. Here are some key model classes used in the project:

### ApplicationUser
Represents a user in the system with properties like Id, Name, Email, and more.

## Endpoints
The BlogAPI provides various endpoints for managing blog resources. Here is a table of the key endpoints and their purposes:

### Author Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/authors                  | Creates a new author                                 |
| PUT         | /api/authors                  | Updates an author's details                          |
| GET         | /api/authors/byId             | Retrieves the currently logged-in author by ID       |
| GET         | /api/authors/username         | Retrieves an author by username                      |
| GET         | /api/authors/all              | Retrieves all authors                                |
| PATCH       | /api/authors/deactivate       | Deactivates an author                                |
| PATCH       | /api/authors/activate         | Activates an author                                  |
| PATCH       | /api/authors/ban              | Bans an author                                       |
| POST        | /api/authors/add/profile-image | Adds a profile image for the author                 |
| DELETE      | /api/authors/remove/profile-image | Removes the profile image of the author          |
| GET         | /api/authors/image            | Retrieves the image of an author                     |
| POST        | /api/authors/forgot-password  | Initiates the forgot password process                |
| POST        | /api/authors/reset-password   | Resets the password using a token                    |

### Bookmark Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/bookmarks/{postId}       | Adds a bookmark to a post                            |
| DELETE      | /api/bookmarks/{postId}       | Removes a bookmark from a post                       |
| GET         | /api/bookmarks                | Retrieves bookmarks by the logged-in author          |

### Category Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| GET         | /api/categories               | Retrieves all categories                             |
| GET         | /api/categories/{id}          | Retrieves a specific category by ID                  |
| POST        | /api/categories               | Creates a new category                               |
| PUT         | /api/categories/{id}          | Updates a category's details                         |
| PATCH       | /api/categories/{id}/status/inactive | Sets a category's status to inactive          |
| PATCH       | /api/categories/{id}/status/active | Sets a category's status to active              |

### Comment Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/comments                 | Adds a comment to a post                             |
| POST        | /api/comments/{parentCommentId}/replies | Adds a reply to a comment                  |
| GET         | /api/comments/post/{postId}   | Retrieves comments by post ID                        |
| GET         | /api/comments/{commentId}/replies | Retrieves replies by comment ID                  |
| PUT         | /api/comments/{commentId}     | Updates a comment                                    |
| DELETE      | /api/comments/{commentId}     | Deletes a comment (marks it as deleted if it has replies) |

### Comment Like Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/commentlikes/{commentId} | Likes a comment                                      |
| DELETE      | /api/commentlikes/{commentId} | Unlikes a comment                                    |
| GET         | /api/commentlikes/{commentId} | Retrieves likes by comment ID                        |

### Follow Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/follows/{followedAuthorId} | Follows an author                                  |
| DELETE      | /api/follows/{followedAuthorId} | Unfollows an author                                |
| GET         | /api/follows/followers/{authorId} | Retrieves followers by author ID                 |
| GET         | /api/follows/following/{authorId} | Retrieves authors followed by author ID          |

### Like Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/likes/{postId}           | Likes a post                                         |
| DELETE      | /api/likes/{postId}           | Unlikes a post                                       |

### Post Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/posts                    | Creates a new post                                   |
| PUT         | /api/posts/{postId}           | Updates a post's details                             |
| GET         | /api/posts/{postId}           | Retrieves a specific post by ID                      |
| GET         | /api/posts/tag/{tagName}      | Retrieves posts by tag name                          |
| GET         | /api/posts                    | Retrieves all posts                                  |
| GET         | /api/posts/active             | Retrieves active posts                               |
| GET         | /api/posts/banned             | Retrieves banned posts                               |
| GET         | /api/posts/deactivated        | Retrieves deactivated posts                          |
| PATCH       | /api/posts/{postId}/ban       | Bans a post                                          |
| PATCH       | /api/posts/{postId}/deactivate | Deactivates a post                                  |
| PATCH       | /api/posts/{postId}/activate  | Activates a post                                     |
| POST        | /api/posts/{postId}/image     | Adds an image to a post                              |
| DELETE      | /api/posts/{postId}/image     | Removes an image from a post                         |

### Reading List Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/readinglists/{postId}    | Adds a post to the reading list                      |
| DELETE      | /api/readinglists/{postId}    | Removes a post from the reading list                 |
| GET         | /api/readinglists             | Retrieves reading list by author ID                  |

### SubCategory Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| GET         | /api/subcategories            | Retrieves all subcategories                          |
| GET         | /api/subcategories/{id}       | Retrieves a specific subcategory by ID               |
| GET         | /api/subcategories/{id}/posts | Retrieves posts by subcategory ID                    |
| POST        | /api/subcategories            | Creates a new subcategory                            |
| PUT         | /api/subcategories/{id}       | Updates a subcategory's details                      |
| PATCH       | /api/subcategories/{id}/status/inactive | Sets a subcategory's status to inactive    |
| PATCH       | /api/subcategories/{id}/status/active | Sets a subcategory's status to active        |

### Authentication Endpoints
| HTTP Method | Endpoint                      | Purpose                                              |
|-------------|-------------------------------|------------------------------------------------------|
| POST        | /api/authentication/login     | Logs in a user and returns a JWT token               |
| POST        | /api/authentication/logout    | Logs out the currently logged-in user                |

## Running the Non-Dockerized Version

To run the non-dockerized version of the project, follow these steps:

1. Clone the non-dockerized branch:

```sh 
git clone -b non-dockerized https://github.com/MervanMunis/BlogAPI.git
```

2. Open the solution in Visual Studio.

3. Update the connection string in appsettings.json to point to your local SQL Server instance.

4. Apply the migrations:

```sh 
dotnet ef migrations add Initial
```
5. Apply the migrations:

```sh 
dotnet ef database update
```

## Developer Guide for Testing the Project

To test the BlogAPI project, follow these steps:

### Step 1: Login as Admin
1. **Login**:
   * Endpoint: `POST /api/authentication/login`
   * JSON Body:
```json
{
    "email": "admin@admin.com",
    "password": "Admin1234!"
}
```
  * Copy the token from the response.
    
2. Authorize:
  * Click on the "Authorize" button in Swagger.
  * Paste the token and click "Authorize".

### Step 2: Create Categories and SubCategories

#### 1. Create a Category:
  * Endpoint: POST /api/categories
  * JSON Body:
```json
{
    "name": "Technology"
}

```
#### 2. Create a SubCategory
  * Endpoint: POST /api/subcategories
  * JSON Body:
```json
{
    "name": "Programming",
    "categoryId": 1
}
```

### Step 3: Author Actions
#### 1. Create an Author Account: 
  * Endpoint: POST /api/authors
  * JSON Body:
```json
{
    "name": "John",
    "lastName": "Doe",
    "email": "johndoe@example.com",
    "password": "Password123!",
    "confirmPassword": "Password123!",
    "birthDate": "1990-01-01",
    "gender": "Male"
}
```
#### 2. Login as Author:
   * Repeat the login steps using the author's credentials.

#### 3. Follow an Author:
   * Endpoint: POST /api/follows/{followedAuthorId}
   * URL Parameter: followedAuthorId = "someAuthorId"


### Step 4: Create and Interact with Posts
#### 1. Create Post
  * Endpoint: POST /api/posts
  * JSON Body:
```json
{
    "title": "My First Post",
    "content": "This is the content of my first post.",
    "tags": ["Introduction", "FirstPost"],
    "subCategoryIds": [1]
}
```

#### 2. Comment on a Post:
  * Endpoint: POST /api/comments
  * JSON Body:
```json
{
    "content": "Great post!",
    "postId": 1
}
```

#### 3. Like a Post:
  * Endpoint: POST /api/likes/{postId}
  * URL Parameter: postId = 1

#### 4. Like a Comment:
  * Endpoint: POST /api/commentlikes/{commentId}
  * URL Parameter: commentId = 1

### Step 5: Bookmark and Add to Reading List
#### 1. Add to Reading List:
  * Endpoint: POST /api/readinglists/{postId}
  * URL Parameter: postId = 1

#### 2. Bookmark a Post:
  * Endpoint: POST /api/bookmarks/{postId}
  * URL Parameter: postId = 1

By following these steps, you can effectively test the key features of the BlogAPI project. This guide helps you understand the flow of actions from setting up the admin, 
creating categories, registering an author, and engaging with the blog content.

## Conclusion

The BlogAPI project provides a robust and scalable solution for managing blog resources. With its comprehensive set of features, modern technology stack, and detailed documentation, 
it is well-suited for real-world blog environments.

## Acknowledgements

This project was developed as part of the backend program at [Softito Yazılım - Bilişim Akademisi](https://softito.com.tr/index.php). Special thanks to the instructors and peers who provided valuable feedback and support throughout 
the development process.
