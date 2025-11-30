# BandLab

.net core 10 API Clean architecture

## Getting started

Aspire: Startup project
API: Web api
CDN: Mimic of cnd for hosting images
Scaling: Worker with several instances to scale images
Clients: Test project to execute requests for api.
	Current settings is 100 RPS
FileStorage - Mimic of network file storage, like S3.
Scale... events are used to coordinate background image compression.


API can be configured for MongoDB or Sqlite (EF core) at the App.cs (use one option)
    // .AddMongoDBRepositories(builder.Configuration.GetSection("MongoDB"))
	// .AddSqliteRepositories(builder.Configuration.GetSection("Sqlite"))

## Metrics

Performance is measured in metrics:

* http.server.request.duration - Duration of HTTP server requests.
* scaling.scaled - Number of images scaled.
