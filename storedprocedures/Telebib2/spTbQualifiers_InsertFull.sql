USE [marDb22]
GO
/****** Object:  StoredProcedure [dbo].[spTb2Qualifiers_InsertFull]    Script Date: 25/08/2019 15:48:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTb2Qualifiers_InsertFull]
    @DE nvarchar(4),
	@Qualifier nvarchar(10),
    @Version float,    
    @Lbc1 nvarchar(60),
    @Lbc2 nvarchar(60),
    @Lbc3 nvarchar(60),
    @Lbc4 nvarchar(60)    

AS
BEGIN
    SET NOCOUNT ON

    insert into TbQualifiers
        (DE, Qualifier, Version, Lbc1, Lbc2, Lbc3, Lbc4)

    values
        ( @DE, @Qualifier, @Version, @Lbc1, @Lbc2, @Lbc3, @Lbc4)

END
