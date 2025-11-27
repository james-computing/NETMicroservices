# Tests ordered according to functionality, not the order they should be run.
# To just run a test script, use test-api-ordered.sh

# Platforms service
# GET
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Platforms
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Platforms/1
#POST
curl --resolve acme.com:80:127.0.0.1 --json '{"Name": "name", "Publisher": "publisher", "Cost": "cost"}' http://acme.com/api/Platforms

# Commands service
# Note that the platformId is the Id, not the ExternalId, in the Platform model of the Commands service
# GET
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/CommandsPlatforms
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Commands/platformId/1
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Commands/platformId/1/commandId/1
# POST
curl --resolve acme.com:80:127.0.0.1 -X POST http://acme.com/api/CommandsPlatforms
curl --resolve acme.com:80:127.0.0.1 --json '{"HowTo": "Push a docker container to Hub", "CommandLine": "docker push <name of container>"}' http://acme.com/api/Commands/platformId/1