gen-all: gen-server-selfsigned gen-ca gen-client gen-no-sn gen-wrong-sn

gen-server-selfsigned:
	openssl req -x509 -newkey rsa:4096 \
		-keyout ./nginx/server.key \
		-out ./nginx/server.crt \
		-sha256 -days 3650 -nodes \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Keytos Security/CN=localhost"
	cat ./nginx/server.key >> ./nginx/server.crt
	rm ./nginx/server.key

gen-ca:
	openssl req -x509 -newkey rsa:4096 \
		-keyout ./nginx/ca.key \
		-out ./nginx/ca.crt \
		-sha256 -days 3650 -nodes \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Keytos Security/CN=server-cert"
	cat ./nginx/ca.key >> ./nginx/ca.crt
	rm ./nginx/ca.key

client:
	mkdir -p client

gen-client: gen-ca client
	openssl req -newkey rsa:4096 -nodes -days 3650 \
		-keyout ./client/client.key -out ./client/client.crt \
		-CA ./nginx/ca.crt -CAkey ./nginx/ca.crt \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Client Certificate/SN=Test User"
	cat ./client/client.key >> ./client/client.crt
	rm ./client/client.key

gen-no-sn: gen-ca client
	openssl req -newkey rsa:4096 -nodes -days 3650 \
		-keyout ./client/no_sn.key -out ./client/no_sn.crt \
		-CA ./nginx/ca.crt -CAkey ./nginx/ca.crt \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Client Certificate"
	cat ./client/no_sn.key >> ./client/no_sn.crt
	rm ./client/no_sn.key

gen-wrong-sn: gen-ca client
	openssl req -newkey rsa:4096 -nodes -days 3650 \
		-keyout ./client/wrong_sn.key -out ./client/wrong_sn.crt \
		-CA ./nginx/ca.crt -CAkey ./nginx/ca.crt \
		-subj "/C=US/ST=Massachusetts/L=Boston/O=Keytos/OU=Client Certificate/SN=Fake User"
	cat ./client/wrong_sn.key >> ./client/wrong_sn.crt
	rm ./client/wrong_sn.key

run-test:
	curl --cert ./client/client.crt --key ./client/client.crt --cacert ./nginx/server.crt https://localhost -v

run-test-no-sn:
	curl --cert ./client/no_sn.crt --key ./client/no_sn.crt --cacert ./nginx/server.crt https://localhost -v

run-test-wrong-sn:
	curl --cert ./client/wrong_sn.crt --key ./client/wrong_sn.crt --cacert ./nginx/server.crt https://localhost -v
