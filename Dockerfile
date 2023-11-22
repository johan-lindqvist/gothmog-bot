FROM mcr.microsoft.com/dotnet/core/runtime:7.0 AS base

WORKDIR /bot
COPY ./output ./
ENTRYPOINT ["./GothmogBot"]
