# SensitiveWordsAPI

## 📖 Description

`SensitiveWordsAPI` is a .NET Core Web API that filters sensitive words from user-provided input. For example, if the word **"select"** is considered sensitive, and the client sends `"select this"`, the response will be `"****** this"`.

It also includes internal endpoints for managing the list of sensitive words stored in an MS SQL Server Express database.

---

## 🚀 Features

- ✅ Sanitize input strings by masking sensitive words
- 🛠 Admin API to manage (add/update/delete) sensitive words
- 💾 Persists sensitive words in SQL Server Express
- 📄 Swagger UI available for API testing

---

## 🧰 Tech Stack

- ASP.NET Core Web API
- SQL Server 2022 Express
- Dapper
- Swagger (Swashbuckle)

---

## 🖥️ Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server 2022 Express](https://go.microsoft.com/fwlink/p/?linkid=2216019)
- SQL Management tool (SSMS, Azure Data Studio, etc.)
- IDE (Visual Studio)

### ⚙️ Installation

1. **Install SQL Server Express 2022**
2. **Run the DB initialization script:**
   Docs\sql\sql-init.sql
3. **Make sure to change the DefaultConnectionString information to reflect your username and password for the database in the appsetting.json file and to grant execute permissions for that user account if necessary.**

## 🧪 Testing

### 🔧 Prerequisites
- Ensure the project is running using **IIS Express** (default launch profile)
- The API should be accessible at `https://localhost:{port}`

### 🧬 Testing Options
- ✅ **Swagger UI**: Navigate to `/swagger` in your browser to interact with and test the API endpoints directly.
- 🧾 **HTTP File**: Use the included `.http` file (e.g., `SensitiveWordsAPI.http`) to test endpoints. This only works in Visual Studio.