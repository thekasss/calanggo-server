run:
	dotnet run --project src/Calanggo.API/Calanggo.API.csproj

build:
	dotnet build

migrations-add:
	cd src/Calanggo.API && dotnet ef migrations add AddNewMigrations --project ../Calanggo.Infrastructure/Calanggo.Infrastructure.csproj

migrations-update:
	cd src/Calanggo.API && dotnet ef database update --verbose --project ../Calanggo.Infrastructure/Calanggo.Infrastructure.csproj