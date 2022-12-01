namespace MyWorkBoard.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public DateTime CreatedDate { get; set; }

        // Nie każdy komentarz musi być zedytowany
        public DateTime? UpdatedDate { get; set; }
    }
}
