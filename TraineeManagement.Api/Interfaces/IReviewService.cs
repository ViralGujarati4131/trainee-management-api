using TraineeManagement.Api.Data.ReviewDTO;

namespace TraineeManagement.Api.ReviewServiceInterface;

public interface IReviewService
{
    Task<ReviewResponseDto> CreateReviewAsync(ReviewCreateDto createReviewDto);

    Task<IEnumerable<ReviewResponseDto>> GetReviewsAsync();

    Task<ReviewResponseDto> GetReviewByIdAsync(int id);
}