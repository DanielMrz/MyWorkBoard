namespace MyWorkBoard.Entities.Tables
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }

        // Relacja z User
        public User Author { get; set; }
        public Guid AuthorId { get; set; }

        public DateTime CreatedDate { get; set; }

        // Nie każdy komentarz musi być zedytowany
        public DateTime? UpdatedDate { get; set; }

        // Relacja
        public WorkItem WorkItem { get; set; }
        public int WorkItemId { get; set; }
    }
}
