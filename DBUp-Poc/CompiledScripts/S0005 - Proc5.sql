﻿IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'Proc5')
DROP PROCEDURE dbo.Proc5
GO

CREATE PROCEDURE dbo.Proc5
AS
BEGIN
	SELECT 1, 2, 3
END