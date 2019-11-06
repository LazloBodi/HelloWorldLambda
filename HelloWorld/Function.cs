
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.XRay.Recorder.Core;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace HelloWorld
{
    public class Function
    {
        public APIGatewayProxyResponse FunctionHandler(
            APIGatewayProxyRequest request,
            ILambdaContext context)
        {

            LogMessage(context, "Processing request started");
            string name = TraceFunction(GetName, request, "GetName");
            // string name = GetName(request);
            LogMessage(context, $"Processing request started for {name}");
            return new APIGatewayProxyResponse
            {
                Body = $"Hello {name}!",
                StatusCode = 200,
            };
        }

        private static string GetName(APIGatewayProxyRequest request)
        {
            if (request.QueryStringParameters == null) { return string.Empty; }
            if (!request.QueryStringParameters.TryGetValue("name", out string name))
            { return string.Empty; } 
            return name;
        }

        private void LogMessage(ILambdaContext ctx, string msg)
        {
            ctx.Logger.LogLine(
                string.Format("{0}:{1} - {2}",
                    ctx.AwsRequestId,
                    ctx.FunctionName,
                    msg));
        }

        private T TraceFunction<T,K>(Func<K,T> func, K arg, string subSegmentName)
        {
            AWSXRayRecorder.Instance.BeginSubsegment(subSegmentName);
            T result = func(arg);
            AWSXRayRecorder.Instance.EndSubsegment();
            return result;
        }
    }
}
