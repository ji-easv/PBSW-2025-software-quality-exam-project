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