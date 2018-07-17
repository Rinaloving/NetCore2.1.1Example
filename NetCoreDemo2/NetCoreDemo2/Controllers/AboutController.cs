using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreDemo2.Controllers
{
    [Route("[controller]")]
    public class AboutController 
    {
        
       public AboutController()
        {

        }

        [Route("")]
        public string Phone()
        {
            return "+10086";
        }

        [Route("[action]")]
        public string Country()
        {
            return "中国";
        }
    }
}