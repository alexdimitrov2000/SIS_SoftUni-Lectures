using ChushkaWebApplication.ViewModels.Orders;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System.Linq;

namespace ChushkaWebApplication.Controllers
{
    public class OrdersController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse All()
        {
            var orders = this.Context.Orders
                .Select(o => new OrderViewModel
                {
                    Id = o.Id,
                    CustomerName = o.Client.FullName,
                    ProductName = o.Product.Name,
                    OrderedOn = o.OrderedOn.ToString("hh:mm dd/MM/yyyy")
                })
                .ToList();

            var orderCollection = new OrderCollectionViewModel
            {
                Orders = orders
            };

            return this.View(orderCollection);
        }
    }
}
