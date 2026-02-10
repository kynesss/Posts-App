using PostsCommentsAPI.Domain.Interfaces;

namespace PostsCommentsAPI.Domain.Entities
{
    public class Comment : IAuditable
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }
}