# Task Management System - Document of implementation and complete status of the challenge.

Please find the information below on Task Management System challenge,

1. Technology stack used

    1. Framework - .Net 5
    2. Language - C#
    3. Web API - ASP.NET Core 5
    4. API Gateway - Ocelot (17.0)
    5. Database - MS SQL Server 2016
    6. ORM - EntityFrameworkCore 5.0 (for quicker CRUD operations. However, I have experience with writing stored procedures)
    7. Automapper is used to mapping the entities. This is used to avoid the mapping of ViewModel and Domain Model

3. Application is divided into multiple layers using Clean Architecture. Please find the brief about each layer and what it consists.

  You will find three folders under the solutions as below. The context of each folder is explained below.
  
    1. database
          - DDL (Database & Tables)
          - DML (INSERT statements for StatusLookup table)
          - Screenshots (Screenshots of Tasks and StatusLookup tables from Database)
    2. src
          - TaskManagementSystem.API
                - Created a microservice with Http Methods for POST, GET & PUT operations. DELETE method is not implemented per requirement.
                
          - TaskManagementSystem.APIGateWay
                - Created a gateway using Ocelot to call the microservice.
                
          - TaskManagementSystem.ApplicationCore
                - Implemented the services, View Model mapping and business functionality under this layer.
                
          - TaskManagementSystem.DependencyInjection
                - Have injected the interfaces of services, repositories, Dbcontext & AutoMapperProfile
                
          - TaskManagementSystem.Domain
                - This consists the contracts or interfaces for Repositories and domain models are handled here.     
                
          - TaskManagementSystem.Infrastructure
                - Database interactions are handled here. DatabaseContext and respective Repository for CRUD operations using Entity Framework Core.
                
          - TaskManagementSystem.UI
                - Have tried to implement the UI part to handle the tasks on UI. To avoid the further delay of submitting the challenge this part is not implemented 100%.
                
    3. tests
          -UnitTests
            - TaskManagementSystem.APITests
                - Implemented multiple test cases for microservice. To be honest, I have limited experience with TDD due to my earlier projects. However, I have tried my best                   to accomplish. Feedback on this will improve my knowledge and skills.
          -IntegrationTests
            - TaskManagementSystem.GatewayTests
                - Same as above.


4. Challenge requirement.

    1. Database for storing information about tasks, use MS SQL Server 2012+ 
        - Completed. Used MS SQL Server 2016 (localdb) and saved the related data to the Tasks table. Please find the screenshots of data under database folder.
    2. Web API for CRUD operations, use ASP.NET MVC 5 or ASP.NET Core
        - Completed. Implemented for Create, Read & Update operations. Delete is out of scope from the challenge.
    3. Web API for retrieving report with list of all tasks with state inProgress for some date in .csv format, use ASP.NET MVC
       5 or ASP.NET Core
       - Implemented API method to pull the tasks based on any status (Completed, Inprogress & Planned). 
