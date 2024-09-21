namespace GameStore.Dtos.ReviewDtos
{
    public class AddReviewDto
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public bool IsRecommended { get; set; }
    }
}
