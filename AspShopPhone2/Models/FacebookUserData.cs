using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspShopPhone2.Models
{
    public class FacebookUserData
    {
        public string Link { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public dynamic AgeRange { get; set; }
    }
}