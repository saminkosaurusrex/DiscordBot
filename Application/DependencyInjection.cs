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
        return service;
    }
}