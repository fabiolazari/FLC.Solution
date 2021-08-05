using FlcIO.Business.Interfaces;
using FlcIO.Data.Context;
using FlcIO.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlcIO.App
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<MessengerContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
			});

			services.AddControllersWithViews();

			services.AddAutoMapper(typeof(Startup));

			services.AddScoped<MessengerContext>();
			services.AddScoped<IFlcMessageRepository, FlcMessageRepository>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Messenger}/{action=Index}/{id?}");
			});
		}
	}
}
