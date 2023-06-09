
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[App_Menu_getPaging]
(
	 @Name nvarchar(250)
	,@ParentName nvarchar(250)
	,@pageIndex int
	,@pageSize int
	,@totalCount INT OUTPUT
)
as
begin
	
	select 
		a.*
		,b.Name ParentName
	into #data
	from App_Menu a
		left join App_Menu b on a.ParentId=b.Id

	where 
		(@Name is null or a.Name like N'%'+ @Name +'%')
		and (@ParentName is null or b.Name like N'%'+ @ParentName +'%')

	select * from #data

	order by Name
		offset @pageSize * @pageIndex rows
	FETCH NEXT @pageSize ROWS ONLY

	Set @totalCount = (select count(*) from #data)
	drop table #data
end
GO
