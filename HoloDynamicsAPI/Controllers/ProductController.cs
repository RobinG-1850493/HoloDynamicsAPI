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
            List<Product> products = new List<Product>();
            string user = "";
            string pw = "";

            if (Request.Headers.Contains("AuthorizationUser"))
            {
                user = Request.Headers.GetValues("AuthorizationUser").First();
                pw = Request.Headers.GetValues("AuthorizationPass").First();
            }

            if (user != null && pw != null)
            {
                man.ConnectToCrm(user, pw);
                products = man.getProducts();
            }

            return products;
        }
        // GET: api/Product/{id}
        public List<Account> GetProductById(string id)
        {
            List<Account> accountList = new List<Account>();
            string user = "";
            string pw = "";

            if (Request.Headers.Contains("AuthorizationUser"))
            {
                user = Request.Headers.GetValues("AuthorizationUser").First();
                pw = Request.Headers.GetValues("AuthorizationPass").First();
            }

            if (user != null && pw != null)
            {
                man.ConnectToCrm(user, pw);
                accountList = man.getCustomersByProductId(id);
            }

            return accountList;
        }
    }
}
