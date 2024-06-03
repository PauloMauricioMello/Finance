﻿using Fina.API.Common.Api;
using Fina.API;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Categories;

public class GetCategoryByIdEndpoint :IEndpoint
{
      public static void Map(IEndpointRouteBuilder app)
          => app.MapGet("/{id}", HandleAsync)
              .WithName("Categories: Get By Id")
              .WithSummary("Recupera uma categoria")
              .WithDescription("Recupera uma categoria")
              .WithOrder(4)
              .Produces<Response<Category?>>();

      private static async Task<IResult> HandleAsync(
          ICategoryHandler handler,
          long id)
      {
            var request = new GetCategoryByIdRequest
            {
                  UserId = ApiConfiguration.Userid,
                  Id = id
            };

            var result = await handler.GetByIdAsync(request);
            return result.IsSuccess
                ? TypedResults.Ok(result)
                : TypedResults.BadRequest(result);
      }
}