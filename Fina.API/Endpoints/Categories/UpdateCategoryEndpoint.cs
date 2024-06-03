using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;
using Fina.API.Common.Api;
using Fina.API;

namespace Fina.Api.Endpoints.Categories;

public class UpdateCategoryEndpoint :IEndpoint
{
      public static void Map(IEndpointRouteBuilder app)
          => app.MapPut("/{id}", HandleAsync)
              .WithName("Categories: Update")
              .WithSummary("Atualiza uma categoria")
              .WithDescription("Atualiza uma categoria")
              .WithOrder(2)
              .Produces<Response<Category?>>();

      private static async Task<IResult> HandleAsync(
          ICategoryHandler handler,
          UpdateCategoryRequest request,
          long id)
      {
            request.UserId = ApiConfiguration.Userid;
            request.Id = id;

            var result = await handler.UpdateAsync(request);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
      }
}