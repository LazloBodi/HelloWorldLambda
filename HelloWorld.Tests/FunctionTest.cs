using System.Collections.Generic;
using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Core.Strategies;

namespace HelloWorld.Tests
{
    public class FunctionTest
    {
        public FunctionTest()
        {
            AWSXRayRecorder.Instance.ContextMissingStrategy = ContextMissingStrategy.LOG_ERROR;
        }

        [Fact]
        public void HelloNameTest()
        {
            var function = new Function();
            var context = new TestLambdaContext();
            APIGatewayProxyRequest req = new APIGatewayProxyRequest
            {
                QueryStringParameters = new Dictionary<string, string> { {"name", "Laci" } }
            };

            var resp = function.FunctionHandler(req, context);

            Assert.Equal("Hello Laci!", resp.Body);
            Assert.Equal(200, resp.StatusCode);
        }
    }
}
