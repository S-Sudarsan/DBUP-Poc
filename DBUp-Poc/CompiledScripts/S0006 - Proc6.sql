﻿IF EXISTS(SELECT 1 FROM SYS.OBJECTS WHERE TYPE = 'P' AND NAME = 'Proc6')
DROP PROCEDURE dbo.Proc6
GO

CREATE PROCEDURE dbo.Proc6
AS
BEGIN
	SELECT 1, 2, 3
END