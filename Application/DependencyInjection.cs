using Application.Commands.Handlers;
using Application.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAplication(this IServiceCollection service)
    {
        service.AddSingleton<ISlashCommandHandler, PingSlashCommandHandler>();
        return service;
    }
}