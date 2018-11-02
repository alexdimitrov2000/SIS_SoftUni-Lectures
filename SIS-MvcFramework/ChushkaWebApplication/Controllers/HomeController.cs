using ChushkaWebApplication.ViewModels.Home;
using SIS.HTTP.Responses;
using System.Linq;

namespace ChushkaWebApplication.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.IsLoggedIn)
            {
                var products = this.Context.Products
                    .Select(p => new IndexProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description
                    })
                    .ToList();

                var productCollection = new ProductCollectionViewModel
                {
                    Products = products
                };

                return this.View("Home/LoggedIndex", productCollection);
            }

            return this.View();
        }
    }
}
