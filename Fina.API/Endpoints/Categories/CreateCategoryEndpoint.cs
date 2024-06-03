using Fina.API.Common.Api;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.API.Endpoints.Categories;

public class CreateCategoryEndpoint :IEndpoint
{
      public static void Map(IEndpointRouteBuilder app)
            => app.MapPost("/", HandleAsync)
            .WithName("Categories: Create")
            .WithSummary("Crear uma nova categoria")
            .WithOrder(1)
            .Produces<Response<Category?>>();

      private static async Task<IResult> HandleAsync(
            ICategoryHandler handler,
            CreateCategoryRequest request)
      {
            request.UserId = ApiConfiguration.Userid;
            var response = await handler.CreateAsync(request);
            return response.IsSuccess
                  ? TypedResults.Created($"vi/categories/{response.Data?.Id}")
                  : TypedResults.BadRequest(response);
      }
}
