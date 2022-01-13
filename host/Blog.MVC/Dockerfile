#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["host/Blog.MVC/Blog.MVC.csproj", "host/Blog.MVC/"]
COPY ["src/Blog.Service/Blog.Service.csproj", "src/Blog.Service/"]
COPY ["src/Blog.ViewModels/Blog.ViewModels.csproj", "src/Blog.ViewModels/"]
COPY ["src/Blog.Extensions/Blog.Extensions.csproj", "src/Blog.Extensions/"]
COPY ["src/Blog.Shared/Blog.Shared.csproj", "src/Blog.Shared/"]
COPY ["src/Blog.Exceptions/Blog.Exceptions.csproj", "src/Blog.Exceptions/"]
COPY ["src/Blog.Data/Blog.Data.csproj", "src/Blog.Data/"]
COPY ["src/Blog.Permissions/Blog.Permissions.csproj", "src/Blog.Permissions/"]
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