FROM mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
COPY ./prod.sql /
USER root
ENV SA_PASSWORD=EHq(iT|@A4q
ENV ACCEPT_EULA=Y
COPY ./init.sh /
COPY ./entrypoint.sh /

RUN chmod +x ./init.sh
RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh