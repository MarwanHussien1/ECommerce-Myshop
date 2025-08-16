# Myshop.Web
📌 Overview
This project is a full-stack ASP.NET Core MVC web application designed to manage customer orders and cart functionality.
It includes:
Product listing with dynamic pricing.
Shopping cart functionality (add/remove/update).
Checkout process with order summary.
Integration with Entity Framework Core for data persistence.
Clean UI built with Bootstrap for responsiveness.

🚀 Features
🛍️ Cart Management – Add, update, and remove products.
📦 Order Placement – Place an order and view order details.
🔑 User Authentication (Identity) – Secure login/register for users.
💳 Checkout – Pickup details and payment integration ready.
📊 Admin Panel (optional) – Manage products, users, and orders.

🛠️ Technologies Used
ASP.NET Core MVC – Backend framework.
Entity Framework Core – ORM for database operations.
SQL Server – Database.
Bootstrap 5 – Frontend styling.
C# – Core programming language.

⚙️ Setup Instructions
1️⃣ Clone the Repository
git clone [https://github.com/MarwanHussien1/ECommerce-Myshop.get](https://github.com/MarwanHussien1/ECommerce-Myshop)
cd ECommerce-Myshop

2️⃣ Configure the Database
Open appsettings.json.
Update the connection string with your SQL Server instance:
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=OrderSystemDB;Trusted_Connection=True;MultipleActiveResultSets=true"
}

3️⃣ Apply Migrations
dotnet ef database update

4️⃣ Run the Application
dotnet run


The app will run on:
👉 https://localhost:5001 (or configured port).
