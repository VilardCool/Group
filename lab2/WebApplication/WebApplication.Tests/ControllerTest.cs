using Microsoft.AspNetCore.Mvc;
using WebApplication.Controllers;
using Xunit;

namespace WebApplication.Tests
{
    public class ControllerTest
    {
        [Fact]
        public void IndexViewDataMessage()
        {
            TestController controller = new TestController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.Equal("Hello!", result?.ViewData["Message"]);
        }

        [Fact]
        public void IndexViewResultNotNull()
        {
            TestController controller = new TestController();
            ViewResult result = controller.Index() as ViewResult;

            Assert.NotNull(result);
        }
    }
}
