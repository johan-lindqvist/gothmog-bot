FROM mcr.microsoft.com/dotnet/runtime-deps

WORKDIR /bot
COPY ./output ./
ENTRYPOINT ["./GothmogBot"]
