namespace Talabat.API.Errors
{
    public class ApiExceptionResponse: ApiResponse
    {
        public string? Details { get; }
        public ApiExceptionResponse(int StatusCode, string? Message = null,string? Details=null):base(StatusCode,Message)
        {
            this.Details = Details;
        }
    }
}
