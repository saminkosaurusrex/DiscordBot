using Application.Commands.Handlers;
using Application.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service.AddSingleton<ISlashCommandHandler, PingSlashCommandHandler>();
        service.AddSingleton<ISlashCommandHandler, AboutSlashCommandHandler>();
        service.AddSingleton<ISlashCommandHandler, KeksSlashCommandHandler>();
        service.AddSingleton<ISlashCommandHandler, MovieSlashCommandHandler>();
        service.AddSingleton<ISlashCommandHandler, PopularMoviesHandler>();
        service.AddSingleton<ISlashCommandHandler, TomasCommandHandler>();
        service.AddSingleton<ISlashCommandHandler, FilkoCommandHandler>();
        service.AddSingleton<ISlashCommandHandler, FutbalistaCommandHandler>();
        return service;
    }
}