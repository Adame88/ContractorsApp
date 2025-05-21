IF EXISTS (SELECT name FROM sys.databases WHERE name = N'ContractorsDB')
BEGIN
	USE master
    ALTER DATABASE ContractorsDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ContractorsDB;
END
GO

CREATE DATABASE ContractorsDB;
GO

USE ContractorsDB;
GO


CREATE TABLE Contractors (
    ContractorID INT PRIMARY KEY IDENTITY(1,1),
    CompanyName NVARCHAR(255) NOT NULL,
    TaxNumber NVARCHAR(20) UNIQUE,
    REGON NVARCHAR(14)
);


CREATE TABLE Addresses (
    AddressID INT PRIMARY KEY IDENTITY(1,1),
    ContractorID INT,
    Street NVARCHAR(255),
    City NVARCHAR(255),
    PostalCode NVARCHAR(10),
	IsMainAddress BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (ContractorID) REFERENCES Contractors(ContractorID)
);


CREATE UNIQUE INDEX UX_Addresses_MainAddress
ON Addresses(ContractorID)
WHERE IsMainAddress = 1;
GO

CREATE VIEW vw_ContractorsWithMainAddress AS
SELECT 
    c.ContractorID,
    c.CompanyName,
    c.TaxNumber,
    c.REGON,
    a.AddressID,
    a.Street,
    a.City,
    a.PostalCode,
	a.IsMainAddress
FROM Contractors c
LEFT JOIN Addresses a ON c.ContractorID = a.ContractorID;
GO


-- przykładowi kontrachenci
INSERT INTO Contractors (CompanyName, TaxNumber, REGON)
VALUES
('Kowalski i Synowie Sp. z o.o.', '123-456-32-18', '789654321'),
('Nowak Electronics', '987-654-21-65', '123456789'),
('Anderson Consulting', '543-210-98-76', '234567890'),
('Piekarnia Złoty Chleb', '321-654-87-90', '876543210'),
('TechWorld IT Services', '987-123-45-67', '123789456'),
('Bajka Agencja Reklamowa', '222-333-44-55', '876123456'),
('Łódzkie Meble', '222-777-44-88', '345678901');

-- Wstawianie adresów
INSERT INTO Addresses (Street, City, PostalCode, IsMainAddress, ContractorID)
VALUES
('ul. Warszawska 45', 'Warszawa', '00-123', 1, 1),
('ul. Krakowska 12', 'Kraków', '30-001', 0, 1),
('ul. Pięciomorgowa 8', 'Gdańsk', '80-200', 1, 2),
('ul. Malwowa 22', 'Sopot', '81-200', 0, 2),
('ul. Dębowa 10', 'Wrocław', '50-400', 1, 3),
('ul. Pieprzowa 9', 'Poznań', '61-111', 1, 4),
('ul. Kwiatowa 3', 'Gniezno', '62-200', 0, 4),
('ul. Słoneczna 27', 'Łódź', '90-001', 1, 5),
('ul. Polan 5', 'Zgierz', '95-200', 0, 5),
('ul. Strumykowa 13', 'Szczecin', '70-300', 1, 6),
('ul. Sosnowa 55', 'Łódź', '91-002', 1, 7);
GO


CREATE PROCEDURE sp_GetContractorsByNameOrTaxNumber 
    @CompanyName NVARCHAR(255) = NULL,
    @TaxNumber NVARCHAR(20) = NULL
AS
BEGIN
    SELECT * FROM vw_ContractorsWithMainAddress
    WHERE 
        (@CompanyName IS NULL OR CompanyName LIKE '%' + @CompanyName + '%') AND
        (@TaxNumber IS NULL OR TaxNumber LIKE '%' + @TaxNumber + '%')
ORDER BY CompanyName;
END;
GO

CREATE PROCEDURE sp_GetContractorById
    @ContractorID INT
AS
BEGIN
    SELECT * FROM vw_ContractorsWithMainAddress
    WHERE ContractorID = @ContractorID
END;
GO
------------------------------------------------

CREATE PROCEDURE sp_UpdateContractorDetails
    @ContractorID INT,
    @CompanyName NVARCHAR(255),
    @TaxNumber NVARCHAR(20) = NULL,
    @REGON NVARCHAR(14) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Contractors
    SET CompanyName = @CompanyName,
        TaxNumber = @TaxNumber,
        REGON = @REGON
    WHERE ContractorID = @ContractorID;
END;
GO

CREATE PROCEDURE sp_InsertContractor
    @CompanyName NVARCHAR(255),
    @TaxNumber NVARCHAR(10) = NULL,
    @REGON NVARCHAR(14) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Contractors (CompanyName, TaxNumber, REGON)
    OUTPUT INSERTED.ContractorID
    VALUES (@CompanyName, @TaxNumber, @REGON);
END;
GO

CREATE PROCEDURE sp_InsertAddress
    @ContractorID INT,
    @Street NVARCHAR(255),
    @City NVARCHAR(255),
    @PostalCode NVARCHAR(20),
    @IsMainAddress BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Addresses (ContractorID, Street, City, PostalCode, IsMainAddress)
    VALUES (@ContractorID, @Street, @City, @PostalCode, @IsMainAddress);
END;
GO

----------------------------------------------------------------
CREATE PROCEDURE sp_DeleteContractorById
    @ContractorID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Najpierw usuwamy adresy powiązane z kontrahentem
        DELETE FROM Addresses
        WHERE ContractorID = @ContractorID;

        -- Następnie usuwamy kontrahenta
        DELETE FROM Contractors
        WHERE ContractorID = @ContractorID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;

        -- Przerzucenie błędu dalej
        DECLARE @ErrorMessage NVARCHAR(4000);
        SET @ErrorMessage = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END;
GO

CREATE PROCEDURE sp_DeleteAddressesByContractorId
    @ContractorID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Addresses
    WHERE ContractorID = @ContractorID;
END;
GO

CREATE PROCEDURE sp_DeleteAddressById
    @AddressID INT
AS
BEGIN
    DELETE FROM Addresses
    WHERE AddressID = @AddressID;
END;
