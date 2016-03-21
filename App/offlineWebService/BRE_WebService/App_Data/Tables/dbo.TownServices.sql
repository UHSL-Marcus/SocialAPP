CREATE TABLE [dbo].[Town_Services]
(
    [Name] VARCHAR(MAX) NOT NULL,
    [Town] TEXT NOT NULL, 
    [Category] TEXT NOT NULL, 
    [Rating] FLOAT NOT NULL, 
    [Latitude] FLOAT NOT NULL, 
    [Longitude] FLOAT NOT NULL, 
    [Perimeter] VARCHAR(MAX) NULL
)
