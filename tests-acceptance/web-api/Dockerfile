FROM ubuntu:22.04

RUN apt-get update && apt-get install -y curl

WORKDIR /test
COPY . .

CMD ["/test/web-api.test.sh"]