
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /SalesOrder

COPY . ./


RUN dotnet restore


RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /SalesOrder

COPY --from=build-env /SalesOrder/out .

ENTRYPOINT ["dotnet", "SalesOrder.dll"]
