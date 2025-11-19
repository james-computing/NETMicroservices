#Should succeed
curl --resolve cafe.example.com:80:127.0.0.1 http://cafe.example.com:80/
curl --resolve cafe.example.com:80:127.0.0.1 http://cafe.example.com:80/some-path
curl --resolve cafe.example.com:80:127.0.0.1 http://cafe.example.com:80/some/path
#Should fail
curl --resolve pub.example.com:80:127.0.0.1 http://pub.example.com:80/