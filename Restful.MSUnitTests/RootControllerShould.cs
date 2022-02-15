using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Restful.Controllers;
using Restful.ViewModels;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Restful.UnitTests
{
    [TestClass]
    public class RootControllerShould
    {
        private readonly RootController _sut;

        public RootControllerShould()
        {
            var mockUrlHelper = new Mock<IUrlHelper>();

            // System Under Test(±ª≤‚ ‘œµÕ≥)
            _sut = new RootController(mockUrlHelper.Object);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("application/json")]
        [DataRow("application/vnd.johnson.hateoas+json")]
        public void ReturnInActionResult(string media)
        {
            var result = _sut.GetRoot(media);

            Assert.IsTrue(typeof(IActionResult).IsAssignableFrom(result.GetType()));  
        }

        [TestMethod]
        public void ReturnNoContentWhenNotHateoasMediaType()
        {
            var media = "application/json";

            var result = _sut.GetRoot(media);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void ReturnLinksInOkWhenCompanyHateoasMediaType()
        {
            var media = "application/vnd.johnson.hateoas+json";

            var result = _sut.GetRoot(media);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var objectResult = result as OkObjectResult;

            Assert.IsInstanceOfType(objectResult?.Value, typeof(List<LinkResource>));
        }
    }
}
