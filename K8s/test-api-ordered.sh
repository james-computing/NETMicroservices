# Run this script to test the api

# Get all platforms from the Platforms service
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Platforms

# Get the platforms stored in the Commands service. It should give the same platforms, but with new ids specific to the Commands service.
# The origianal ids won't be obtained by this request, but it is stored as an ExternalId in the Commands service database.
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/CommandsPlatforms

# Get only the platform with Id=1 
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Platforms/1

# Create a new platform for the Platforms service. The new platform will also be sent by the Platforms service to the Commands service.
curl --resolve acme.com:80:127.0.0.1 --json '{"Name": "name", "Publisher": "publisher", "Cost": "cost"}' http://acme.com/api/Platforms

# Check that the new platform was created in both services
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Platforms
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/CommandsPlatforms

# Create 2 commands for the platform with Id=1. Notice that this Id is local to the Commands service. The original Id is stored as
# an ExternalId in the Commands service database.
curl --resolve acme.com:80:127.0.0.1 --json '{"HowTo": "Push a docker container to Hub", "CommandLine": "docker push <name of container>"}' http://acme.com/api/Commands/platformId/1
curl --resolve acme.com:80:127.0.0.1 --json '{"HowTo": "how", "CommandLine": "command"}' http://acme.com/api/Commands/platformId/1

# Check that the commands were created
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Commands/platformId/1

# Get the command created with Id=1
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Commands/platformId/1/commandId/1