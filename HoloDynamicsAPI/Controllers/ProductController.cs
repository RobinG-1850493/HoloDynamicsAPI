using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HoloDynamicsAPI.Models;
using Microsoft.Xrm.Sdk;
using Newtonsoft.Json;

namespace HoloDynamicsAPI.Controllers
{
    public class ProductController : ApiController
    {
        public OrganizationMan man = new OrganizationMan();
        // GET: api/Product
        public List<Product> Get()
        {
            man.ConnectToCrm();
            List<Product> products = man.getProducts();
            return products;
        }
        // GET: api/Product/{id}
        public List<Account> GetProductById(string id)
        {
            man.ConnectToCrm();
            List<Account> accountList = man.getCustomersByProductId(id);
            return accountList;
        }
    }
}
