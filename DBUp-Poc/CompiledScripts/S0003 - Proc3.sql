﻿IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'Proc3')
DROP PROCEDURE dbo.Proc3
GO

CREATE PROCEDURE dbo.Proc3
AS
BEGIN
	SELECT 1, 2, 3
END