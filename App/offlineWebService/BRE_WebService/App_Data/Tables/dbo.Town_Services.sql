CREATE TABLE [dbo].[Town_Services] (
    [ServiceID]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]               VARCHAR (MAX) NOT NULL,
    [TownID]             INT           NOT NULL,
    [CategoryID]         INT           NOT NULL,
    [SubCategoryID]      INT           NOT NULL,
    [Rating]             FLOAT (53)    NOT NULL,
    [Latitude]           FLOAT (53)    NOT NULL,
    [Longitude]          FLOAT (53)    NOT NULL,
    [HasPerimeter]       BIT           NOT NULL,
    [Perimeter]          VARCHAR (MAX) NULL,
    [HasVirtualServices] BIT           NOT NULL,
    [VirtualServices]    VARCHAR (MAX) NULL
);

