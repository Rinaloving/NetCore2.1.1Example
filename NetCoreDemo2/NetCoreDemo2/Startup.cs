using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreDemo2.Models;

namespace NetCoreDemo2
{
    public class Startup
    {

        public Startup()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json");
            Configuration = builder.Build();  //json文件所在目录 C:\Users\Administrator\source\repos\NetCoreDemo2\NetCoreDemo2\bin\Debug\netcoreapp2.0
        }

        public IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //注册MVC服务
            services.AddMvc();

            //添加服务，连接数据库
            services.AddEntityFrameworkSqlite()
            .AddDbContext<HelloWorldDBContext>
           (options => options.UseSqlite(Configuration["database:connection"]));

            //添加Identity 服务和所有依赖的相关服务
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<HelloWorldDBContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //异常和错误处理
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //添加另一个中间件，必须要在Run()之前
            //app.UseWelcomePage();


            //让程序启动就调用静态文件
            //app.UseDefaultFiles();
            //添加调用静态文件的中间件
            //  app.UseStaticFiles();


            //输出结果和同时使用UseDefaultFiles 和 UseStaticFiles 效果一样

            app.UseFileServer();

            //但如果要在 MVC 控制器中进行授权检查，又需要在 MVC 框架之前插入 Identity 中间件，
            //以确保 cookie 和 401 错误得到成功处理
            app.UseAuthentication();

              app.UseMvcWithDefaultRoute(); //MVC 模式

            //app.UseMvc(ConfigureRoute);  //身份验证

            //app.Run(async (context) =>
            //{
            //    //模拟异常，看异常处理是否生效
            //    //throw new System.Exception("Throw Exception");

            //    var msg = Configuration["message"];

            //    await context.Response.WriteAsync(msg);
            //});

        }

        //配置路由
        private void ConfigureRoute(IRouteBuilder routeBuilder)
        {
            //Home/Index
            routeBuilder.MapRoute("Default","{controller}/{action}/{id?}");
        }


       

       
    }
}
