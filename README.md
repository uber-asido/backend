# SF Movies backend

There are lots of free data available with movie filming locations. The service is a backend system, that provides an [OData](http://www.odata.org/) API allowing to search movies, their filming locations as well as to populate the database with new data.

The project is written in **C#** and uses **ASP.NET Core** - a cross-platform, high-performance, open-source framework. Currently supported storage is **PosgreSQL**. The solution runs on Windows, MacOS and Linux.

## System architecture

![Service architecture](https://raw.githubusercontent.com/uber-asido/backend/8e81c22031143e80f349c10865465125a430d2ce/images/service-arch.png)

The system is a monolith, but divided into 4 separate modules.

Module | Description
--- | ---
Movie | Movie module is the core module storing movie related information, such as movie metadata (title, release year), production company, distributor, director, writers and actors. It also stores information about shooting locations. The primary job of the module is to merge new incomming information about movies, communicate with the search module to index searchable information, and serve that information back when requested.
File | File module accepts files with movie data and is responsible for converting that data into what **Movie** module accepts. It uses **Geocoding** module to resolve location names to latitude & longitude coordinates. Currently accepted file format is CSV.
Search | Search module, as the name implies, is a generic search engine that other modules can use to index certain information. It is able to perform full-text search as well as query information based on it's type. Search items are fast to query small objects perfect for implementing autocompletion.
Geocoding | Geocoding module is a service able to resolve location strings, such as `Montgomery between Green and Broadway` to a structured address `Montgomery St & E Broadway, New York, NY 10002, USA, (40.7144612, -73.9853415)`. Currently it includes a [Google Geocoding API](https://developers.google.com/maps/documentation/geocoding/intro) provider, but is designed to support any form of resolution. All you need to do is implement `IGeocodeProvider`.

## Module architecture

![Module architecture](https://raw.githubusercontent.com/uber-asido/backend/8e81c22031143e80f349c10865465125a430d2ce/images/module-arch.png)

Each module is further broken down into several components. A typical module has 5 components, that are shown in the figure above. Abstractions contain the interfaces to the services the module provides, the data it accepts and returns, as well as interfaces to the storages the service is using. Service logic and the store implements those abstractions, while API and test components consume them. Modules also talk with one another, but only via the interfaces defined in the abstractions. No module can ever access other module internals, including know anything about the storage it uses.

![Module architecture](https://raw.githubusercontent.com/uber-asido/backend/52891e332f6b78de7d4ec2deab9e8d7d7265b6fd/images/store-arch.png)

Abstractions can have multiple implementations. That is especially common for stores. It is typical to have a persistent store for production and development, while use in-memory or mock store for testing.

![Store implementations](https://raw.githubusercontent.com/uber-asido/backend/eab076130c41ba17a6e1dd64be98781b816d8c24/images/store-impl.png)

Storage entities and database technology are strictly encapsulated in the store implementations, where even the service logic doesn't know how the data it asks to save is stored. It can be stored as simply as an in-memory object reference in a list, or as complicated as a normalized data in a dozen of tables somewhere in the cloud.

## Gateway

The gateway server aggregates the modules into a monolith and exposes an [OData](http://www.odata.org/) API to the public. The architecture, however, is extremely flexible. Modules get dependencies via dependency injection. Since dependencies are referenced via abstractions, actual implementations can be either a reference to an instance within the same module or a even a reference to a client library, which communicates with the dependency on another machine via REST or other form of RPC. Such architecture allows to run each module on individual machines if there is a need with no dependee source code changes.

## OData API

[OData](http://www.odata.org/) API was mentioned several times in this document. For those not familiar with the term, it stands for **Open Data Protocol**. It is an [ISO/IEC approved](https://www.oasis-open.org/news/pr/iso-iec-jtc-1-approves-oasis-odata-standard-for-open-data-exchange), [OASIS standard](https://www.oasis-open.org/committees/tc_home.php?wg_abbrev=odata) that defines a set of best practices for building and consuming RESTful APIs.

It offers several advantages over a custom made REST API.
* It is an open standard and ensures consistent and clean API. Server OData libraries are able to validate the API to make sure that the registered endpoints and entity collections comply with the standrd.
* It provides a machine-readable description of the data model of the API, called **OData metadata**. It enables the use of powerful generic client proxies and tools to consume the API. As an example, there are tools that can consume the metadata and generate API client code for dozens of programming language. On top of that, popular office suits, such as Microsoft Excel and LibreOffice Calc can use OData API as a data source.
* It provides a standardized [query option syntax](http://docs.oasis-open.org/odata/odata/v4.0/odata-v4.0-part2-url-conventions.html), which allow clients to tailor the requests and their responses to their needs. Such as sepecify data filters, ordering, paging, property selection and transformation, etc.

This service currently exposes 4 entity sets.

| Endpoint |
| --- |
| [/odata/UploadHistory](https://uber-asido.azurewebsites.net/odata/UploadHistory) | 
| [/odata/FilmingLocation](https://uber-asido.azurewebsites.net/odata/FilmingLocation) |
| <a href="https://uber-asido.azurewebsites.net/odata/Movie(fa55cc41-09ce-4d59-a295-835219ca1cdd)">/odata/Movie</a> |
| [/odata/SearchItem](https://uber-asido.azurewebsites.net/odata/SearchItem) |

A complete service OData metadata can be found [here](https://uber-asido.azurewebsites.net/odata/$metadata).

OData also has a concept of **functions** and **actions**. A function is a **GET** call, that has no side effects. An action is a **POST** request, usually with a side effect. **Functions** and **actions** can be bounded to a specific single entity, bounded to an entire collection, or unbounded (global). The scope influences a calling convention. For an unbounded method, one can simply issue:

```/odata/MyMethod```

Bounded collection methods require an entity set name and a method namespace:

```/odata/UploadHistory/Service.MyMethod```

Bounded entity method requires an additional entity key:

```/odata/UploadHistory(16861f90-3db4-4d86-85ff-df775d289edf)/Service.MyMethod```

The methods can also take parameters:

```/odata/UploadHistory/Service.MyMethod(param1='hello')```

This service exposes 2 bounded collection **functions** as well as 1 unbounded **action**.

Type | Endpoint | Parameters
--- | --- | ---
Function | <a href="https://uber-asido.azurewebsites.net/odata/FilmingLocation/Service.SearchByFreeText(text='star')">/odata/FilmingLocation/Service.SearchByFreeText</a> | **string** text
Function | <a href="https://uber-asido.azurewebsites.net/odata/FilmingLocation/Service.SearchBySearchItem(searchItemKey=c70c314f-6b50-4bf8-8654-4aad00642378)">/odata/FilmingLocation/Service.SearchBySearchItem</a> | **uuid** searchItemKey
Action | /odata/UploadFile | **stream** file

Query examples:

Method | URL | Description
--- | --- | ---
GET | [/odata/UploadHistory](https://uber-asido.azurewebsites.net/odata/UploadHistory) | List entire upload history.
GET | <a href="https://uber-asido.azurewebsites.net/odata/Movie(fa55cc41-09ce-4d59-a295-835219ca1cdd)">/odata/Movie(fa55cc41-09ce-4d59-a295-835219ca1cdd)</a> | Get a specific movie.
GET | <a href="https://uber-asido.azurewebsites.net/odata/Movie(fa55cc41-09ce-4d59-a295-835219ca1cdd)?$select=title,releaseYear">/odata/Movie(fa55cc41-09ce-4d59-a295-835219ca1cdd)?$select=title,releaseYear</a> | Get title and release year of a specific movie.
GET | <a href="https://uber-asido.azurewebsites.net/odata/SearchItem?$filter=contains(tolower(text), 'star')">/odata/SearchItem?$filter=contains(tolower(text), 'star'))</a> | Get search results, where text contains substring `star`.
GET | [/odata/SearchItem/$count](https://uber-asido.azurewebsites.net/odata/SearchItem/$count) | Count search items in the index.
GET | <a href="https://uber-asido.azurewebsites.net/odata/FilmingLocation/Service.SearchBySearchItem(searchItemKey=c70c314f-6b50-4bf8-8654-4aad00642378)">/odata/FilmingLocation/Service.SearchBySearchItem(searchItemKey=c70c314f-6b50-4bf8-8654-4aad00642378)</a> | Get filming locations for a movie, that is linked with a specific search item (such as selected autocompletion).

## Run service

### Prerequisites

Install [.NET Core 2.1 RC-1](https://www.microsoft.com/net/download).

Overwrite project configuration by creating a new file at `src/Uber.Server.Gateway/AppSettings.User.json`. In the file you have to provide database connection strings:
```json
{
    "ConnectionStrings": {
        "File": "<PostgreSQL connection string>",
        "Geocoding": "<PostgreSQL connection string>",
        "Movie": "<PostgreSQL connection string>",
        "Search": "<PostgreSQL connection string>",
        "Hangfire": "<PostgreSQL connection string>"
    }
}
```

All connection strings can point either to the same database or separate database servers.

#### Example

Here is an example how to create the required databases.

Login to psql `sudo -u postgres psql` and run the following script:

```sql
create role uber with createdb;
alter role uber with password 'x';

create database uber_file with owner uber;
create database uber_geocoding with owner uber;
create database uber_movie with owner uber;
create database uber_search with owner uber;
create database uber_hangfire with owner uber;
```

Afterwards save the following `AppSettings.User.json`:

```json
{
    "ConnectionStrings": {
        "File": "Server=localhost;Port=5432;Database=uber_file;User Id=uber;Password=x;",
        "Geocoding": "Server=localhost;Port=5432;Database=uber_geocoding;User Id=uber;Password=x;",
        "Movie": "Server=localhost;Port=5432;Database=uber_movie;User Id=uber;Password=x;",
        "Search": "Server=localhost;Port=5432;Database=uber_search;User Id=uber;Password=x;",
        "Hangfire": "Server=localhost;Port=5432;Database=uber_hangfire;User Id=uber;Password=x;"
    }
}
```

### Build

```
cd src/Uber.Server.Gateway
dotnet restore
dotnet build
```

### Test

Every module contains a test project. The current source code doesn't have complex logic, therefore most of the tests are integration tests. Modules are tested in isolation. If it has a dependency service from another module, `Uber.Core.Test` project provides mocked services.

Test projects currently have one code smell - database connection strings are hardcoded in the fixtures. If they don't match your configuration, then modify them in the following files:

```
src/Uber.Module.File.Test/FileFixture.cs
src/Uber.Module.Geocoding.Test/GeocodingFixture.cs
src/Uber.Module.Movie.Test/MovieFixture.cs
src/Uber.Module.Search.Test/SearchFixture.cs
```

To run tests run `dotnet test` from the root directory.

### Run

```
dotnet run
```

Visit `http://localhost:50519/odata/$metadata` to print OData metadata.

### Visual Studio

You can also open the project in [Visual Studio](https://www.visualstudio.com/) on MacOS or Windows, so that you don't need to do **build**, **test** and **run** steps from a command line.

### Docker

Create a docker image: `docker build -t uber-backend .`

Run docker image: `docker run -p 8080:80 uber-backend`

**NOTE:** Docker builds a release build, which doesn't include `AppSettings.User.json` configuration. Docker builds require database connection strings to be setup in `AppSettings.Production.json` file.

### Monitoring

`Uber.Server.Gateway` has [Application Insights](https://azure.microsoft.com/en-us/services/application-insights/) configured, which collects a lot of runtime information, such as API response times, unhandled exceptions, various machine parameters, etc. It is able to notice and inform any unusual activity, such as increased latency, increased number of certain HTTP status codes.

## To be done

The project is straightforward and there were no technical trade-offs taken during the development. The only real trade-off was my time, which I didn't spare. I care about craftsmanship and so I invested time to perfect the interfaces and to create an architecture that scales. However, as with anything, there is always room to improve. Among those are:

* Database connection strings for tests are now hardcoded in test project initializers. Ideally should move it to a config file.
* Some obvious optimizations could be considered, such as caching filming location coordinates. Currently geocoding module is used to lookup and fill that information every time the data is queried.
* The search module uses PostgreSQL as it's store. It works for now well and decouples the service from additional 3rd party software and libraries, but is not optimal if the searchable data grows. A specialized store, such as Apache Solr or Elasticsearch could do a much better job in terms of performance and query flexiblity.

However, none of the missing bits make the system less production-ready.

## Live

A web client using the API is available at:
https://uber-frontend.azurewebsites.net/

[Source code](https://github.com/uber-asido/frontend)

## Author

Arvydas Sidorenko

[Resume](https://drive.google.com/open?id=1rp6DXQoXR7WZFy2ogMdRpWNTGJtF22EA)

[LinkedIn](www.linkedin.com/in/arvydassidorenko)

## Related projects
[SF Movies frontend](https://github.com/uber-asido/frontend)
