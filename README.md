# 📦 StockSync

Welcome to **StockSync**, a minimalistic ASP.NET Core API for managing stock inventories. This project uses the power of **Newtonsoft.JSON** for JSON manipulation and **SQLite** as its database.

**This project contains a sample ASP.NET Core app. This app is an example of the article I produced for the Telerik Blog (telerik.com/blogs).**

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or Higher
- SQLite

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/zangassis/stock-sync.git
   cd stocksync
   ```

2. **Install dependencies:**

   ```bash
   dotnet restore
   ```

3. **Run the application:**

   ```bash
   dotnet run
   ```

### Configuration

StockSync uses a local SQLite database by default. Ensure that your `appsettings.json` is correctly configured:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=stockSyncDb.db"
  }
}
```

## 🛠️ Technologies Used

- **ASP.NET Core Minimal API** - Fast and minimal API framework.
- **Newtonsoft.JSON** - High-performance JSON framework for .NET.
- **SQLite** - Lightweight, file-based database.
- **EF Core**

## 🌟 Features

- Simple and intuitive API for stock management.
- Lightweight and fast with minimal setup.
- Uses Entity Framework Core with SQLite for seamless database operations.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
