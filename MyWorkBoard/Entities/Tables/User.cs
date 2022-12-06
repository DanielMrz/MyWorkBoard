namespace MyWorkBoard.Entities.Tables
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        // Relacje
        public Address Address { get; set; }
        // Każdy twórca może mieć wiele workitemów
        public List<WorkItem> WorkItems { get; set; } = new List<WorkItem>();
        // Każdy twórca może zamieszczać wiele komentarzy
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
