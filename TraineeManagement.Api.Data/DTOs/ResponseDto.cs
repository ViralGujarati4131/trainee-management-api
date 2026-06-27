namespace TraineeManagement.Api.Data.ResponseDTO;

public record InterServiceCommunicationResponse<T>(
    string Code,
    string Message,
    T? Data
);