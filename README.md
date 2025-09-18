# 🏥 MedicalApp

A .NET 9 Web API project for managing medical appointments with authentication, role-based authorization, and CRUD operations for doctors, patients, and appointments.

---

## 📦 Technologies Used
- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Identity (Authentication & Authorization with Roles)
- AutoMapper
- Serilog (Logging)
- Swagger (API Documentation)

---

## ▶️ Run the Project

### 1️⃣ Clone the repository
```bash
git clone https://github.com/sara-yasserr/MedicalApp.git
cd MedicalApp
```

### 2️⃣ Configure the database
Update the **connection string** in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=.; Initial Catalog=MedicalAppDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"
}
```

### 3️⃣ Apply migrations & update database
```bash
dotnet ef database update --project MedicalApp.DA --startup-project MedicalApp.API
```

### 4️⃣ Build the solution
```bash
dotnet build
```

### 5️⃣ Run the API
```bash
dotnet run --project MedicalApp.API
```

API available at:  
- `https://localhost:57466`  
- `http://localhost:57467`  

---

## 🔑 Authentication & Authorization

- JWT Bearer Authentication is used.  
- Roles: **Admin**, **Doctor**, **Patient**.  

To use secured endpoints:
1. Login with valid credentials to get a JWT token.  
2. In Swagger click **Authorize 🔒** and enter:  
   ```
   Bearer <your_token>
   ```
3. Or send in Postman with header:  
   ```
   Authorization: Bearer <your_token>
   ```

---

## 📂 Project Structure
```
MedicalApp/
│── MedicalApp.API        → Presentation Layer (Controllers, Swagger, Authentication)
│── MedicalApp.BL         → Business Logic Layer (Services, DTOs, AutoMapper)
│── MedicalApp.DA         → Data Access Layer (DbContext, Entities, Repositories, Identity)
│── README.md             → Project Documentation
│── .gitignore
```

---

## 📝 Features
- User Registration & Login with JWT
- Role-based Authorization (Admin / Doctor / Patient)
- CRUD for Doctors & Patients
- Appointment Scheduling & Management
- Logging with Serilog
- Swagger UI Documentation

---

## 🤝 Contribution
1. Fork the repo  
2. Create a new branch (`feature/YourFeature`)  
3. Commit your changes  
4. Push the branch  
5. Create a Pull Request  

---

## 📜 License
This project is licensed under the MIT License.
