ARG SOLUTION_NAME=TimejApi
ARG PROJECT_NAME=TimejApi
ARG TEST_PROJECT_NAME=TimejApi.Tests
ARG BUILD_TYPE=Debug


###### BASE IMAGE ######
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine3.17-amd64 AS base
WORKDIR /app
EXPOSE 80


###### BUILD IMAGE ######
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine3.16-amd64 AS build
ARG SOLUTION_NAME
ARG PROJECT_NAME
ARG TEST_PROJECT_NAME
ARG BUILD_TYPE

WORKDIR /src
COPY ${SOLUTION_NAME}.sln .
COPY ${PROJECT_NAME}/${PROJECT_NAME}.csproj ${PROJECT_NAME}/${PROJECT_NAME}.csproj
COPY ${TEST_PROJECT_NAME}/${TEST_PROJECT_NAME}.csproj ${TEST_PROJECT_NAME}/${TEST_PROJECT_NAME}.csproj

WORKDIR /src/${TEST_PROJECT_NAME}
RUN dotnet add package JunitXml.TestLogger --version 3.0.124
WORKDIR /src
RUN dotnet restore
COPY . .
RUN dotnet build ${TEST_PROJECT_NAME}/${TEST_PROJECT_NAME}.csproj -c ${BUILD_TYPE} -o /app


###### TEST IMAGE ######
FROM build AS test
ARG TEST_PROJECT_NAME

ENV PROJECT_ENTRYPOINT=${TEST_PROJECT_NAME}

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT dotnet test --logger:junit ${PROJECT_ENTRYPOINT}.dll
