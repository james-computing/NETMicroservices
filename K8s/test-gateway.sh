# GET
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/Platforms
curl --resolve acme.com:80:127.0.0.1 http://acme.com/api/CommandsForPlatforms

# POST
curl --resolve acme.com:80:127.0.0.1 --json '{"Name": "name", "Publisher": "publisher", "Cost": "cost"}' http://acme.com/api/Platforms