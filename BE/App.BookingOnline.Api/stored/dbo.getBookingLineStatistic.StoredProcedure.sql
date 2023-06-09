
INSERT INTO App_Menu ([Name] ,[IsActive],[ParentId],[Url],[Icon],[DisplayOrder],[CreatedUser] ,[CreatedDate] ,[Level])
     VALUES (N'Quản Trị', 1 , (select Top(1) Id from App_Menu where Name='ROOT'), NULL, 'list', 0,  'admin', GetDate(), 0);

INSERT INTO App_Menu ([Name] ,[IsActive],[ParentId],[Url],[Icon],[DisplayOrder],[CreatedUser] ,[CreatedDate] ,[Level])
     VALUES ('Menu', 1 , (select Top(1) Id from App_Menu where Name=N'Quản Trị'), '/admin/menu', 'list', 0,  'admin', GetDate(), 0);

INSERT INTO App_Menu ([Name] ,[IsActive],[ParentId],[Url],[Icon],[DisplayOrder],[CreatedUser] ,[CreatedDate] ,[Level])
     VALUES ('Role', 1 , (select Top(1) Id from App_Menu where Name=N'Quản Trị'), '/admin/role', 'list', 0,  'admin', GetDate(), 0);

INSERT INTO App_Menu ([Name] ,[IsActive],[ParentId],[Url],[Icon],[DisplayOrder],[CreatedUser] ,[CreatedDate] ,[Level])
     VALUES ('User', 1 , (select Top(1) Id from App_Menu where Name=N'Quản Trị'), '/admin/user', 'list', 0,  'admin', GetDate(), 0);
	 
INSERT INTO [App_Role_Menu_Ref] (AspRoleId,[MenuId],[CreatedUser],[CreatedDate] )
     VALUES( (select Top(1) Id from AspNetRoles where Name='Admin'), (select Top(1) Id from App_Menu where Name='ROOT') , 'admin',GetDate())

	 
INSERT INTO [App_Role_Menu_Ref] (AspRoleId,[MenuId],[CreatedUser],[CreatedDate] )
     VALUES( (select Top(1) Id from AspNetRoles where Name='Admin'), (select Top(1) Id from App_Menu where Name=N'Quản Trị') , 'admin',GetDate())


INSERT INTO [App_Role_Menu_Ref] (AspRoleId,[MenuId],[CreatedUser],[CreatedDate] )
     VALUES( (select Top(1) Id from AspNetRoles where Name='Admin'), (select Top(1) Id from App_Menu where Name='Menu') , 'admin',GetDate())


INSERT INTO [App_Role_Menu_Ref] (AspRoleId,[MenuId],[CreatedUser],[CreatedDate] )
     VALUES( (select Top(1) Id from AspNetRoles where Name='Admin'), (select Top(1) Id from App_Menu where Name='Role') , 'admin',GetDate())


INSERT INTO [App_Role_Menu_Ref] (AspRoleId,[MenuId],[CreatedUser],[CreatedDate] )
     VALUES( (select Top(1) Id from AspNetRoles where Name='Admin'), (select Top(1) Id from App_Menu where Name='User') , 'admin',GetDate())

INSERT INTO [AspNetUserRoles] ([UserId],[RoleId] )
     VALUES( (select Top(1) Id from AspNetUsers where UserName='admin'), (select Top(1) Id from AspNetRoles where Name='Admin'))