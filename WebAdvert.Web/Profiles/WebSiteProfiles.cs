using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.ServiceClient;

namespace WebAdvert.Web.Profiles
{
    public class WebSiteProfiles : Profile
    {
        public WebSiteProfiles()
        {
            CreateMap<CreateAdvertViewModel, CreateAdvertModel>().ReverseMap();
        }
    }
}
