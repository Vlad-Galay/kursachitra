using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MyBlogApp.DAL.Entity
{
    public class Company
    {
        public int Id { get; set; }
        public DateTime PublishTime { get; set; }

        [MaxLength(100, ErrorMessage = "Title of the company must be less than 100 symbs")]
        [Required (ErrorMessage = "Company must contain title")]
        public String Title { get; set; }
        public String Description { get; set; }
        public String PicsUrl { get; set; }

        [MaxLength(1000, ErrorMessage = "Company content length must be less than 1000 symbs")]
        [Required (ErrorMessage = "Company must contain content")]
        public String Content { get; set; }
        public List<CompanyTag> CompanyTags { get; set; }

        [Required(ErrorMessage = "Company must be part of some category")]
        public Category Category { get; set; }
    }
}
