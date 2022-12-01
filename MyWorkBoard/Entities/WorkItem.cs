namespace MyWorkBoard.Entities
{
    // Właściwości tej klasy będą reprezentować typy i właściowści tabeli workitems
    public class WorkItem
    {
        public int Id { get; set; } // Klucz główny, bez adnotacji bo domyślnie entity uzywa id
        public string State { get; set; }
        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }

        // Epic
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Issue
        public decimal Efford { get; set; }

        // Task
        public string Activity { get; set; }
        public decimal RemainingWork { get; set; }

        // Jakiego typu jest to workitem?
        public string Type { get; set; }
    }
}
