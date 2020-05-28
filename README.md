# Check School Performance Data (Alpha)
 
## Prerequisites
- Visual Studio 2019
- .NET Core 3.1 SDK
- Node
- NPM
- Python 2.7
- Ruby
- Gulp

## Set up
Install the frontend dependencies by running the following from the `Dfe.CspdAlpha.Web.Application` directory:
```
npm install
npm install gulp-cli -g
```
Build `Dfe.CspdAlpha.Web.sln`, either within Visual Studio, or running the following .NET Core CLI command from the solution directory:
```
dotnet build
```
Build the frontend assets (compiled JS and CSS) by running the following from the `Dfe.CspdAlpha.Web.Application` directory:
```
gulp buildDev
```
Run the web application through Visual Studio, or running the following .NET Core CLI command from the solution directory:
```
dotnet run
```
