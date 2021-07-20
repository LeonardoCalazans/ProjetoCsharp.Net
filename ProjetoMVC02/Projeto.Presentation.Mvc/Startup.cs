using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Projeto.Infra.Data.Repositories;

namespace Projeto.Presentation.Mvc
{
    public class Startup
    {
        //construtor para inicialização
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //prop + 2x[tab]
        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Habilitar o projeto para MVC
            services.AddControllersWithViews();

            //Habilitar a autenticação por meio de cookies
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            //ler a connectionstring mapeada no arquivo appsettings.json
            var connectionString = Configuration.GetConnectionString("ProjetoMVC02");

            //configurar as classes UsuarioRepository e CompromissoRepository para serem
            //inicializadas pelo Startup (execução do projeto)
            services.AddTransient(map => new UsuarioRepository(connectionString));
            services.AddTransient(map => new CompromissoRepository(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(); //habilitar a pasta \wwwroot   
            app.UseRouting(); //habilitar a navegação (Controllers/Views)

            app.UseCookiePolicy(); //habilitar autenticação por meio de cookies
            app.UseAuthentication(); //habilitar autenticação por meio de cookies
            app.UseAuthorization(); //habilitar autenticação por meio de cookies

            //mapeamento da rota inicial do projeto
            app.UseEndpoints(
                endpoints => {
                    endpoints.MapControllerRoute(
                        name: "default", //define o padrão de navegação
                        pattern: "{controller=Account}/{action=Login}"
                        );
                });
        }
    }
}
