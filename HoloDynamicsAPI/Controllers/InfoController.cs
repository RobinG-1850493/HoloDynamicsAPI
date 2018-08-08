using HoloDynamicsAPI.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HoloDynamicsAPI.Controllers
{
    public class InfoController : ApiController
    {
        public OrganizationMan man = new OrganizationMan();

        [HttpGet]
        [Route("api/Info/{productId}/{accountId}")]
        public List<Info> GetInfoByProductAndAccountId(string productId, string accountId)
        {
            man.ConnectToCrm();
            List<Info> infoList = man.getInfoByProductAndAccountId(productId, accountId);
            return infoList;
        }
    }
}
