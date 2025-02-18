using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class UserBlock
    {
        [Key]
        public string UserBlockId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public string BlockedUserId { get; set; }
        public AppUser BlockedUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}