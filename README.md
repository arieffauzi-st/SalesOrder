# How to run locally

**Prerequisites**
1. **Docker installed on your machine.**
2. **.NET SDK installed on your machine.**

## Steps
Clone the repository or download the ZIP project:

```bash
git clone https://github.com/arieffauzi-st/SalesOrder.git
```
Run SQL Server in Docker as containerized database services

```bash
docker-compose up -d
```
After Docker is running for SQL Server, navigate to the project directory and run the application with the following command:

```bash
dotnet run
```
Once the application is running, open your browser and navigate to:

```bash
https://localhost:5000
```
You can also see the URL provided in the terminal (CMD or PowerShell) after running the application.

## Generate SQL Schema
Open a terminal or command prompt and navigate to the directory of your project.
Entity Framework Core Tools installed. If it's not, run this below:
```bash
dotnet tool install --global dotnet-ef
```
Generate SQL Script: Run the following command to generate the SQL script from migrations:
```bash
dotnet ef migrations script
```
**OR** Save SQL Script to File, run this command:

```bash
dotnet ef migrations script -o schema.sql
```
