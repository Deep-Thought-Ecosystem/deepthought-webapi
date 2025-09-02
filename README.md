# WebApi

ASP.NET Core Web API project using **MS SQL Server** as the database and **Scalar** for API documentation (instead of Swagger).

---

## 🚀 Features
- .NET 9 Web API
- Entity Framework Core with SQL Server
- API documentation via **Scalar**
- Clean Architecture style (Api, Core, Infrastructure, Contracts)
- Dockerized SQL Server 2022 for development

---

## 🛠️ Requirements
- [.NET SDK](https://dotnet.microsoft.com/download) (6.0+ or 7.0/8.0 depending on project)
- [Docker](https://www.docker.com/get-started)
- SQL Server client (Azure Data Studio, SQL Server Management Studio, or `sqlcmd`)

---

## 📦 Database Setup with Docker

Start SQL Server 2022 in a container:


```
docker run \
  --platform=linux/amd64 \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=123456@Angelo" \
  -e "MSSQL_PID=Evaluation" \
  -p 1433:1433 \
  --name sqlpreview \
  --hostname sqlpreview \
  -d mcr.microsoft.com/mssql/server:2022-latest
```
```
•	Port: 1433
•	User: sa
•	Password: 123456@Angelo
```
Connection String
```
Server=localhost,1433;Database=WebApi42Db;User Id=sa;Password=123456@Angelo;TrustServerCertificate=True;
```

⸻

▶️ Running the API
	1.	Restore packages:
```
dotnet restore
```

2.	Apply migrations:
```
dotnet ef database update --project WebApi42.Infrastructure
```

3.	Run the API:
```
dotnet run --project WebApi42.Api
```


⸻

📑 API Documentation (Scalar)

Scalar replaces Swagger as the API explorer UI.
	•	When the app is running, open:
http://localhost:5000/scalar/v1 (or whichever port you configured)

⸻

📂 Project Structure

WebApi42.sln
 ├── WebApi42.Api           → Presentation layer (controllers, DI, Scalar docs)
 ├── WebApi42.Core          → Domain & application logic
 ├── WebApi42.Infrastructure→ Data access (EF Core, repositories, migrations)
 └── WebApi42.Contracts     → DTOs and request/response models


⸻

🤝 Contributing
	1.	Fork the repo
	2.	Create a feature branch (git checkout -b feature/my-feature)
	3.	Commit changes (git commit -m 'Add feature')
	4.	Push branch (git push origin feature/my-feature)
	5.	Create a Pull Request

⸻

📜 License

MIT License. See LICENSE for details.

