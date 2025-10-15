# 🚗 AutoAuctionPro

**AutoAuctionPro** is a simple car auction management system.  
It allows managing vehicles, creating auctions, place bids, and closing auctions to determine the winner.  
Developed in **.NET 9** with **PostgreSQL** and **Entity Framework Core**, following **Domain-Driven Design (DDD)** principles and a clean architecture approach.

---

## 📦 Main Features

- Add vehicles to the inventory  
- Start auctions for vehicles  
- Place bids on active auctions  
- Close auctions and determine the winner  
- REST API with Swagger documentation  
- Unit tests using xUnit  

---

## ⚙️ Tech Stack

| Layer | Technologies |
|:--|:--|
| **Backend** | .NET 9, ASP.NET Core Web API |
| **ORM / DB** | Entity Framework Core, PostgreSQL |
| **Testing** | xUnit |
| **Documentation** | Swagger (Swashbuckle) |
| **Configuration** | Environment variables (.env) |

---

## 🚀 Running the Project

### 1. Prerequisites
- .NET 9 SDK  
- PostgreSQL (you can point to an already configured server)  
- Visual Studio  

### 2. Database Configuration

Create a PostgreSQL database or use an existing one, and define the variables in the `.env` file:
```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=AutoAuctionPro
DB_USER=postgres
DB_PASS=password
```
This `.env` file should exist in both the WebApi and the Tests projects.

### 3. Apply Migrations

`dotnet ef database update`

### 4. Run the Application

`dotnet run --project AutoAuctionPro.WebApi`

The API will be available at:  
https://localhost:7278/swagger/

---

## 🧪 Unit Tests

Tests are located in `/AutoAuctionPro.Tests`.

The tests cover:
- Vehicle creation  
- Auction start  
- Bid submission  
- Auction closure  
- Expected error cases (duplicate vehicle, non-existent auction, etc.)
