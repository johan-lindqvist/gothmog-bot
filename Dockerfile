FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base

WORKDIR /bot
COPY ./output ./
ENTRYPOINT ["./GothmogBot"]
