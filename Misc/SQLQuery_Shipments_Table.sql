CREATE TABLE Shipments (
    Id NVARCHAR(50) PRIMARY KEY,
    ClientId INT NOT NULL,
    Origin NVARCHAR(255) NOT NULL,
    Destination NVARCHAR(255) NOT NULL,
    CreationDate DATETIME NOT NULL DEFAULT GETDATE(),
    Status NVARCHAR(50) NOT NULL CHECK (Status IN ('Processing', 'Shipped', 'Delivered', 'Canceled')),
    CONSTRAINT FK_Shipments_Clients FOREIGN KEY (ClientId) REFERENCES Clients(Id)
);