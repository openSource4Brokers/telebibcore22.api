USE [marDb22]
GO
/****** Object:  StoredProcedure [dbo].[spTb2Qualifiers_InsertFull]    Script Date: 25/08/2019 15:48:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTb2Valeurs_InsertFull]
    @Code nvarchar(4),
	@Valeur nvarchar(10),
    @Lbc1 nvarchar(20),
    @Lbc2 nvarchar(20),    
    @Lbl1 nvarchar(60),
    @Lbl2 nvarchar(60),
    @Lbl3 nvarchar(60),
    @Lbl4 nvarchar(60)    

AS
BEGIN
    SET NOCOUNT ON

    insert into TbValeurs
        (Code, Valeur, Lbc1, Lbc2, Lbl1, Lbl2, Lbl3, Lbl4)

    values
        ( @Code, @Valeur, @Lbc1, @Lbc2, @Lbl1, @Lbl2, @Lbl3, @Lbl4)

END
