namespace MyWorkBoard.Entities.Tables.Linktable
{
    // Klasa łącząca workitem z tagami
    public class WorkItemTag
    {
        public WorkItem WorkItem { get; set; }
        public int WorkItemId { get; set; }

        public Tag Tag { get; set; }
        public int TagId { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
