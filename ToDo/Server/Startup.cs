using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ToDo.Entity;

namespace ToDo.Server
{
    public class Startup
    {
        /// <summary>
        ///     �����־
        /// </summary>
        public static readonly ILoggerFactory
            loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddDbContext<TodoContext>(r =>
                // r.UseMySql(
                //         Configuration.GetConnectionString("MySqlConnection"),
                //         new MariaDbServerVersion(new Version(8, 0, 21)),
                //         mySqlOptions => mySqlOptions
                //             .CharSetBehavior(CharSetBehavior.NeverAppend))
                //     .EnableSensitiveDataLogging()
                //     .EnableDetailedErrors()
                r.UseSqlite(@"Data Source=data.db")
            // options.UseSqlite(@"Data Source=C:\blogging.db");
            );

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, //�Ƿ���֤Issuer
                        ValidateAudience = true, //�Ƿ���֤Audience
                        ValidateLifetime = true, //�Ƿ���֤ʧЧʱ��
                        ValidateIssuerSigningKey = true, //�Ƿ���֤SecurityKey
                        ValidAudience = "guetClient", //Audience
                        ValidIssuer = "guetServer", //Issuer���������ǩ��jwt������һ��
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes("123456789012345678901234567890123456789")) //�õ�SecurityKey
                    };
                });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}