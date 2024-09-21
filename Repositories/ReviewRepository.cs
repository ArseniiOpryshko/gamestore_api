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

            return OperationResult<int>.SuccessResult(review.Id);
        }

        public async Task<OperationResult<int>> DeleteReview(int id)
        {
            Review? review = await context.Reviews.FindAsync(id);

            if(review == null)
            {
                return OperationResult<int>.FailureResult("Review with such id doesn't exist"); 
            }

            context.Reviews.Remove(review);
            await context.SaveChangesAsync();

            return OperationResult<int>.SuccessResult(id);
        }

        public async Task<OperationResult<IEnumerable<Review>>> GetReviewsByGameId(int gameId)
        {
            List<Review> reviews = await context.Reviews
                .AsNoTracking()
                .Where(x=>x.GameId == gameId)
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

            return OperationResult<int>.SuccessResult(review.Id);
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
