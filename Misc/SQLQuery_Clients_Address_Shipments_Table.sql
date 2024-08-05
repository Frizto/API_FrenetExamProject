CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255),
    Phone VARCHAR(20),
    Email VARCHAR(255),
    AspNetUserId VARCHAR(50),
);

CREATE TABLE Addresses (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Street VARCHAR(255),
    City VARCHAR(100),
    State VARCHAR(100),
    ZipCode VARCHAR(20),
    ClientID INT,
    CONSTRAINT FK_Addresses_Clients FOREIGN KEY (Id) REFERENCES Clients(Id)
);
CREATE TABLE Shipments (
    Id NVARCHAR(50) PRIMARY KEY,
    ClientId INT NOT NULL,
    Origin NVARCHAR(255) NOT NULL,
    Destination NVARCHAR(255) NOT NULL,
    CreationDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(50) NOT NULL CHECK (Status IN ('Processing', 'Shipped', 'Delivered', 'Canceled')),
    CONSTRAINT FK_Shipments_Clients FOREIGN KEY (ClientId) REFERENCES Clients(Id)