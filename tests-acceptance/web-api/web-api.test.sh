#!/bin/bash

echo "Hexapokernet Web API tests"

BASE_URL=http://${APP_HOST}:${APP_PORT}
echo "Tesing ${BASE_URL}"

echo "Wait 3 seconds to start all services."
echo "TODO: replace it with health check"
sleep 2

echo;echo
echo "=== POST /story ==="
curl -i -v -X POST ${BASE_URL}/story \
  -H "Content-Type: application/json" \
  -d '{ "title": "Test story" }'

echo;echo
echo "=== GET /story/:storyId ==="
curl -i -v -X GET ${BASE_URL}/story/123

echo "Done!"