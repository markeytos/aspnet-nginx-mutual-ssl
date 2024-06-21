gen-all: gen-server-selfsigned gen-ca gen-client

gen-server-selfsigned:
	openssl req -x509 -newkey rsa:4096 \
		-keyout server.key \
		-out server.crt \
		-sha256 -days 3650 -nodes \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Keytos Security/CN=localhost"
	cat server.key >> server.crt
	rm server.key

gen-ca:
	openssl req -x509 -newkey rsa:4096 \
		-keyout ca.key \
		-out ca.crt \
		-sha256 -days 3650 -nodes \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Keytos Security/CN=server-cert"
	cat ca.key >> ca.crt
	rm ca.key

gen-client: gen-ca
	openssl req -newkey rsa:4096 -nodes -days 3650 \
		-keyout client.key -out client.crt \
		-CA ca.crt -CAkey ca.crt \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Client Certificate"
	cat client.key >> client.crt
	rm client.key

run-test:
	curl --cert ./client.crt --key ./client.crt --cacert ./server.crt https://localhost -v
