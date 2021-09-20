using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaskManager.Common.Validation;

namespace TaskManager.Tests.Integration
{
    public class IntegrationApiTestBase : IntegrationTestBase
    {
        protected ByteArrayContent CreateContent(object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var buffer = Encoding.UTF8.GetBytes(json);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
        protected ValidationResponse CreateExpectedResponse(string message, string error)
        {
            return new ValidationResponse
            {
                Message = message,
                Errors = new List<ValidationError> { new(error) }
            };
        }

        protected async Task<HttpResponseMessage> SendPostRequest(string path, ByteArrayContent content)
        {
            var client = Server.CreateClient();
            var response = await client.PostAsync(path, content);
            return response;
        }

        protected async Task<HttpResponseMessage> SendGetRequest(string path)
        {
            var client = Server.CreateClient();
            var response = await client.GetAsync(path);
            return response;
        }

        protected async Task<ValidationResponse> GetJsonObject(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ValidationResponse>(json);
            return result;
        }

        protected async Task<JArray> GetJArray(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<JArray>(json);
            return result;
        }
    }
}