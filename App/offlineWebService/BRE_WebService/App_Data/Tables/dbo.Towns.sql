CREATE TABLE [dbo].[Towns] (
    [TownID]       INT           IDENTITY (1, 1) NOT NULL,
    [Town]         VARCHAR (MAX) NOT NULL,
    [County]       VARCHAR (MAX) NOT NULL,
    [Latitude]     FLOAT (53)    NOT NULL,
    [Longitude]    FLOAT (53)    NOT NULL,
    [HasPerimeter] BIT           NOT NULL,
    [Perimeter]    VARCHAR (MAX) DEFAULT ((0)) NULL
);

