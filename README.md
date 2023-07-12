# :ocean: What is it?

## It is a natural water way and aquarium management application.
Currently hosted here: https://maelstrom-app.azurewebsites.net/ <br> <i>(This project does not receive any funding and may not always be active as a consequence)</i>

### But what if I am not into that kind of thing? Why is this relevant?
Most importantly Maelstrom demonstrates an architecture that can be used in a broad base of user end point applications.

:cyclone: Users can log in securely <br>
:cyclone: Users can only see the data that relates to them <br>
:cyclone: Users can upload media and data <br>
:cyclone: Users can share resources between themselves <br>
:cyclone: Users can assign different privilege levels to other users

# So many folders! What do?

## Main Razor pages
Navigate to Maelstrom => Areas => User/Pages 

Everything there is related to displaying content to users:
Routing, HTML generation, Working with models. 

## LINQ to EF Core
This one is easy! Just look in the AppUserService folder.
You will find two files:

1) the actual queries
2) the interface that is used to implement Dependency Inversion (DI)
   
   - quick note-
   You can see this used (DI) in the razor pages constructors (at the top of the files) listed above.
   This is made possible by configuring the middleware to look for it.
   In the Program.cs file:
   
          builder.Services.AddScoped<IAppUserService, AppUserService>();

   

## EF Core Models and Microsoft Identity Stuff
This one is a little more complicated but I can show a real example of how this works in a full application.
There is not much on this topic ANYWHERE; I do not know why because it is extremely powerful and one of the most useful things you can do in ASP.Net Core.

If you look in the EF_Models folder you will see a bunch of models that we use to work with the database design.
Some of these as of July/12/2023 have not been implemented so do not worry about the whole structure right now.
### Lets just look at AppUser.cs:

     public class AppUser : IdentityUser

That line right there is your <i>gate-way into Identity </i>

Basically, what is happening is that the AppUser properties listed in the file get combined with the hidden properties generated in the scaffolded code when Identity first ran. The database only sees The IdentityUser Table but ASP.Net Core knows that they are extended and will handle the object relations behind the scenes.

This black box stuff is what drives people crazy and they never get around to learning it, but believe me it’s worth it.
Once you know it, you can create secure user-specific logins in minutes.

### Our next stop is the MaelstromContext:

This file is the Database Context file, it is a necessary requirement for using EF Core as it helps EF Core Map to the database.

      public partial class MaelstromContext : IdentityDbContext<AppUser>

Here we extend our custom context into the default Identity Context that is created when you make a project in ASP.Net Core with default Identity settings.

###  Configure the middlewear in the Program.cs file
      builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<MaelstromContext>();

   

### Now it gets weird:

It may stem from my own misunderstanding of how EF Core works but sometimes when you have more complex Object Relational Maps conflicts can occur when using certain tools. To use the PMC and run new migrations you have to comment out:

    public MaelstromContext(DbContextOptions<MaelstromContext> options) : base(options) { }


1) In PMC run: Add-Migration
2) Double check the migration code that was generated in the Migrations folder because sometimes the scaffolder wants to create duplicate code
3) In PMC run: Update-Database



# :ocean: Can I run this app?
### Yes you can! 

## Step one: 
Make sure you have a Database! If you don't have one you can pick up one here: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
There is plenty of documentation on how to set this up out on the web.

## Step two:
Clone the repository! I recommend starting this project in VS and not VScode as it has more support for these kinds of projects out the box.

## Step three:
Use SQLServer Object explorer to find your Database, if it is not there you may have to create one. Look for the connection string in the properties tab and copy it for use later.

## Step four:
Create an appsettings.json file with the connection string. (Note: I will post more detailed images in a future version of this walk through).
Paste the connection string you just copied from the SqlServer Object Explorer properties tab.

## Step five:
Open up the Package Manager Console: run the Migration and then update the database. So this will be 2 separate commands:

      Add-Migration
      
      Update-Database

I highly recommend checking out the full EF Core PMC Docs, they are very readable and concise!
https://learn.microsoft.com/en-us/ef/core/cli/powershell

## Step six:
Press F5 and run the app! It should be working if not there may be some naming issues that the debugger should be able to display to you. 

This is the first draft of this set up guide so feel free to reach out if I missed something or you are having issues.

