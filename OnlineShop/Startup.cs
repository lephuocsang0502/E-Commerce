using Owin;
using Microsoft.Owin;


[assembly: OwinStartup(typeof(OnlineShop.Startup))]

namespace OnlineShop
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
            app.MapSignalR();
           
        }
    }
}