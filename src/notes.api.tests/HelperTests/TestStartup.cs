using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using notes.data;

namespace notes.api.tests.HelperTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureDbContext(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => { options.UseInMemoryDatabase("TestDb"); });
        }
    }
}
