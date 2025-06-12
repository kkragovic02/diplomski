using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Zora.Core.AttractionServices;
using Zora.Core.AttractionServices.Models;
using Zora.Core.Database;
using Zora.Core.Database.Models;
using Zora.Core.DestinationServices;
using Zora.Core.DiaryNoteServices;
using Zora.Core.EquipmentServices;
using Zora.Core.TourServices;
using Zora.Core.UserServices;

namespace Zora.Core;

public static partial class ServiceProviderConfig
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddDbContext<ZoraDbContext>(options =>
            options.UseSqlServer(
                connectionString: "Server=localhost\\MSSQLSERVER02;Database=ZoraDB;User Id=sa3;Password=P@ssw0rd;TrustServerCertificate=True;"
            )
        );

        return services;
    }

    public static IServiceCollection AddRequiredServices(this IServiceCollection services)
    {
        services.AddScoped<IUserReadService, UserReadService>();
        services.AddScoped<IUserWriteService, UserWriteService>();

        services.AddScoped<PasswordHasher<UserModel>>();

        services.AddScoped<IDestinationReadService, DestinationReadService>();
        services.AddScoped<IDestinationWriteService, DestinationWriteService>();

        services.AddScoped<ITourReadService, TourReadService>();
        services.AddScoped<ITourWriteService, TourWriteService>();

        services.AddScoped<IEquipmentReadService, EquipmentReadService>();
        services.AddScoped<IEquipmentWriteService, EquipmentWriteService>();

        services.AddScoped<IAttractionReadService, AttractionReadService>();
        services.AddScoped<IAttractionWriteService, AttractionWriteService>();

        services.AddScoped<IDiaryNoteReadService, DiaryNoteReadService>();
        services.AddScoped<IDiaryNoteWriteService, DiaryNoteWriteService>();

        return services;
    }
}
