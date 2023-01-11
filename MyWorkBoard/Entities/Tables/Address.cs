using Microsoft.EntityFrameworkCore;

namespace MyWorkBoard.Entities.Tables
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }

        // Relacje 1-1 gdzie kluczem głównym jest Id w tabeli user
        // Kluczem obcym jest id w address
        public virtual User User { get; set; }
        public Guid UserId { get; set; }

        public Coordinate Coordinate { get; set; }
    }

    // model który będzie zawarty w istniejącej tabeli
    [Owned]
    public class Coordinate
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }
}
