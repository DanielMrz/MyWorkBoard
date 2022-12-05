namespace MyWorkBoard.Entities.Tables
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagValue { get; set; }

        // Relacja wiele do wielu
        // Każdy workitem moze mieć wiele tagów
        // Każdy Tag może byc przypisany do wielu woritemów
        // W sql trzeba uzyc tabeli łączącej. W .net < 5 też tak trzeba (zrobić encje łączącą)
        // W entity od .net5

        // public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
        public List<WorkItem> WorkItems { get; set; }
    }
}
