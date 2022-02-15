using Microsoft.AspNetCore.Mvc;
using Restful.ViewModels;
using System.Collections.Generic;

namespace Restful.Controllers
{
    [Route("api")]
    public class RootController : ControllerBase
    {
        private readonly IUrlHelper _urlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string media)
        {
            if (string.Equals(media, "application/vnd.johnson.hateoas+json"))
            {
                var links = new List<LinkResource>
                {
                    new LinkResource(_urlHelper.Link("GetRoot", null),"self","GET"),
                    new LinkResource(_urlHelper.Link("GetCountries", null),"countries","GET"),
                    new LinkResource(_urlHelper.Link("CreateCountry", null),"create_country","POST"),
                };

                return Ok(links);
            }

            return NoContent();
        }
    }
}
