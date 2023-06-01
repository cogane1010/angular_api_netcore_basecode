
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[App_User_getRolesPermisstion]
(
	@Id nvarchar(250)
)
as
begin
	select 
		* 
		,(select 1 where exists (select 1 from AspNetUserRoles i where i.UserId=@Id and i.RoleId=a.Id)) HasRole
	from AspNetRoles a order by CreatedDate
end
GO
