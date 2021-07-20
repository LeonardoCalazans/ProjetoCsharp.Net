using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Project.Repository.Repositories;
using Projeto.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projetc.Presentation.Mvc
{
    public class Startup
    {
        //construtor
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        //interface utilizada para ler o arquivo appsettings.json
        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //não estava vazio e sim com o codigo comentado abaixo
            //services.AddRazorPages();
            //services.AddMvc(); //NET CORE 2.1
            services.AddControllersWithViews(); //NET CORE 3.1

            //mapear as classes ClienteRepository e DependenteRepository
            //de forma que possamos passar o caminho da connectionstring
            //para estas classes
            var connectionString = Configuration.GetConnectionString("ProjetoMVC01");

            services.AddTransient
           (map => new ClientRepository(connectionString));

            services.AddTransient
           (map => new DependenteRepository(connectionString));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();  //NET CORE 2.1 ou 3.1


            app.UseRouting();  //NET CORE 3.1

            //NET CORE 2.1
            /*
            app.UseMvc(
            routes =>
            {
            routes.MapRoute(
            name: "default",
           template: "{controller=Home}/{action=Index}");
            }); 
            */

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{Action=Index}"
                    );
            });
        }
    }
}
