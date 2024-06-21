gen-selfsigned:
	openssl req -x509 -newkey rsa:4096 \
		-keyout key.pem \
		-out cert.pem \
		-sha256 -days 3650 -nodes \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Keytos Security/CN=server-cert"
gen-client:
	openssl req -newkey rsa:4096 -nodes -days 3650 \
		-keyout client.key -out client.crt \
		-CA cert.pem -CAkey key.pem \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Client Certificate"
