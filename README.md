# ProductsApiManagement
Products API is a **RESTful Web API** built with `.NET 8`, **Entity Framework Core**, SQL Server. It includes basic CRUD operations, stock management, and security features such as CORS and validation.
ProductsApiManagement/
│── ProductsApi/      # Main API Project
│── ProductTest/      # Test Project
│── README.md         # Documentation
│── .gitignore        # Ignored files
1. Clone the Repository
git clone https://github.com/Venkateesh98/ ProductsApiManagement.git
cd ProductsApi
dotnet restore
dotnet build 
2. Configure the Database (SQL Server)
Before running the application, update the database connection string in `appsettings.json`:
{ "ConnectionStrings": { "DefaultConnection": "Server=YOUR-SQL-SERVER-NAME;Database=ProductsDb;Trusted_Connection=True;" } }
3. After setting up the connection string, apply migrations:
   dotnet ef database update
4. Run the API
   dotnet run (Swagger UI will be available based on localhost at- https://localhost:7257/swagger/index.html)
5. Run Tests
   dotnet test


---
## Setup Instructions
### **Prerequisites**
- Install **.NET SDK 8.0**  
- Install **SQL Server** (or use `UseInMemoryDatabase` for testing)  
- Install and Use Swagger for API testing
---

## Technologies Used
- **.NET 8**
- **Entity Framework Core** (EF Core)
- **SQL Server** (Can use In-Memory for testing)
- **xUnit & Moq** (Testing framework)
- **Swashbuckle.AspNetCore** (Swagger API documentation)

---

## Features
**Product Management**: Create, update, delete, fetch products  
**Stock Handling**: Increment or decrement stock levels   
**Validation**: Ensure required fields and correct data formats  
**Content Negotiation**: Supports both JSON & XML responses  
**CORS**: Handles cross-origin requests  
**Security Headers**: Protect against common web vulnerabilities  
**Unit & Integration Tests**: Built using **xUnit** and **Moq**  

---

---
