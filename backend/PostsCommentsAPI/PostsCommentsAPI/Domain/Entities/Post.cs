using PostsCommentsAPI.Domain.Interfaces;

namespace PostsCommentsAPI.Domain.Entities
{
    public class Post : IAuditable
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public List<Comment> Comments { get; set; } = [];
    }
}