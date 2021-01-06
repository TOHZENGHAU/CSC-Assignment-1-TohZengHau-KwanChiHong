using AssTask2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AssTask2.Controllers
{
    public class ProductsV2Controller : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();
        
        [HttpGet]
        [Route("api/v2/products")]
        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }

        [HttpGet]
        [Route("api/v2/products/{id}")]
        public Product GetProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        [HttpGet]
        [Route("api/v2/getProductsbyCategory/{category}")]
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        [HttpPost]
        [Route("api/v2/addProducts")]
        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
            //response.Headers.Location = new Uri(uri);
            return response;
        }

        [HttpPut]
        [Route("api/v2/updateProducts/{id}")]
        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("api/v2/deleteProducts/{id}")]
        public void DeleteProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            repository.Remove(id);
        }
    }
}
