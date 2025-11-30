var solution = DistributedApplication.CreateBuilder(args);

var queues = solution.AddRabbitMQ("queues")
    .WithManagementPlugin();
/*
var db = solution.AddMongoDB("db")
    .WithLifetime(ContainerLifetime.Session);
*/
var cdn = solution.AddProject<Projects.CDN>("cdn")
    .WithReference(queues)
    .WaitFor(queues);

var scale = solution.AddProject<Projects.Scaling>("scale")
    .WithReference(queues)
    .WithReplicas(3)
    .WaitFor(queues);

var api = solution.AddProject<Projects.API>("api")
    .WithReference(queues)
    // .WithReference(db)
    .WithReference(cdn)
    // .WaitFor(db)
    .WaitFor(queues);
/*
 * Test project to generate load
var clients = solution.AddProject<Projects.Clients>("clients")
    .WithReference(api)
    .WaitFor(api);
*/
solution
    .Build()
    .Run();
