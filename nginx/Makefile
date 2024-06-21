gen-all: gen-server-selfsigned gen-ca gen-client gen-no-sn gen-wrong-sn

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
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Client Certificate/SN=Test User"
	cat client.key >> client.crt
	rm client.key

gen-no-sn: gen-ca
	openssl req -newkey rsa:4096 -nodes -days 3650 \
		-keyout no_sn.key -out no_sn.crt \
		-CA ca.crt -CAkey ca.crt \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Client Certificate"
	cat no_sn.key >> no_sn.crt
	rm no_sn.key

gen-wrong-sn: gen-ca
	openssl req -newkey rsa:4096 -nodes -days 3650 \
		-keyout wrong_sn.key -out wrong_sn.crt \
		-CA ca.crt -CAkey ca.crt \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Client Certificate/SN=Fake User"
	cat wrong_sn.key >> wrong_sn.crt
	rm wrong_sn.key

run-test:
	curl --cert ./client.crt --key ./client.crt --cacert ./server.crt https://localhost -v

run-test-no-sn:
	curl --cert ./no_sn.crt --key ./no_sn.crt --cacert ./server.crt https://localhost -v

run-test-wrong-sn:
	curl --cert ./wrong_sn.crt --key ./wrong_sn.crt --cacert ./server.crt https://localhost -v
