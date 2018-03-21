


#Ef add migration example
dotnet ef migrations add Initial -c DDD.Customer.Data.Context.CustomerDbContext -p ../../Modules/Customer/DDD.Customer.Data -o ./Context/Migrations

dotnet ef migrations add Initial -c DDD.Users.Data.Context.UsersDbContext -p ../../Modules/Users/DDD.Users.Data -o ./Context/Migrations



