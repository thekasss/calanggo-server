run:
	dotnet run --project src/Pingu.API/Pingu.API.csproj

build:
	dotnet build src/Pingu.API/Pingu.API.csproj

migrations-add:
	cd src/Pingu.API && dotnet ef migrations add AddNewMigrations --project ../Pingu.Infrastructure/Pingu.Infrastructure.csproj

migrations-update:
	cd src/Pingu.API && dotnet ef database update --verbose --project ../Pingu.Infrastructure/Pingu.Infrastructure.csproj