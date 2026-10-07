USE UsedCarSalesDb;
GO

INSERT INTO dbo.Customers (FullName, Email, Phone, Address)
SELECT v.FullName, v.Email, v.Phone, v.Address
FROM (VALUES
    (N'Rahul Patil',  N'rahul.patil@example.com', N'+91 98765 43210', N'Shahupuri, Kolhapur'),
    (N'Sneha Kulkarni', N'sneha.kulkarni@example.com', N'+91 98220 11223', N'Kothrud, Pune'),
    (N'Amit Deshmukh', N'amit.deshmukh@example.com', N'+91 90110 55667', N'Gangapur Road, Nashik')
) AS v (FullName, Email, Phone, Address)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Customers c WHERE c.Email = v.Email);

INSERT INTO dbo.Cars (Vin, Make, Model, ManufactureYear, Mileage, Color, FuelType, Transmission, Price, Description)
SELECT v.Vin, v.Make, v.Model, v.Yr, v.Mileage, v.Color, v.Fuel, v.Trans, v.Price, v.Descr
FROM (VALUES
    (N'MA3EJKD1S00123456', N'Maruti Suzuki', N'Swift VXI',    2019, 42000, N'White',  N'Petrol', N'Manual',    485000, N'Single owner, full service history'),
    (N'MALBM51BLKM123789', N'Hyundai',       N'Creta SX',     2020, 35000, N'Grey',   N'Diesel', N'Automatic', 1250000, N'Sunroof, top variant'),
    (N'MA1TA2XYZ00456123', N'Tata',          N'Nexon XZ+',    2021, 22000, N'Blue',   N'Petrol', N'Manual',    980000, N'Well maintained, new tyres'),
    (N'MBHCZC82SKG789456', N'Honda',         N'City ZX',      2018, 58000, N'Silver', N'Petrol', N'Automatic', 760000, N'Second owner, accident free')
) AS v (Vin, Make, Model, Yr, Mileage, Color, Fuel, Trans, Price, Descr)
WHERE NOT EXISTS (SELECT 1 FROM dbo.Cars c WHERE c.Vin = v.Vin);
GO