using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TraineeManagementApi.Constants;

namespace TraineeManagementApi.Utils.ResponsesBuilder;

public static class ResponseBuilder
{
    public static ActionResult CreateValidationErrorResponse()
    {
        ApiResponseDescriptor descriptor = AppConstants.ApiResponse.ValidationError;

        return new ObjectResult(new
        {
            Code = descriptor.CustomCode,
            Message = descriptor.Message
        })
        {
            StatusCode = descriptor.HttpStatusCode
        };
    }
    public static ActionResult CreateSuccessResponse(ApiResponseDescriptor descriptor, object? data = null)
    {
        if (descriptor.HttpStatusCode == StatusCodes.Status204NoContent)
        {
            return new StatusCodeResult(StatusCodes.Status204NoContent);
        }

        return new ObjectResult(new
        {
            Code = descriptor.CustomCode,
            Message = descriptor.Message,
            Data = data
        })
        {
            StatusCode = descriptor.HttpStatusCode
        };
    }
}