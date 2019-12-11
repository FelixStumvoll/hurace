FROM mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
COPY ./prod.sql /
USER root
COPY ./init.sh /usr/local/bin/
COPY ./entrypoint.sh /usr/local/bin/

RUN chmod +x /usr/local/bin/init.sh
RUN chmod +x /usr/local/bin/entrypoint.sh
ENTRYPOINT ["sh","/usr/local/bin/entrypoint.sh" ]