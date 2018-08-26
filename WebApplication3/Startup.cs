using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication3.Framework.Models;
using WebApplication3.Register;

namespace WebApplication3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //public void ConfigureServices(IServiceCollection services)
        //{

        //    services.AddDbContext <ApplicationDb>(options =>
        //   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        //    services.AddMvc();
        //}
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDb>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/Denied");
                });
            services.AddMvc();
            return InitIoC(services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        /// <summary>
        /// IoC初始化
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private IServiceProvider InitIoC(IServiceCollection services)
        {
            //var connectionString = Configuration.GetConnectionString("PostgreSQL");
            //var dbContextOption = new DbContextOption
            //{
            //    ConnectionString = connectionString,
            //    ModelAssemblyName = "Zxw.Framework.Website.Models",
            //    DbType = DbType.NPGSQL
            //};
            //var codeGenerateOption = new CodeGenerateOption
            //{
            //    ModelsNamespace = "Zxw.Framework.Website.Models",
            //    IRepositoriesNamespace = "Zxw.Framework.Website.IRepositories",
            //    RepositoriesNamespace = "Zxw.Framework.Website.Repositories",
            //    IServicsNamespace = "Zxw.Framework.Website.IServices",
            //    ServicesNamespace = "Zxw.Framework.Website.Services"
            //};
            //IoCContainer.Register(Configuration);//注册配置
            //IoCContainer.Register(dbContextOption);//注册数据库配置信息
            //IoCContainer.Register(codeGenerateOption);//注册代码生成器相关配置信息
            //IoCContainer.Register(typeof(DefaultDbContext));//注册EF上下文
            IoCContainer.Register("WebApplication3.Framework.Repositorys", "WebApplication3.Framework.IRepositorys");//注册仓储
            //IoCContainer.Register("Zxw.Framework.Website.Services", "Zxw.Framework.Website.IServices");//注册service
            return IoCContainer.Build(services);
        }
    }
}
