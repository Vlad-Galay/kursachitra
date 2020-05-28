using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyBlogApp.DAL.Entity;
using MyBlogApp.BLL.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MyBlogApp.DAL.Entity.Infrastructure;
using MyBlogApp.BLL.Exceptions;

namespace MyBlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanysController : ControllerBase
    {
        private ILogger<CompanysController> logger;
        private ICompanyService companyService; 
        public CompanysController(ICompanyService companyService, ILogger<CompanysController> logger)
        {
            this.logger = logger;
            this.companyService = companyService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]Company company)
        {
            logger.LogInformation("Accept POST Company");
            if (company == null)
            {
                return BadRequest(JsonConvert.SerializeObject("invalid company object"));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                companyService.AddCompany(company);
            }
            catch (ServiceException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(JsonConvert.SerializeObject("Server error"));
            }
            
            logger.LogInformation(JsonConvert.SerializeObject("Company was added from POST method"));
            return Ok(company);
        }

        [Authorize]
        [HttpPut]
        public IActionResult EditCompany([FromQuery] int id, [FromBody] Company company)
        {
            if (company == null)
                return BadRequest(JsonConvert.SerializeObject("company is null"));

            if (companyService.GetCompany(id) == null)
                return BadRequest(JsonConvert.SerializeObject("company does not exists"));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                companyService.EditCompany(id, company);
            }
            catch (ServiceException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(JsonConvert.SerializeObject("Server error"));
            }
            return Ok(company);
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteCompany([FromQuery] int id)
        {
            if (companyService.GetCompany(id) == null)
                return BadRequest(JsonConvert.SerializeObject("company does not exists"));

            try
            {
                companyService.RemoveCompany(id);
            }
            catch (ServiceException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(JsonConvert.SerializeObject("Server error"));
            }
            return Ok(id);
        }

        [HttpGet]
        public IActionResult GetCompanys([FromQuery] CompanyQueryParameters companyParameters)
        {
            if (companyParameters == null)
                return BadRequest(JsonConvert.SerializeObject("Null params"));

            if (!companyParameters.ValidYearRange)
            { 
                return BadRequest(JsonConvert.SerializeObject("Max date cannot be less than min date"));
            }

            if (companyParameters.Tags != null)
                companyParameters.Tags = companyParameters.Tags.TrimEnd();

            PagedList<Company> companys = null;
            try
            {
                companys = companyService.GetCompanys(companyParameters);
            }
            catch (ServiceException ex)
            {
                logger.LogError(ex.Message);
                return BadRequest(JsonConvert.SerializeObject("Server error"));
            }

            if (companys == null)
                return Ok(new List<Company>());

            var metadata = new
            {
                companys.TotalCount,
                companys.PageSize,
                companys.CurrentPage,
                companys.TotalPages,
                companys.HasNext,
                companys.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            logger.LogInformation($"Returned {companys.TotalCount} companys from database.");

            return Ok(JsonConvert.SerializeObject(companys));
        }
    }
}