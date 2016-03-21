CREATE TABLE [dbo].[Sub_Category] (
    [SubCategoryID] INT           IDENTITY (1, 1) NOT NULL,
    [SubCategory]   VARCHAR (MAX) NULL,
    [CategoryID]    INT           NOT NULL
);

