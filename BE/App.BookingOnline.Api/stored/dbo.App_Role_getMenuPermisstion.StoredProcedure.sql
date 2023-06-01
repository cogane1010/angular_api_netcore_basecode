
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[App_Role_getMenuPermisstion]
(
	@Id nvarchar(250)
)
as
begin
	select 
		* 
		,(select 1 where exists (select 1 from App_Role_Menu_Ref i where i.MenuId=a.Id and i.AspRoleId=@Id)) HasMenu
	from App_Menu a
	order by a.DisplayOrder
end

GO
