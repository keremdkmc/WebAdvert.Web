using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvert.Web.ServiceClient
{
    public class ConfirmAdvertRequest
    {
        public string Id { get; set; }
        public AdvertStatus Status { get; set; }
    }
}
