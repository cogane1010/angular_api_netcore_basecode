
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[App_User_getUserMenu]
(
	@UserName nvarchar(250)
)
as
begin
	select menu.Id, menu.ParentId, menu.Level, menu.Name, menu.Icon, menu.Url
	from App_Menu menu
	inner join App_Role_Menu_Ref rolemenu on menu.Id= rolemenu.MenuId
	where exists (select 1 from AspNetUserRoles userrole
							join AspNetUsers appuser on appuser.Id=userrole.UserId
							where appuser.UserName=@UserName
								and userrole.RoleId=rolemenu.AspRoleId)
	order by menu.DisplayOrder
	
end
GO
