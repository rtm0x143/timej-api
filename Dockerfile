ARG PROJECT_NAME=TimejApi
ARG BUILD_TYPE=Debug
ARG ENVIRONMENT=Development


###### BASE IMAGE #####
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.17-amd64 AS base
WORKDIR /app
EXPOSE 80


###### BUILD IMAGE #####
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.16-amd64 AS build
ARG PROJECT_NAME
ARG BUILD_TYPE

WORKDIR /src
COPY ${PROJECT_NAME}.csproj .

RUN dotnet restore ${PROJECT_NAME}.csproj
COPY . .
RUN dotnet build ${PROJECT_NAME}.csproj -c ${BUILD_TYPE} -o /app


###### PUBLISH IMAGE #####
FROM build AS publish
ARG PROJECT_NAME
ARG BUILD_TYPE
ARG ENVIRONMENT

RUN dotnet publish ${PROJECT_NAME}.csproj -c ${BUILD_TYPE} -o /app


###### DEPLOYMENT IMAGE #####
FROM base AS final
ARG PROJECT_NAME
ARG ENVIRONMENT

ENV PROJECT_ENTRYPOINT=${PROJECT_NAME}
ENV ASPNETCORE_ENVIRONMENT=${ENVIRONMENT}

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT dotnet ${PROJECT_ENTRYPOINT}.dll