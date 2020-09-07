using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinhasTarefaAPI.Database;
using MinhasTarefaAPI.Models;
using MinhasTarefaAPI.Repositories;
using MinhasTarefaAPI.Repositories.Contracts;
using Newtonsoft.Json;

namespace MinhasTarefaAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Este m�todo � chamado em tempo de execu��o. Use este m�todo para adicionar servi�os ao cont�iner.
        public void ConfigureServices(IServiceCollection services)
        {

            #region  Removendo a valida��o do  modelState
            services.Configure<ApiBehaviorOptions>(op =>
            {
                op.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(option => option.EnableEndpointRouting = false)
            .AddNewtonsoftJson(options =>options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            #endregion


            #region Criando inje��o com o banco de dados  / usando o Sqlite
            services.AddControllers();
            services.AddDbContext<MinhasTarefasContext>(op =>
            {
                op.UseSqlite("Data Source=Database\\MinhasTarefas.db");
            });
            #endregion

            #region Registrando os repositorie como servi�o
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITarefaRepository, TarefaRepository>();
            #endregion

            #region  Trabalhando com autentica��o de usuarios  
            services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<MinhasTarefasContext>();  // j� � a configura��o pra usar o dentity como servi�o
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

         
            app.UseStatusCodePages();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization(); ;
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // app.UseMvc();



        }
    }
}
