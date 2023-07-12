# So many folders! What do?

##To check out the main Razor pages.
Navigate to Maelstrom => Areas => User/Pages 

Everything there is directly related to displaying content to users:
Routing, HTML generation, Working with the models. 

## LINQ to EF core.
This one is easy! Just look in the AppUserService folder.
You will find two files:

1) the actual queries
2) the interface that is used to implement Dependency Inversion (DI)
   
     - quick note-
       You can see this used (DI) in the razor pages constructors (at the top of the files) listed above.
       This is made possible by configuring the middleware to look for it in the Maelstrom.csproj file.
       
       

# Can I run this app?
Yes you can! 

## Step one: 
Make sure you have a Database! If you don't have one you can pick up one here: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
There is plenty of documentation on how to set this up out on the web.

## Step two:
Clone the repository! I recommend starting this project in VS and not VScode as it has more support for these kinds of projects out the box.

## step three:
Use SQLServer Object explorer to find your Database, if it is not there you may have to create one. Look for the connection string in the properties tab and copy it for use later.

## step four:
Create an appsettings.Json file with the connection string. (Note: I will post more detailed images in a future version of this walk through).
Paste the connection string you just copied from the SqlServer Object Explorer properties tab.

## step five:
Open up the Package Manager Console: run the Migration and then update the database. So this will be 2 separate commands:

Add-Migration
Update-Database

I highly recommend checking out the full EF Core PMC Docs, those guys made them very readable and concise!
https://learn.microsoft.com/en-us/ef/core/cli/powershell

## step five:
Press F5 and run the app! It should be working if not there may be some naming issues that the debugger should be able to display to you. 

This is the first draft of this set up guide so feel free to reach out if I missed something or you are having issues.

