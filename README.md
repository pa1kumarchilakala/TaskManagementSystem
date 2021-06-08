# TaskManagementSystem

1. Technology stack used

    Framework - .Net 5
    Frontend - ASP.Net Core 5
    Backend - 9
    Web API - .Net 5
    API Gateway - .Net 5
    Database - MS SQL Server
    ORM - EntityFrameworkCore 5.0

2. Application is created using dotnet cli.

3. Application is divided into multiple layers

    1. database
          - DDL
          - DML
    2. src
          - TaskManagementSystem.API
          - TaskManagementSystem.APIGateWay
          - TaskManagementSystem.ApplicationCore
          - TaskManagementSystem.DependencyInjection
          - TaskManagementSystem.Domain
          - TaskManagementSystem.Infrastructure
          - TaskManagementSystem.UI
    3. tests
          - TaskManagementSystem.APITests

