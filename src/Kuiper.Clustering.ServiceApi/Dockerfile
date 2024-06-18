# Stage 1: Build stage
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 as build
ARG BUILD_CONFIG='Debug'

WORKDIR /app
COPY ./bin/${BUILD_CONFIG} .

# Stage 2: Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-cbl-mariner2.0 AS runtime
LABEL maintainer="Travis Sharp <furiousscissors@gmail.com>"

# Update and install necessary packages
RUN tdnf repolist --refresh && \
    tdnf -y install gettext shadow-utils && \
    tdnf -y upgrade && \
    tdnf -y update && \
    tdnf clean all

ENV ASPNETCORE_URLS=http://+:80

# Set user and group
ARG user=kuiper-service-api
ARG group=kuiper-service-api
ARG uid=1000
ARG gid=1000
RUN groupadd -g ${gid} ${group} && \
    useradd -u ${uid} -g ${group} -s /bin/sh -m ${user}

# Create and set permissions for the app directory
RUN mkdir -p /app && chown -R ${uid}:${gid} /app

# Set working directory
WORKDIR /app

# Copy the .NET Core application from the build stage
COPY --chown=${uid}:${gid} --from=build /app .

# Set restrictive permissions on the app directory
RUN chmod -R a-w /app

# Create a data directory and set permissions for the non-root user
RUN mkdir /var/lib/kuiperdb && chown ${uid}:${gid} /var/lib/kuiperdb && chmod 770 /var/lib/kuiperdb

# Switch to non-root user
USER ${uid}:${gid}

# Expose the port on which the app runs
EXPOSE 80

# Specify the entry point for the container
ENTRYPOINT ["dotnet", "Kuiper.Clustering.ServiceApi.dll"]
