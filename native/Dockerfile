FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1.3-buster-slim
RUN apt update && apt install -y libicu63 libicu-dev gcc make
WORKDIR /app
COPY ./ ./
RUN make
