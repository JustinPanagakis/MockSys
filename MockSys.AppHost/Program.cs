using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

// Add PostgreSQL

//var postgres = builder
//    .AddPostgres("mocksys-postgres", port: 58888)
//    .WithContainerName("mocksys-postgres")
//    .WithLifetime(ContainerLifetime.Persistent)
//    .WithDataVolume(isReadOnly: false);

var postgres = builder
    .AddPostgres("mocksys-postgres")
    .WithContainerName("mocksys-postgres")
    .WithEndpoint("mocksys-postgres", ep =>
    {
        ep.Port = 58888;
        ep.TargetPort = 5432;
        ep.Protocol = ProtocolType.Tcp;
        ep.UriScheme = "tcp";
        ep.IsProxied = false;
    })
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume(isReadOnly: false);

var postgresDb = postgres
    .AddDatabase("mocksys");

// Add Service Bus

var serviceBus = builder.AddAzureServiceBus("mocksys-servicebus")
    .RunAsEmulator();

serviceBus.AddServiceBusQueue("sync-queue");

serviceBus.AddServiceBusQueue("reports-queue");

serviceBus.AddServiceBusQueue("metrics-queue");


// Add projects to the application builder

var fakeDataApi = builder.AddProject<Projects.MockSys_FakeDataApi>("mocksys-fakedataapi");

builder.AddProject<Projects.MockSys_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(fakeDataApi)
    .WithReference(postgresDb)
    .WaitFor(postgresDb)
    .WaitFor(fakeDataApi);

builder.AddAzureFunctionsProject<Projects.MockSys_Reporting_DataSync>("mocksys-reporting-datasync")
    .WithEnvironment("ASPNETCORE_URLS", "http://0.0.0.0:7242")
    .WithReference(postgresDb)
    .WithReference(serviceBus)
    .WithReference(fakeDataApi)
    .WaitFor(postgresDb)
    .WaitFor(serviceBus)
    .WaitFor(fakeDataApi);

builder.AddAzureFunctionsProject<Projects.MockSys_Reporting_ReportETL>("mocksys-reporting-reportetl")
    .WithEnvironment("ASPNETCORE_URLS", "http://0.0.0.0:7243")
    .WithReference(postgresDb)
    .WithReference(serviceBus)
    .WaitFor(postgresDb)
    .WaitFor(serviceBus);

builder.Build().Run();
