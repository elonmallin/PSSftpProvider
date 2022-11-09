FROM alpine

# Install pwsh
RUN apk add --no-cache \
    ca-certificates \
    less \
    ncurses-terminfo-base \
    krb5-libs \
    libgcc \
    libintl \
    libssl1.1 \
    libstdc++ \
    tzdata \
    userspace-rcu \
    zlib \
    icu-libs \
    curl \
    bash

RUN apk -X https://dl-cdn.alpinelinux.org/alpine/edge/main add --no-cache \
    lttng-ust

ADD https://github.com/PowerShell/PowerShell/releases/download/v7.2.7/powershell-7.2.7-linux-alpine-x64.tar.gz /tmp/powershell.tar.gz

RUN mkdir -p /opt/microsoft/powershell/7 \
    && tar zxf /tmp/powershell.tar.gz -C /opt/microsoft/powershell/7 \
    && chmod +x /opt/microsoft/powershell/7/pwsh \
    && ln -s /opt/microsoft/powershell/7/pwsh /usr/bin/pwsh

RUN pwsh -Command "Install-Module Pester -Force"

# Install dotnet
ADD https://dot.net/v1/dotnet-install.sh /tmp/dotnet-install.sh

RUN chmod +x /tmp/dotnet-install.sh \
    && /tmp/dotnet-install.sh -c 6.0 \
    && ln -s /root/.dotnet/dotnet /usr/bin/dotnet

# Install openssh/sftp
# RUN echo "@community http://dl-cdn.alpinelinux.org/alpine/edge/community" >> /etc/apk/repositories
# RUN apk add --no-cache \
#     openssh \
#     openssl \
#     shadow@community

# RUN ssh-keygen -A

# RUN useradd -ms /bin/bash -p "$(openssl passwd -1 pass)" foo \
#     && chown root:root /home/foo \
#     && chmod 755 /home/foo

# ADD . .

# RUN chmod +x /test/entrypoint.sh \
#     && chmod +x /test/atmoz-entrypoint.sh

# COPY test/sshd-config /etc/ssh/sshd_config

WORKDIR /app

SHELL ["pwsh", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ENTRYPOINT /app/test/docker-compose/entrypoint.ps1
