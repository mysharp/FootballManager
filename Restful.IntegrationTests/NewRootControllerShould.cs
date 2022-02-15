using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Restful.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Restful.IntegrationTests
{
    [TestClass]
    public class NewRootControllerShould
    {
        private TestApplication _app;    

        [TestInitialize]
        public void Setup()
        {
            _app = new TestApplication(); 
        }

        [TestCleanup]
        public void Cleanup()
        {
            _app.Dispose();
        }

        [TestMethod]
        public async Task NoContentWhenOtherMediaType()
        {
            var response = await _app.HttpClient.GetAsync("/api");

            response.EnsureSuccessStatusCode();

            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(string.IsNullOrEmpty(content));
        }

        [TestMethod]
        public async Task ReturnLinkResourceListWhenSpecificMediaType()
        {
            _app.HttpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.johnson.hateoas+json");

            var response = await _app.HttpClient.GetAsync("/api");

            response.EnsureSuccessStatusCode();

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<LinkResource>>(content);
            Assert.IsTrue(list.Count > 0);

            var self = list.Single(x => x.Group == "self");
            Assert.IsNotNull(self);
        }
    }
}
