using HoloDynamicsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HoloDynamicsAPI.Controllers
{
    public class DocumentController : ApiController
    {
        public OrganizationMan man = new OrganizationMan();

        [HttpGet]
        [Route("api/document/{infoId}")]
        public Document GetPagesByInfoId(string infoId)
        {
            Document infoList = new Document();
            string user = "r";
            string pw = "";

            if (Request.Headers.Contains("AuthorizationUser"))
            {
                user = Request.Headers.GetValues("AuthorizationUser").First();
                pw = Request.Headers.GetValues("AuthorizationPass").First();
            }

            if (user != null && pw != null)
            {
                man.ConnectToCrm(user, pw);
                infoList = man.getDocumentByInfoId(infoId);
            }

            return infoList;
        }
    }
}
