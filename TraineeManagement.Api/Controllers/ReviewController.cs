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
            return CustomResponseBuilder.CreateValidationErrorResponse();
        }
        _logger.LogDebug("Invoking review service to add a new review");

        ReviewResponseDto createdReview = await _reviewService.CreateReviewAsync(createReviewDto);

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Created,
            createdReview
        );
    }

    [HttpGet]
    public async Task<ActionResult> GetReviews()
    {
        _logger.LogDebug("Invoking review service to fetch all reviews");

        IEnumerable<ReviewResponseDto> reviews = await _reviewService.GetReviewsAsync();

        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Success,
            reviews
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetReviewById(int id)
    {
        if (!ModelState.IsValid || id < 1)
        {
            return CustomResponseBuilder.CreateValidationErrorResponse();
        }
        _logger.LogDebug("Invoking review service to retrieve review for ReviewId: {ReviewId}", id);

        ReviewResponseDto review = await _reviewService.GetReviewByIdAsync(id);
        
        return CustomResponseBuilder.CreateSuccessResponse(
            CustomResponse.Success,
            review
        );
    }
}