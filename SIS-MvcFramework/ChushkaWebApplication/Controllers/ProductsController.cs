using ChushkaWebApplication.Models;
using ChushkaWebApplication.Models.Enums;
using ChushkaWebApplication.ViewModels.Products;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Linq;

namespace ChushkaWebApplication.Controllers
{
    public class ProductsController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [HttpPost]
        public IHttpResponse Create(ProductCreateInputModel model)
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Type = Enum.Parse<ProductType>(model.Type)
            };

            this.Context.Products.Add(product);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestErrorWithView(e.Message);
            }

            return this.Redirect($"/Products/Details?id={product.Id}");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var productExists = this.Context.Products
                .Any(p => p.Id == id);

            if (!productExists)
            {
                return this.BadRequestErrorWithView("Invalid product id");
            }

            var productViewModel = this.Context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Type = p.Type.ToString()
                })
                .First();

            return this.View(productViewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(int id)
        {
            var productExists = this.Context.Products
                   .Any(p => p.Id == id);

            if (!productExists)
            {
                return this.BadRequestErrorWithView("Invalid product id");
            }

            var productViewModel = this.Context.Products
                .Select(p => new ProductEditViewModel
                {
                    Description = p.Description,
                    Name = p.Name,
                    Price = p.Price,
                    Type = p.Type.ToString(),
                    Id = p.Id
                })
                .First(p => p.Id == id);

            return this.View(productViewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Edit(ProductEditViewModel model)
        {
            var product = this.Context.Products.FirstOrDefault(p => p.Id == model.Id);

            if (product == null)
            {
                return this.BadRequestErrorWithView("Invalid product id");
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Type = Enum.Parse<ProductType>(model.Type);

            this.Context.SaveChanges();

            return this.Redirect("/Products/Details?id=" + product.Id);
        }

        [Authorize]
        public IHttpResponse Order(int id)
        {
            var clientId = this.Context.Users.FirstOrDefault(u => u.Username == this.User.Username).Id;

            var order = new Order
            {
                OrderedOn = DateTime.UtcNow,
                ProductId = id,
                ClientId = clientId
            };

            this.Context.Orders.Add(order);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestErrorWithView(e.Message);
            }

            return this.Redirect("/Orders/All");
        }

        [Authorize("Admin")]
        public IHttpResponse Delete(int id)
        {
            var productExists = this.Context.Products
                      .Any(p => p.Id == id);

            if (!productExists)
            {
                return this.BadRequestError("Invalid product id");
            }

            var productViewModel = this.Context.Products
                .Select(p => new ProductEditViewModel
                {
                    Description = p.Description,
                    Name = p.Name,
                    Price = p.Price,
                    Type = p.Type.ToString(),
                    Id = p.Id
                })
                .First(p => p.Id == id);

            return this.View(productViewModel);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse DoDelete(int id)
        {
            var product = this.Context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return this.BadRequestErrorWithView("Invalid product id");
            }

            try
            {
                this.Context.Products.Remove(product);
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestError(e.Message);
            }

            return this.Redirect("/");
        }
    }
}
