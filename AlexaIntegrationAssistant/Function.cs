
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaIntegrationAssistant
{
    public class Function
    {
        private static HttpClient _httpClient;
        public const string INVOCATION_NAME = "ai";

        public Function()
        {
            _httpClient = new HttpClient();
        }
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {

            var requestType = input.GetRequestType();
            if (requestType == typeof(IntentRequest))
            {
                var intentRequest = input.Request as IntentRequest;
                var numberRequested = intentRequest?.Intent?.Slots["number"].Value;

                if (numberRequested == null)
                {
                    context.Logger.LogLine("Number was not understood");
                    return MakeSkillResponse("I'm sorry, your reponse was not a number", false);
                }

                return MakeSkillResponse(
                    $"This is test number {numberRequested}",
                    true);
            }
            else
            {
                return MakeSkillResponse(
                    $"I don't know how to handle this intent. Please say something like Alexa, ask {INVOCATION_NAME} and a number.",
                    true);
            }
        }

        private SkillResponse MakeSkillResponse(string outputSpeech,
            bool shouldEndSession,
            string repromptText = "Just say, something stupid.")
        {
            var response = new ResponseBody
            {
                ShouldEndSession = shouldEndSession,
                OutputSpeech = new PlainTextOutputSpeech { Text = outputSpeech }
            };

            if (repromptText != null)
            {
                response.Reprompt = new Reprompt() { OutputSpeech = new PlainTextOutputSpeech() { Text = repromptText } };
            }

            var skillResponse = new SkillResponse
            {
                Response = response,
                Version = "1.54"
            };
            return skillResponse;
        }
    }
}
