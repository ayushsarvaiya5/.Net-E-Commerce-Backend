# .NET E-Commerce Backend

A robust and scalable e-commerce backend API built with ASP.NET Core, following clean architecture principles and modern development practices.

## Overview

This project implements a complete backend solution for an e-commerce platform with features including:

- User authentication and authorization
- Product catalog management
- Shopping cart functionality
- Order processing
- Payment integration
- Review and rating system
- Role-based access control

## Tech Stack

- **Framework**: ASP.NET Core 6.0
- **Database**: Entity Framework Core with SQL Server
- **Authentication**: JWT Token-based authentication
- **Design Pattern**: Clean Architecture, Repository Pattern, CQRS
- **API Documentation**: Swagger/OpenAPI
- **Validation**: FluentValidation
- **Testing**: xUnit, Moq

## Architecture

The solution follows Clean Architecture principles, organized into the following projects:

- **API**: REST API endpoints, controllers, and API configuration
- **Application**: Application services, DTOs, CQRS commands/queries, and business logic
- **Domain**: Core business entities, interfaces, and domain logic
- **Infrastructure**: Data access, external services integration, and implementation details

## Features

### User Management
- User registration and login
- JWT authentication
- Role-based authorization (Admin, Customer)
- User profile management

### Product Management
- Product CRUD operations
- Product categorization
- Product search and filtering
- Product images

### Order Management
- Shopping cart functionality
- Order creation and processing
- Order history and status tracking

### Payment Processing
- Secure payment integration
- Multiple payment method support
- Transaction history

### Review System
- Product ratings and reviews
- Review moderation

## Getting Started

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. Clone the repository
   ```
   git clone https://github.com/ayushsarvaiya5/.Net-E-Commerce-Backend.git
   cd .Net-E-Commerce-Backend
   ```

2. Update the connection string in `appsettings.json` to point to your SQL Server instance

3. Apply database migrations
   ```
   dotnet ef database update
   ```

4. Run the application
   ```
   dotnet run --project src/API
   ```

5. Access the API documentation at `https://localhost:5001/swagger`

### Configuration

The application can be configured through:

- `appsettings.json` - Main application settings
- `appsettings.Development.json` - Development-specific settings
- Environment variables

Key configuration sections:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ECommerceDb;Trusted_Connection=True;"
  },
  "JwtSettings": {
    "Secret": "YourSecretKeyHere",
    "ExpiryMinutes": 60,
    "Issuer": "ECommerceApi",
    "Audience": "ECommerceClients"
  }
}
```

## API Documentation

The API is documented using Swagger/OpenAPI. Once the application is running, access the interactive documentation at:

```
https://localhost:5001/swagger
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Authenticate user and get JWT token

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product (Admin)
- `PUT /api/products/{id}` - Update product (Admin)
- `DELETE /api/products/{id}` - Delete product (Admin)

### Categories
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category (Admin)
- `PUT /api/categories/{id}` - Update category (Admin)
- `DELETE /api/categories/{id}` - Delete category (Admin)

### Shopping Cart
- `GET /api/cart` - Get current user's cart
- `POST /api/cart/items` - Add item to cart
- `PUT /api/cart/items/{id}` - Update cart item quantity
- `DELETE /api/cart/items/{id}` - Remove item from cart

### Orders
- `GET /api/orders` - Get user's orders
- `GET /api/orders/{id}` - Get order details
- `POST /api/orders` - Create new order from cart
- `PUT /api/orders/{id}/status` - Update order status (Admin)

### Reviews
- `GET /api/products/{id}/reviews` - Get product reviews
- `POST /api/products/{id}/reviews` - Add product review
- `PUT /api/reviews/{id}` - Update review
- `DELETE /api/reviews/{id}` - Delete review

## Testing

The project includes unit tests and integration tests.

To run the tests:

```
dotnet test
```

## Deployment

This application can be deployed to:

- Azure App Service
- Docker containers
- IIS
- Any platform supporting ASP.NET Core

### Docker Support

Build and run using Docker:

```
docker build -t ecommerce-api .
docker run -p 8080:80 ecommerce-api
```

## Roadmap

- [ ] Enhanced analytics and reporting
- [ ] Advanced search functionality
- [ ] Wishlist management
- [ ] Product recommendation engine
- [ ] Multi-language support
- [ ] Multi-currency support

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

Ayush Sarvaiya - [GitHub](https://github.com/ayushsarvaiya5)

Project Link: [https://github.com/ayushsarvaiya5/.Net-E-Commerce-Backend](https://github.com/ayushsarvaiya5/.Net-E-Commerce-Backend)
