﻿IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'Proc2')
DROP PROCEDURE dbo.Proc2
GO

CREATE PROCEDURE dbo.Proc2
AS
BEGIN
	SELECT 1, 2, 3
END