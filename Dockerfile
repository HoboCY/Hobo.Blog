#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM registry.cn-shanghai.aliyuncs.com/hobocy/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM registry.cn-shanghai.aliyuncs.com/hobocy/sdk:5.0 AS build
WORKDIR /src
COPY ["host/Blog.MVC/Blog.MVC.csproj", "host/Blog.MVC/"]
COPY ["src/Blog.Data/Blog.Data.csproj", "src/Blog.Data/"]
COPY ["src/Blog.Extensions/Blog.Extensions.csproj", "src/Blog.Extensions/"]
COPY ["src/Blog.Infrastructure/Blog.Infrastructure.csproj", "src/Blog.Infrastructure/"]
COPY ["src/Blog.Service/Blog.Service.csproj", "src/Blog.Service/"]
COPY ["src/Tencent.COS.SDK/Tencent.COS.SDK.csproj", "src/Tencent.COS.SDK/"]
RUN dotnet restore "host/Blog.MVC/Blog.MVC.csproj"
COPY . .
WORKDIR "/src/host/Blog.MVC"
RUN dotnet build "Blog.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.MVC.dll"]