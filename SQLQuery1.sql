select
[user].[DisplayName] ,
[user].[Id],
[roles].[Id],
[roles].[Name]
FRom
	[AspNetUsers] as [user]
INNER JOIN 
	[AspNetUserRoles] AS [userXroles]
		ON [user].[Id] = [userXroles].[UserId]

INNER JOIN
	[AspNetRoles] AS [roles] 
		ON [userXroles].[RoleId] = [roles].[Id]