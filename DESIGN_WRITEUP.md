# 🧩 Design Write-Up — AutoAuctionPro

## 1. Overview

**AutoAuctionPro** was designed as a modular and scalable car auction system using **.NET 9** and **PostgreSQL**, following **Domain-Driven Design (DDD)** and **Clean Architecture** principles.  
The goal is to provide a maintainable codebase with clear separation between business rules, data persistence, and API delivery.

---

## 2. Architecture Overview

### Layers

| Layer | Description |
|:--|:--|
| **Domain** | Core business entities (`Vehicle`, `Auction`, `Bid`) and domain logic. |
| **Infrastructure** | Handles data persistence using **Entity Framework Core** and **PostgreSQL**. |
| **Application** | Coordinates use cases like starting auctions, placing bids, and closing auctions. |
| **WebApi** | REST endpoints, DTOs, and Swagger documentation. |
| **Tests** | Unit tests using **xUnit** to validate success and failure flows. |

### Design Goals

- **Modularity:** Replaceable layers with minimal coupling  
- **Testability:** Core logic testable with in-memory or PostgreSQL DB  
- **Maintainability:** Ready to scale with new features

---

## 3. Entities

### Vehicle

Base entity with:
- `Id`, `Type`, `Manufacturer`, `Model`, `Year`, `StartingBid`, `IsSold`  
Concrete types like `Sedan`, `SUV`, `Hatchback` and `Truck` extend this base class.

The user can set the vehicle ID. If not, it will be set automatically.

### Auction

Represents an auction’s lifecycle:
- `Id`, `VehicleId`, `StartingBid`, `IsActive`, `OpenDateUTC`, `CloseDateUTC`, `AmountSold`, `WinnerBidder` and `Bids`

### Bid

Represents a user’s bid:
- `Id`, `AuctionId`, `BidderName`, `Amount`, `BidTimeUTC`

---

## 4. Core Services

### VehicleService
Handles:
- Adding new vehicles
- Marking vehicles as sold
- Get vehicle by ID
- Get a list of vehicles with the ability to filter by:
    `Type`, `Manufacture`, `Model`, `Year` and `isSold`

### AuctionService
Handles:
- Starting auctions  
- Place bids  
- Closing and determining auction winner
- Get a list of auctions
- Get an auction by vehicle ID

Key rules:
- One active auction per vehicle  
- Bids must increase sequentially  
- Highest bid wins

---

## 5. Repositories

Abstraction over persistence layer for testability:
- `VehicleRepository`
- `AuctionRepository`

Example:
```csharp
public interface IVehicleRepository
{
    Task<Vehicle> AddAsync(Vehicle vehicle);
    Task<Vehicle> UpdateAsync(Vehicle auction);
    Task<Vehicle?> GetByIdAsync(string id);
    Task<bool> ExistsAsync(string id);
    Task<IEnumerable<Vehicle>> GetAllAsync();
}
```

---

## 6. Database Configuration

```env
POSTGRES_HOST=localhost
POSTGRES_PORT=5432
POSTGRES_DB=AutoAuctionPro
POSTGRES_USER=postgres
POSTGRES_PASSWORD=password
```

EF Core migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 7. Testing

### Framework
Implemented with **xUnit**, supporting async database operations.

### Structure
- **Success flows:** normal use cases  
- **Failure flows:** invalid states  
- **Full simulation:** end-to-end lifecycle

Example test:
```csharp
[Fact]
public async Task TestAllFlow()
{
    AuctionService auctionService = new AuctionService(_vehicleRepo, _auctionRepo);
    VehicleService vehicleService = new VehicleService(_vehicleRepo);

    var car = new Sedan("Mercedes-Benz", "CLA45 AMG", 2015, 15000, 5);
    await vehicleService.AddAsync(car);

    await auctionService.StartAuctionAsync(car.Id);

    await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 17000);
    await auctionService.PlaceBidAsync(car.Id, "Ana Maria", 18000);
    await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 19500);
    await auctionService.PlaceBidAsync(car.Id, "João Simões", 20000);
    await auctionService.PlaceBidAsync(car.Id, "Frederico Santos", 21000);

    Auction auctionClosed = await auctionService.CloseAuctionAsync(car.Id);

    Assert.Equal("Frederico Santos", auctionClosed.WinnerBidder);
    Assert.Equal(21000, auctionClosed.AmountSold);
}
```

---

## 8. Design Decisions

| Decision | Reason |
|:--|:--|
| **Domain-first design** | Keeps business logic isolated |
| **Repository pattern** | Improves testability |
| **Async operations** | Non-blocking I/O |
| **xUnit** | Lightweight, integrated with .NET tooling |
| **PostgreSQL** | Stable, production-ready DB |

---

## 9. Future Improvements

- Add authentication and roles  
- Background jobs for automatic auction closure  
- Caching of active auctions  
- Improvement in the log system 
- Frontend with React
- Improve the auction list retrieval system by adding filters.
- Improve the auction list retrieval system to support pagination.

---

## 10. Conclusion

**AutoAuctionPro** demonstrates a clean DDD approach ready for production environments.  
Its modular structure supports scalability, extensibility, and maintainability.
