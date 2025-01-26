run:
	dotnet run --project src/Calango.API/Calango.API.csproj

build:
	dotnet build

migrations-add:
	cd src/Calango.API && dotnet ef migrations add AddNewMigrations --project ../Calango.Infrastructure/Calango.Infrastructure.csproj

migrations-update:
	cd src/Calango.API && dotnet ef database update --verbose --project ../Calango.Infrastructure/Calango.Infrastructure.csproj