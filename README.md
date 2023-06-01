# angular_api_netcore_basecode

This project base on API .net core 
Using design parten: Repository, Unit of work

Front end build from angular 14 using MVVM parten

Cript update DB
EntityFrameworkCore\add-migration  20230424 -context BookingOnlineDbContext -StartupProject IdentityServer

EntityFrameworkCore\Update-Database -context BookingOnlineDbContext -StartupProject IdentityServer
EntityFrameworkCore\Update-Database -context PersistedGrantDbContext -StartupProject IdentityServer
EntityFrameworkCore\Update-Database -context ConfigurationDbContext -StartupProject IdentityServer

EntityFrameworkCore\Script-Migration -From 20230322022212_20230322 -o "App.BookingOnline.Data/Migrations/script/20230424.sql" -context BookingOnlineDbContext -StartupProject IdentityServer

import database from large file script
sqlcmd -S DEV-DB\MSSQL2019 -d BO_GOLF1 -i E:\bo_golf\script.sql 
