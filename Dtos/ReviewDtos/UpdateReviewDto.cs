namespace GameStore.Dtos.ReviewDtos
{
    public class UpdateReviewDto
    {
        public int ReviewId { get; set; }
        public string Content { get; set; }
        public bool IsRecommended { get; set; }
    }
}
