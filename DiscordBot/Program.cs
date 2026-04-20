using Application;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);

builder.Services
    .AddAplication()
    .AddInfrastructure(builder.Configuration);

var host = builder.Build();
await host.RunAsync();