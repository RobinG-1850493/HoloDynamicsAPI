using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoloDynamicsAPI.Models
{
    public class Product
    {
        public string productNaam { get; set; }
        public string productLogo { get; set; }
        public string productId { get; set; }
        public EntityReference marketinglist { get; set; }
    }
}