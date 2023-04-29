using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Repo;
using Infrastructure.Services;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class InfrastructureConfigurationServicesExtension
	{
		public static IServiceCollection AddDatabaseManagerServices(this IServiceCollection services)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
			services.AddSingleton<IContactService, ContactService>();
			services.AddSingleton<IUserService, UserService>();

			return services;
		}
	}
}
