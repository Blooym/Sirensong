FROM ghcr.io/blooym/docker-dalamud:stg-alpine

# Optout of telemtry.
ENV DOTNET_CLI_TELEMETRY_OPTOUT=1
ENV DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# Create a non-root dev user.
RUN adduser --disabled-password dev

# Install required dependencies.
RUN apk add --update openssh bash git