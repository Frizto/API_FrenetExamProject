# Frenet Exam Project

## Table of Contents
1. [Introduction](#introduction)
2. [Prerequisites](#prerequisites)
3. [SQL Tables Setup](#sql-tables-setup)
4. [Environment Setup](#environment-setup)
5. [Running the Application](#running-the-application)
6. [Diagrams](#diagrams)
7. [Testing](#testing)
8. [Contributing](#contributing)
9. [License](#license)

## Introduction
Welcome to the project! This document provides a comprehensive guide to setting up and running the application. It includes instructions for setting up the database, configuring the environment, running the application, and more.

## Prerequisites
Before you begin, ensure you have the following software and tools installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio](https://visualstudio.microsoft.com/)

## SQL Tables Setup
### Step 1: Create Database
Run the necessary SQL scripts to create the database and tables, inside the 'Misc' folder you'll find several SQL Scripts execute them in this order:

CLIENT DATA
a. Run 'SQLQuery_AspNetUserIdentity_Tables' to generate the AspNetUser Tables, we're going to use it to user Auth.
b. Run 'SQLQuery_Clients_Address_Shipments_Table' to generate the Clients/Addresses/Shipments Tables, we're going to use it to store clients data.

LOGS DATA
a. Run 'SQLQuery_NLog_Table' to generate the NLog main table, this table is mutable by the other tables, storing only the last data value.
b. Run 'SQLQuery_NLog_Create_Table' & 'SQLQuery_NLog_Update_Table' & 'SQLQuery_NLog_Delete_Table' these tables only insert new logs and serves as audit trails.
c. Run the respectives '_StoredProcedure' from each table, we're delegating the Db logic to itself to better work with NLog in code.

### Step 2: Seed Data
Optionally, seed the database with initial data.

## Environment Setup
### Step 1: Configure Environment Variables
Set the following environment variables in your development environment:

