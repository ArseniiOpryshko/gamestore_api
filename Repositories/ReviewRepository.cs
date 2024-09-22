using AutoMapper;
using GameStore.Common;
using GameStore.Data;
using GameStore.Dtos.ReviewDtos;
using GameStore.Models.Games;
using GameStore.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly GameStoreContext context;

        public ReviewRepository(GameStoreContext context)
        {
            this.context = context;
        }

        public async Task<OperationResult<int>> AddReview(AddReviewDto reviewDto)
        {
            Game? game = await context.Games
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == reviewDto.GameId);

            if (game == null)
            {
                return OperationResult<int>.FailureResult("Game with such id doesn't exist");
            }

            User? user = await context.Users.FindAsync(reviewDto.UserId);

            if (user == null)
            {
                return OperationResult<int>.FailureResult("User with such id doesn't exist");
            }

            if (!game.Reviews.Any())
            {
                game.Reviews = new List<Review>();
            }

            Review review = new Review()
            {
                IsRecommended = reviewDto.IsRecommended,
                User = user,
                Content = reviewDto.Content
            };
            game.Reviews.Add(review);
            await context.SaveChangesAsync();

            await CalculateReviewStatusForGame(review.GameId);

            return OperationResult<int>.SuccessResult(review.Id);
        }

        public async Task<OperationResult<int>> DeleteReview(int id)
        {
            Review? review = await context.Reviews.FindAsync(id);

            if (review == null)
            {
                return OperationResult<int>.FailureResult("Review with such id doesn't exist");
            }

            context.Reviews.Remove(review);
            await context.SaveChangesAsync();

            await CalculateReviewStatusForGame(review.GameId);

            return OperationResult<int>.SuccessResult(id);
        }

        public async Task<OperationResult<IEnumerable<Review>>> GetReviewsByGameId(int gameId)
        {
            List<Review> reviews = await context.Reviews
                .AsNoTracking()
                .Include(x => x.User)
                .Where(x => x.GameId == gameId)
                .ToListAsync();

            if (reviews == null || !reviews.Any())
            {
                return OperationResult<IEnumerable<Review>>.FailureResult("No reviews were found for this game.");
            }

            return OperationResult<IEnumerable<Review>>.SuccessResult(reviews);
        }

        public async Task<OperationResult<int>> UpdateReview(UpdateReviewDto reviewDto)
        {
            Review? review = await context.Reviews.FindAsync(reviewDto.ReviewId);

            if (review == null)
            {
                return OperationResult<int>.FailureResult("Review with such id doesn't exist");
            }

            review.Content = reviewDto.Content;
            review.IsRecommended = reviewDto.IsRecommended;

            await context.SaveChangesAsync();

            await CalculateReviewStatusForGame(review.GameId);

            return OperationResult<int>.SuccessResult(review.Id);
        }

        public async Task CalculateReviewStatusForGame(int gameId)
        {
            Game? game = await context.Games
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == gameId);

            int totalReviews = game!.Reviews.Count();
            int positiveReviews = game!.Reviews.Where(x => x.IsRecommended).Count();

            double positivePercentage = (double)positiveReviews / totalReviews * 100;

            if (positivePercentage >= 85)
                game.ReviewStatus = ReviewStatus.VeryPositive;
            else if (positivePercentage >= 65)
                game.ReviewStatus = ReviewStatus.Positive;
            else if (positivePercentage >= 45)
                game.ReviewStatus = ReviewStatus.Neutral;
            else if (positivePercentage >= 20)
                game.ReviewStatus = ReviewStatus.Negative;
            else
                game.ReviewStatus = ReviewStatus.VeryNegative;

            await context.SaveChangesAsync();
        }
    }
    public interface IReviewRepository
    {
        Task<OperationResult<IEnumerable<Review>>> GetReviewsByGameId(int gameId);
        Task<OperationResult<int>> AddReview(AddReviewDto reviewDto);
        Task<OperationResult<int>> UpdateReview(UpdateReviewDto reviewDto);
        Task<OperationResult<int>> DeleteReview(int id);
    }
}
