FROM  mcr.microsoft.com/dotnet/aspnet:8.0-cbl-mariner2.0 AS runtime-image
LABEL maintainer="Travis Sharp <furiousscissors@gmail.com>"

## Upgrade will install any possible package updates if available
RUN tdnf repolist --refresh && \
    tdnf -y install gettext && \
    tdnf -y upgrade && \
    tdnf -y update

RUN echo "Cache Buster Cache Cleanup ..." \
    tdnf clean all

ENV ASPNETCORE_URLS=http://+80
EXPOSE 80

# Build runtime image
FROM base AS final
WORKDIR /home/site/wwwroot
COPY ./ ./
RUN chmod -R a-w ./

# Set user and group
ARG user=kuiper-service-api
ARG group=kuiper-service-api
ARG uid=1000
ARG gid=1000
RUN groupadd -g ${gid} ${group}
# <--- the '-m' create a user home directory
RUN useradd -u ${uid} -g ${group} -s /bin/sh -m ${user}
RUN chown -R kuiper-service-api ../wwwroot

# Switch to user
USER ${uid}:${gid}

ENTRYPOINT ["dotnet", "Kuiper.Clustering.ServiceApi.dll"]