using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Reviews.DTOs;
using TraineeManagementApi.Reviews.ServiceInterface;

namespace TraineeManagementApi.Reviews.Cotroller;

[ApiController]
[Route("api/reviews")]
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
    public async Task<ActionResult<ReviewResponseDto>> CreateReview([FromBody] ReviewCreateDto createReviewDto)
    {
        _logger.LogDebug("Invoking review service to add a new review");
        ReviewResponseDto createdReview = await _reviewService.CreateReviewAsync(createReviewDto);
        return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.Id }, createdReview);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReviewResponseDto>>> GetReviews()
    {
        _logger.LogDebug("Invoking review service to fetch all reviews");
        IEnumerable<ReviewResponseDto> reviews = await _reviewService.GetReviewsAsync();
        return Ok(reviews);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewResponseDto>> GetReviewById(int id)
    {
        _logger.LogDebug("Invoking review service to retrieve review for ReviewId: {ReviewId}", id);
        ReviewResponseDto review = await _reviewService.GetReviewByIdAsync(id);
        return Ok(review);
    }
}