.PHONY: add-migration remove-migration

MIGRATION_NAME ?= DefaultMigration
PROJECT = Infrastructure/Infrastructure.csproj
STARTUP = BoxFactoryAPI/BoxFactoryAPI.csproj

add-migration:
	@echo "Adding migration: $(MIGRATION_NAME)..."
	dotnet ef migrations add $(MIGRATION_NAME) --project $(PROJECT) --startup-project $(STARTUP)

remove-migration:
	@echo "Removing last migration..."
	dotnet ef migrations remove --project $(PROJECT) --startup-project $(STARTUP)

run-migrations:
	@echo "Applying migrations to the database..."
	dotnet ef database update --project $(PROJECT) --startup-project $(STARTUP)

test-backend:
	@echo "Running tests..."
	dotnet build --no-incremental

	dotnet coverlet ./Core.UnitTests/bin/Debug/net10.0/Core.UnitTests.dll \
              --target "dotnet" \
              --targetargs "test ./Core.UnitTests/Core.UnitTests.csproj --no-build --logger:trx" \
              -f=opencover \
              -o="coverage.core.xml"
              
	 dotnet coverlet ./BoxFactory.BDDTests/bin/Debug/net10.0/BoxFactory.BDDTests.dll \
			   --target "dotnet" \
			   --targetargs "test ./BoxFactory.BDDTests/BoxFactory.BDDTests.csproj --no-build --logger:trx" \
			   -f=opencover \
			   -o="coverage.bdd.xml"
			   
	 dotnet coverlet ./BoxFactory.IntegrationTests/bin/Debug/net10.0/BoxFactory.IntegrationTests.dll \
               --target "dotnet" \
               --targetargs "test ./BoxFactory.IntegrationTests/BoxFactory.IntegrationTests.csproj --no-build --logger:trx" \
               -f=opencover \
               -o="coverage.integration.xml"