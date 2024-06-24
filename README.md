# Mutual TLS over HTTPS with Nginx and ASP.NET Server

This is a basic setup of an ASP.NET server that carries out and
enforces mutual TLS over HTTPS.

In order to carry out client certificate authentication in ASP.NET,
a proxy that carries out the mutual TLS authentication and session needs
to be put in front of it. In this simple setup, it is an Nginx container.

Additional authorization was also added with a policy to check for a
specific subject name (`SN`) in the certificate.

## Running the tests

Certificate chain is already setup, and can be rebuilt by running `make`.

In one terminal, stand up the containers with the following command.

```bash
$ docker compose up
```

And in another terminal, you should be able to run the following tests

```bash
# Correct test, should result in a 200 (OK) with "Hello world\n".
$ make run-test

# Incorrect subject name, should result in a 403 and no reply.
$ make run-test-wrong-sn

# No subject name, should result ina 403 and no reply.
$ make run-test-no-sn
```
