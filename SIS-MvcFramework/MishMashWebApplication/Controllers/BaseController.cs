using MishMashWebApplication.Data;
using SIS.MvcFramework;

namespace MishMashWebApplication.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Context = new MishMashDbContext();
        }

        protected MishMashDbContext Context { get; set; }
    }
}
