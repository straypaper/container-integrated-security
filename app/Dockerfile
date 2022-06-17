FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

ENV DB="Data Source=sp-sql-001.straypaper.com,1433;Initial Catalog=master;Integrated Security=True;Connect Timeout=30"
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_PREDEFINED_CULTURES_ONLY=true

RUN apk -U upgrade
RUN apk add \
    --no-cache \
    --repository https://dl-cdn.alpinelinux.org/alpine/v3.16/main \
    bash krb5 icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib

RUN addgroup --system --gid 1000 apps
RUN adduser --system --uid 1000 --ingroup apps --shell /bin/false \ 
    --disabled-password --no-create-home svc_app

USER 1000

COPY src/container-integrated-security/bin/Debug/net6.0/ /app

WORKDIR /app

ENTRYPOINT [ "dotnet", "./container-integrated-security.dll" ]
