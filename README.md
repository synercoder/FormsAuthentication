# FormsAuthentication
Enable ASP.NET Core 3.1 cookies to read old ASP.NET Forms Authentication cookies by implementing a custom `ISecureDataFormat`.

NuGet: [![NuGet Shield](https://img.shields.io/nuget/dt/Synercoding.FormsAuthentication.svg)](https://www.nuget.org/packages/Synercoding.FormsAuthentication/)

Usage:

<pre><code>var section = Configuration.GetSection("FormsAuthentication");

var faOptions = new FormsAuthenticationOptions()
{
    DecryptionKey = section.GetValue&lt;string&gt;("DecryptionKey"),
    ValidationKey = section.GetValue&lt;string&gt;("ValidationKey"),
    EncryptionMethod = section.GetValue&lt;EncryptionMethod&gt;("EncryptionMethod"),
    ValidationMethod = section.GetValue&lt;ValidationMethod&gt;("ValidationMethod"),
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
        options.Cookie.Name = section.GetValue&lt;string&gt;("CookieName");
        options.AccessDeniedPath = "/Login/Upgrade/";
        options.LoginPath = "/Login/";
        options.ReturnUrlParameter = "returnurl";
        <strong>options.TicketDataFormat = new FormsAuthenticationDataFormat&lt;AuthenticationTicket&gt;(
            faOptions,
            FormsAuthHelper.ConvertCookieToTicket,
            FormsAuthHelper.ConvertTicketToCookie
            );</strong>
    });</code></pre>
    
The `FormsAuthHelper.ConvertCookieToTicket` and `FormsAuthHelper.ConvertTicketToCookie` helper methods convert an ASP.NET Core `AuthenticationTicket` to a `FormsAuthenticationCookie` and vise versa. This class contains the same data as a old ASPNET Cookie.
