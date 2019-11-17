using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(PConsulting.Areas.Identity.IdentityHostingStartup))]
namespace PConsulting.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}