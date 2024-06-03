using System.Text.Json.Serialization;

namespace Fina.Core.Responses;

public class Response<TData>
{
      private readonly int _code = Configuration.DefaltStatusCode;

      [JsonConstructor]
      public Response()
            => _code = Configuration.DefaltStatusCode;

      public Response(TData? data,
                                        int code = Configuration.DefaltStatusCode,
                                 string? message = null)
      {
            Data = data;
            _code = code;
            Message = message;
      }

      public TData? Data { get; set; }
      public string? Message { get; set; }

      [JsonIgnore]
      public bool IsSuccess => _code is >= 200 and <= 299;
}
