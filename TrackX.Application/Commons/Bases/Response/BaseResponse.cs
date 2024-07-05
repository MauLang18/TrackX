using FluentValidation.Results;

namespace TrackX.Application.Commons.Bases.Response;

public class BaseResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public int? TotalRecords { get; set; }
    public string? Message { get; set; }
    public IEnumerable<ValidationFailure>? Errors { get; set; }
}