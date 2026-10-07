 CREATE DATABASE UsedCarSalesDb;
GO
USE UsedCarSalesDb;
GO

--customer tbl
CREATE TABLE dbo.Customers (
    CustomerId  INT IDENTITY(1,1) NOT NULL,
    FullName    NVARCHAR(150) NOT NULL,
    Email       NVARCHAR(150) NOT NULL,
    Phone       NVARCHAR(20)  NOT NULL,
    Address     NVARCHAR(300) NULL,
    CreatedAt   DATETIME2     NOT NULL CONSTRAINT DF_Customers_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_Customers PRIMARY KEY (CustomerId),
    CONSTRAINT UQ_Customers_Email UNIQUE (Email)
);

--cars tbl
CREATE TABLE dbo.Cars (
    CarId            INT IDENTITY(1,1) NOT NULL,
    Vin              NVARCHAR(17)  NOT NULL,
    Make             NVARCHAR(50)  NOT NULL,
    Model            NVARCHAR(50)  NOT NULL,
    ManufactureYear  SMALLINT      NOT NULL,
    Mileage          INT           NOT NULL,
    Color            NVARCHAR(30)  NULL,
    FuelType         NVARCHAR(20)  NOT NULL,
    Transmission     NVARCHAR(20)  NOT NULL,
    Price            DECIMAL(18,2) NOT NULL,
    Status           NVARCHAR(20)  NOT NULL CONSTRAINT DF_Cars_Status DEFAULT 'Available',
    Description      NVARCHAR(2000) NULL,
    CreatedAt        DATETIME2     NOT NULL CONSTRAINT DF_Cars_CreatedAt DEFAULT SYSUTCDATETIME(),
    UpdatedAt        DATETIME2     NULL,
    RowVersion       ROWVERSION    NOT NULL,
    CONSTRAINT PK_Cars PRIMARY KEY (CarId),
    CONSTRAINT UQ_Cars_Vin UNIQUE (Vin),
    CONSTRAINT CK_Cars_Year         CHECK (ManufactureYear BETWEEN 1980 AND 2100),
    CONSTRAINT CK_Cars_Mileage      CHECK (Mileage >= 0),
    CONSTRAINT CK_Cars_Price        CHECK (Price > 0),
    CONSTRAINT CK_Cars_Status       CHECK (Status IN ('Available','Reserved','Sold')),
    CONSTRAINT CK_Cars_FuelType     CHECK (FuelType IN ('Petrol','Diesel','CNG','Electric','Hybrid')),
    CONSTRAINT CK_Cars_Transmission CHECK (Transmission IN ('Manual','Automatic'))
);
CREATE INDEX IX_Cars_Status_Make ON dbo.Cars (Status, Make);

--car images 
CREATE TABLE dbo.CarImages (
    ImageId    INT IDENTITY(1,1) NOT NULL,
    CarId      INT           NOT NULL,
    FilePath   NVARCHAR(500) NOT NULL,
    IsPrimary  BIT           NOT NULL CONSTRAINT DF_CarImages_IsPrimary DEFAULT 0,
    CreatedAt  DATETIME2     NOT NULL CONSTRAINT DF_CarImages_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_CarImages PRIMARY KEY (ImageId),
    CONSTRAINT FK_CarImages_Cars FOREIGN KEY (CarId) REFERENCES dbo.Cars (CarId) ON DELETE CASCADE
);
CREATE INDEX IX_CarImages_CarId ON dbo.CarImages (CarId);

CREATE UNIQUE INDEX UX_CarImages_OnePrimary ON dbo.CarImages (CarId) WHERE IsPrimary = 1;

-- billls tbl
CREATE TABLE dbo.Bills (
    BillId         INT IDENTITY(1,1) NOT NULL,
    InvoiceNumber  NVARCHAR(30)  NOT NULL,
    CarId          INT           NOT NULL,
    CustomerId     INT           NOT NULL,
    SalePrice      DECIMAL(18,2) NOT NULL,
    TaxPercent     DECIMAL(5,2)  NOT NULL,
    TaxAmount      DECIMAL(18,2) NOT NULL,
    TotalAmount    DECIMAL(18,2) NOT NULL,
    PaymentMethod  NVARCHAR(20)  NOT NULL,
    BillDate       DATETIME2     NOT NULL CONSTRAINT DF_Bills_BillDate DEFAULT SYSUTCDATETIME(),
    CONSTRAINT PK_Bills PRIMARY KEY (BillId),
    CONSTRAINT UQ_Bills_InvoiceNumber UNIQUE (InvoiceNumber),
    CONSTRAINT UQ_Bills_CarId UNIQUE (CarId),   -- a car can be sold only once
    CONSTRAINT FK_Bills_Cars      FOREIGN KEY (CarId)      REFERENCES dbo.Cars (CarId),
    CONSTRAINT FK_Bills_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers (CustomerId),
    CONSTRAINT CK_Bills_Amounts   CHECK (SalePrice > 0 AND TaxAmount >= 0 AND TotalAmount >= SalePrice),
    CONSTRAINT CK_Bills_Payment   CHECK (PaymentMethod IN ('Cash','Card','BankTransfer','Finance'))
);
CREATE INDEX IX_Bills_CustomerId ON dbo.Bills (CustomerId);
GO