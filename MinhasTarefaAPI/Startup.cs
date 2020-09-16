using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MinhasTarefaAPI.Database;
using MinhasTarefaAPI.V1.Models;
using MinhasTarefaAPI.Repositories;
using MinhasTarefaAPI.V1.Repositories.Contracts;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using MinhasTarefaAPI.V1.Repositories;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using MinhasTarefaAPI.v1.Helpers.Swagger;
using MinhasTarefaAPI.V1.Helpers.Swagger;

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

            //services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddMvc(config => {
                config.EnableEndpointRouting = false;
                config.ReturnHttpNotAcceptable = true;  //406 fora do formato
                config.InputFormatters.Add(new XmlSerializerInputFormatter(config));  // Suporta a entrada de dados no formato xml.
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());     // retorno o xml na saida dos dados
            })




            .AddNewtonsoftJson(options =>options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);



            services.AddApiVersioning(cfg =>
            {
                cfg.ReportApiVersions = true;  // essa opção vai mostra no HEADERS as versiões suportada ex:(spi-suported  = 1.0 ,2.0)  //// ativar a disponibilização do versionamento da API
                cfg.AssumeDefaultVersionWhenUnspecified = true; // parâmetro complementar ao ".DefaultApiVersion"
                cfg.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0); // versão default - padrão ou sugerida.

            });





            services.AddSwaggerGen(cfg => {

                cfg.ResolveConflictingActions(apiDescription => apiDescription.First()); // para resolver conflitos de versões da API no Swagger, com mesmo nome.

                cfg.SwaggerDoc("v1.0", new OpenApiInfo
                {
                    Version = "v1.0",
                    Title = "MinhasTarefas API - v1.0"

                });


                var CaminhoProjeto = PlatformServices.Default.Application.ApplicationBasePath;   //recupera o caminho do projeto...
                var NomeProjeto = $"{PlatformServices.Default.Application.ApplicationName}.xml"; //recupera o nome do projeto...
                var CaminhoArquivoXMLComentario = Path.Combine(CaminhoProjeto, NomeProjeto);
                cfg.IncludeXmlComments(CaminhoArquivoXMLComentario);

                //cfg.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    var actioonApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();

                //    if (actioonApiVersionModel == null)
                //    {
                //        return true;
                //    }

                //    if (actioonApiVersionModel.DeclaredApiVersions.Any())
                //    {
                //        return actioonApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                //    }

                //    return actioonApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
                //});

                //cfg.OperationFilter<ApiVersionOperationFilter>();
            });
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
            services.AddScoped<ITokenRepositorie, TokenRepositorie>();
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
            
            
            app.UseMvc();




            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "MinhaTarefa");
                c.RoutePrefix = string.Empty;
            });


        }
    }
}
