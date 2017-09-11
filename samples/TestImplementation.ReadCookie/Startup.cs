using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synercoding.FormsAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TestImplementation.ReadCookie
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var section = Configuration.GetSection("FormsAuthentication");

            var faOptions = new FormsAuthenticationOptions()
            {
                DecryptionKey = section.GetValue<string>("DecryptionKey"),
                ValidationKey = section.GetValue<string>("ValidationKey"),
                EncryptionMethod = section.GetValue<EncryptionMethod>("EncryptionMethod"),
                ValidationMethod = section.GetValue<ValidationMethod>("ValidationMethod"),
            };

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.Name = section.GetValue<string>("CookieName");
                    options.AccessDeniedPath = "/Login/";
                    options.LoginPath = "/Login/";
                    options.ReturnUrlParameter = "returnurl";
                    options.TicketDataFormat = new FormsAuthenticationDataFormat<AuthenticationTicket>(
                        faOptions,
                        FormsAuthHelper.ConvertCookieToTicket,
                        FormsAuthHelper.ConvertTicketToCookie
                    );
                    options.SlidingExpiration = false;

                    options.Events.OnRedirectToAccessDenied = context => FormsAuthHelper.RedirectToAccessDenied(context, section.GetValue<string>("BaseAuthUrl"));
                    options.Events.OnRedirectToLogin = context => FormsAuthHelper.RedirectToLogin(context, section.GetValue<string>("BaseAuthUrl"));
                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();
        }
    }
}
