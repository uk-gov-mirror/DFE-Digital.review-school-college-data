# Review my school or college data (Private beta)
 
## Prerequisites
- Visual Studio 2019
- .NET 5.0 SDK
- Node
- NPM
- Python 2.7
- Ruby

## Set up
Install the frontend dependencies by running the following from the `Dfe.CspdAlpha.Web.Application` directory:
```
npm install
```
Build `Dfe.CspdAlpha.Web.sln`, either within Visual Studio, or running the following .NET Core CLI command from the solution directory:
```
dotnet build
```
Build the frontend assets (compiled JS and CSS) by running the following from the `Dfe.CspdAlpha.Web.Application` directory:
```
npm buildDev
```
Run the web application through Visual Studio, or running the following .NET Core CLI command from the solution directory:
```
dotnet run
```
