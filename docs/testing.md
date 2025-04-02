# Integration Test
```yml
# Construct database migration
docker compose -f docker-compose.yml -f development.yml build
docker compose -f docker-compose.yml -f development.yml up -d

# Run tests
dotnet test
```