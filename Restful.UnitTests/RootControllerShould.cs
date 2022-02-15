using Microsoft.AspNetCore.Mvc;
using Moq;
using Restful.Controllers;
using Restful.ViewModels;
using System;
using System.Collections.Generic;
using Xunit;

namespace Restful.UnitTests
{
    public class RootControllerShould
    {
        private readonly RootController _sut;

        public RootControllerShould()
        {
            var mockUrlHelper = new Mock<IUrlHelper>();

            // System Under Test(±ª≤‚ ‘œµÕ≥)
            _sut = new RootController(mockUrlHelper.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("application/json")]
        [InlineData("application/vnd.johnson.hateoas+json")]
        public void ReturnInActionResult(string media)
        {
            var result = _sut.GetRoot(media);

            Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void ReturnNoContentWhenNotHateoasMediaType()
        {
            var media = "application/json";

            var result = _sut.GetRoot(media);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void ReturnLinksInOkWhenCompanyHateoasMediaType()
        {
            var media = "application/vnd.johnson.hateoas+json";

            var result = _sut.GetRoot(media);

            Assert.IsType<OkObjectResult>(result);

            var objectResult = result as OkObjectResult;

            Assert.IsType<List<LinkResource>>(objectResult?.Value);
        }
    }
}
