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
