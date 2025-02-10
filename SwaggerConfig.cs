using System.Web.Http;
using WebActivatorEx;
using WebAPI;
using Swashbuckle.Application;
using System.Configuration;
using Swashbuckle.Swagger;
using System.Web.Http.Description;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WebAPI
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var config = GlobalConfiguration.Configuration;

            config.EnableSwagger(c =>
                  {
                      c.SingleApiVersion("v1", "WebAPI");
                      c.OperationFilter<AddBodyToGetOperationFilter>(); // Move this line to EnableSwagger
                      c.OperationFilter<AddUrlToDescriptionFilter>();
                  })
                  .EnableSwaggerUi(c =>
                  {
                      c.SupportedSubmitMethods(); // This disables "Try it out" button


                  });

            // Redirect root URL to Swagger UI
            config.Routes.MapHttpRoute(
                name: "SwaggerUI",
                routeTemplate: "",
                defaults: null,
                constraints: null,
                handler: new RedirectHandler(
                    message => message.RequestUri.GetLeftPart(System.UriPartial.Authority) + "/swagger", ""
                   )
            );

        }
    }

    //Add the filter inside SwaggerConfig.cs
    public class AddBodyToGetOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (apiDescription.HttpMethod.Method == "GET" && operation.parameters != null) // Change 'Parameters' to 'parameters'
            {
                operation.parameters.Clear(); // Change 'Parameters' to 'parameters'

                operation.parameters.Add(new Parameter // Change 'Parameters' to 'parameters'
                {
                    name = "body", // Change 'Name' to 'name'
                    @in = "body", // Change 'In' to '@in'
                    description = "Request body", // Change 'Description' to 'description'
                    required = true, // Change 'Required' to 'required'
                    type = "object", // Change 'Type' to 'type'
                    schema = schemaRegistry.GetOrRegister(apiDescription.ParameterDescriptions[0].ParameterDescriptor.ParameterType)
                });
            }
        }
    }

    //public class AddUrlToDescriptionFilter : IOperationFilter
    //{
    //    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    //    {
    //        string baseUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(System.UriPartial.Authority);
    //        string fullUrl = $"{baseUrl}/{apiDescription.RelativePath}";

    //        // Set the description to only show the Request URL
    //        operation.summary = $"**Request URL:**```\n{fullUrl}\n```";
    //    }
    //}

    public class AddUrlToDescriptionFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            string baseUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(System.UriPartial.Authority);
            string fullUrl = $"{baseUrl}/{apiDescription.RelativePath}";

            // Set the description as plain text for easy copying
            operation.summary = $"Request URL: \n\n `{fullUrl}`";
        }
    }
}
