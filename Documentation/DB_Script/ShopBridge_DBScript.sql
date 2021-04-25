-- Create KDLDatabase
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ShopBridge')
  BEGIN
	PRINT 'Creating ShopBridge Database';
    CREATE DATABASE [ShopBridge]
   END

-- Create Product Table
USE ShopBridge 
IF NOT EXISTS(SELECT 1 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' 
AND  TABLE_NAME = 'Product')
BEGIN
 PRINT 'Creating Product table';
CREATE TABLE Product
( 
ID INT IDENTITY(1,1) PRIMARY KEY,
Name VARCHAR(50),
Discription VARCHAR(200),
Price FLOAT,
)
END

