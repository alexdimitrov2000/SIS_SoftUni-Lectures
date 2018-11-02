using ChushkaWebApplication.Data;
using SIS.MvcFramework;

namespace ChushkaWebApplication.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            this.Context = new ChushkaDbContext();
        }

        public ChushkaDbContext Context { get; }
    }
}
