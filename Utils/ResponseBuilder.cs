using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TraineeManagementApi.ResponsesBuilder;

public static class ResponseBuilder
{
    public static ActionResult CreateResponse(int status,string message,ModelStateDictionary modelstate)
    {
        var errors = modelstate
            .Where(ms => ms.Value?.Errors.Count > 0)
            .Select(ms => new
            {
                Field = ms.Key,
                Errors = ms.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
            });
        return new ObjectResult(new
        {
            Messages = message,
            Errors = errors
        })
        {
            StatusCode = status
        };
    }
    public static ActionResult CreateResponseSuccess(int status,object? data = null)
    {
        if (status == StatusCodes.Status204NoContent || data == null)
        {
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }
        return new ObjectResult(data)
        {
            StatusCode = status
        };
    }
}