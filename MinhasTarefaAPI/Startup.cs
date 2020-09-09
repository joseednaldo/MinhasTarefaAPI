using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MinhasTarefaAPI.Database;
using MinhasTarefaAPI.Models;
using MinhasTarefaAPI.Repositories;
using MinhasTarefaAPI.Repositories.Contracts;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

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
        // Este método é chamado em tempo de execução. Use este método para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {

            #region  Removendo a validação do  modelState
            services.Configure<ApiBehaviorOptions>(op =>
            {
                op.SuppressModelStateInvalidFilter = true;
            });

            services.AddMvc(option => option.EnableEndpointRouting = false)
            .AddNewtonsoftJson(options =>options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            #endregion


            #region Criando injeção com o banco de dados  / usando o Sqlite
            services.AddControllers();
            services.AddDbContext<MinhasTarefasContext>(op =>
            {
                op.UseSqlite("Data Source=Database\\MinhasTarefas.db");
            });
            #endregion

            #region Registrando os repositorie como serviço
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITarefaRepository, TarefaRepository>();
            #endregion

            #region  Trabalhando com autenticação de usuarios  
            /*Já é a configuração pra usar o dentity como serviço
             AddDefaultIdentity  = CHAMA O UI, nossa api não precisa disso, serve pra trazer as telas de interface graficas.
             pra resolver isso retiramos o Default. Evita chamar de tela de login...
            */
            //services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<MinhasTarefasContext>(); 
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MinhasTarefasContext>()
                .AddDefaultTokenProviders();   //habilitanto o uso do Token.

            #region
            services.AddAuthentication(options =>{
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    //aqui indicamos o que será validado ou seja quais parametros vamos verificar pra validar o token.
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,       //data de  expires:exp,
                    ValidateIssuerSigningKey=true,  //validando a chave do emissor
                    //como é a chave do e emissor
                    IssuerSigningKey=  new SymmetricSecurityKey(Encoding.UTF8.GetBytes("chave-api-jwt-minhas-tarefas")) //O ideal é criar a chave(texto) no appsettings.js

                };
            });


            #region
            
            //Verifica o esquema de autenticação,verifica o usuario e 
            services.AddAuthorizationCore(auto =>
            {

                auto.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build()
                    );

            });
            #endregion

            #endregion


                /*Configurando o serviço para quando o usuário nao tiver logado, vamos tratar a mensagem 404*/
                services.ConfigureApplicationCookie(options=>{
                options.Events.OnRedirectToLogin = context => { 
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });
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
