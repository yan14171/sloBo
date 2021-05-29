#escape=`

FROM mcr.microsoft.com/dotnet/runtime:5.0

COPY Alexa_proj/bin/Release/net5.0-windows7.0/publish/ App/Alexa_proj

WORKDIR /App

ENV DOTNET_EnableDiagnostics=0

ENTRYPOINT ["cmd"]