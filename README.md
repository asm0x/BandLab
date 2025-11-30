# BandLab

.net core 10 API Clean architecture

## Getting started

| Project | |
|-|-|
| Aspire| Startup project|
|API| Web api|
|CDN| Mimic of cnd for hosting images|
|Scaling| Worker with several instances to scale images<br>(Events are used to coordinate background image compression)|
|Clients| Test project to execute requests for api<br>(Current settings is 100 rps)|
|Files| Mimic of network file storage, like S3|


API can be configured for MongoDB or Sqlite (EF core) at the App.cs (use one option):<br>
&emsp;// .AddMongoDBRepositories(builder.Configuration.GetSection("MongoDB"))<br>
&emsp;// .AddSqliteRepositories(builder.Configuration.GetSection("Sqlite"))<br>

## Metrics

Performance is measured in metrics:

* http.server.request.duration - Duration of HTTP server requests.
* scaling.scaled - Number of images scaled.
