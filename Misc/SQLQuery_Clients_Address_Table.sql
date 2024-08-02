CREATE TABLE Address (
    AddressID INT PRIMARY KEY IDENTITY(1,1),
    Street VARCHAR(255),
    City VARCHAR(100),
    State VARCHAR(100),
    ZipCode VARCHAR(20)
);

CREATE TABLE Clients (
    ClientID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255),
    Phone VARCHAR(20),
    Email VARCHAR(255),
    AddressID INT,
    FOREIGN KEY (AddressID) REFERENCES Address(AddressID)
);