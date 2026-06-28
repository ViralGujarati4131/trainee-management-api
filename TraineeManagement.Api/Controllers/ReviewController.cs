using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Data.ReviewDTO;
using TraineeManagement.Api.ReviewServiceInterface;
using TraineeManagement.Api.ResponsesBuilder;
using TraineeManagement.Api.Data.ConstRoute;
using TraineeManagement.Api.Data.Response;

namespace TraineeManagement.Api.ReviewController;

[ApiController]
[Route(CustomConstRoute.Reviews)]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;
    
    private readonly ILogger<ReviewsController> _logger;

    public ReviewsController(IReviewService reviewService, ILogger<ReviewsController> logger)
    {
        _reviewService = reviewService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> CreateReview([FromBody] ReviewCreateDto createReviewDto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        _logger.LogDebug("Invoking review service to add a new review");

        ReviewResponseDto createdReview = await _reviewService.CreateReviewAsync(createReviewDto);

        _logger.LogInformation("State check: Review creation success. Id: {ReviewId}", createdReview.Id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataInsertedSuccess,
            createdReview
        );
    }

    [HttpGet]
    public async Task<ActionResult> GetReviews()
    {
        _logger.LogDebug("Invoking review service to fetch all reviews");

        IEnumerable<ReviewResponseDto> reviews = await _reviewService.GetReviewsAsync();

        _logger.LogInformation("State check: Bulk fetch reviews success.");
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            reviews
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetReviewById(int id)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Request failed validation. Invalid model state.");
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.UnprocessableEntity);
        }
        if(id < 1)
        {
            _logger.LogWarning("Request failed validation. Invalid ID range. Id: {ReviewId}", id);
            return CustomResponseBuilder.CreateValidationErrorResponse(CustomResponse.BadRequest);
        }
        _logger.LogDebug("Invoking review service to retrieve review for ReviewId: {ReviewId}", id);

        ReviewResponseDto review = await _reviewService.GetReviewByIdAsync(id);
        
        _logger.LogInformation("State check: Fetch review by ID success. Id: {ReviewId}", id);
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.DataRetrivedSuccess,
            review
        );
    }
}