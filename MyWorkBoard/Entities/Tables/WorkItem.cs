using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWorkBoard.Entities.Tables
{
    public class Epic : WorkItem
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class Issue : WorkItem
    {
        public decimal Efford { get; set; }
    }

    public class Task : WorkItem
    {
        public string Activity { get; set; }
        public decimal RemainingWork { get; set; }
    }

   
    public abstract class WorkItem
    {
        // Opis: właściwości tej klasy będą reprezentować typy i właściowści tabeli workitems
        public int Id { get; set; } // Klucz główny, bez adnotacji bo domyślnie entity uzywa id

        // Relacje State
        public State State { get; set; } // Wydzielilismy stan do nowo utworzonej encji
        public int StateId { get; set; } // Referencje Klucza obcego

        // [Column(TypeName = "varchar(200)")] // zmiana typu
        public string Area { get; set; }
        public string IterationPath { get; set; }
        public int Priority { get; set; }



        // Relacje Comment
        // Kazdy workitem będzie miał relacje do wielu komentarzy
        public List<Comment> Comments { get; set; } = new List<Comment>();

        // Relacje User
        // Workitem możem ieć tylko 1 twórcę
        public User Author { get; set; }
        public Guid AuthorId { get; set; }

        // Relacje Tag
        // Każdy workitem może mieć wiele tagów i teraz można odnieśc się do powiązanych encji. Sposób < .net5
        // public List<WorkItemTag> WorkItemTags { get; set; } = new List<WorkItemTag>();
        public List<Tag> Tags { get; set; }

    }
}
