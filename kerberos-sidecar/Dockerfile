FROM alpine:3.16

RUN apk -U upgrade
RUN apk add \
    --no-cache \
    --repository https://dl-cdn.alpinelinux.org/alpine/v3.16/main \
    bash krb5

RUN addgroup --system --gid 1000 apps
RUN adduser --system --uid 1000 --ingroup apps --shell /bin/false \ 
    --disabled-password --no-create-home svc_app

WORKDIR /
COPY krb5.conf /kerberos-sidecar/
COPY rekinit.sh .

RUN chmod +x rekinit.sh
RUN chown -R 1000:1000 /kerberos-sidecar

USER 1000

ENTRYPOINT [ "bash", "/rekinit.sh" ]
