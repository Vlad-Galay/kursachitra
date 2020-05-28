using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlogApp.DAL.Entity
{
    public class CompanyTag
    {

        [JsonIgnore]
        public Company Company { get; set; }
        public int CompanyId { get; set; }

        [JsonIgnore]
        public Tag Tag { get; set; }
  
        public int TagId { get; set; }

    }
}
