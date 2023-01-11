using MyWorkBoard.Entities.Tables;

namespace MyWorkBoard.Entities.Model.Dto
{
    public class PagedResult<T>
    {
        // Rezultat - Lista klasy User 
        public List<T> Items { get; set; }

        // Całkowita liczba stron
        public int TotalPages { get; set; }

        // Zakres wierszy
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }

        // Liczba wszystkich elementów, które spełniają kryteria wyszukiwania
        public int TotalItemsCount { get; set; }

        // Te wszystkie informacje będą zależne od informacji, które poda użytkownik
        // Publiczny konstruktor który te informacje dostarczy
        public PagedResult(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalCount;
            ItemsFrom = pageSize * (pageNumber - 1) + 1; // +1 ponieważ rozpoczyna od następnego elementu
            ItemsTo = ItemsFrom + (pageSize - 1);
            // Trzeba przewidzieć że jak będzie 12 elementów a użytkownik chciałby aby się wyswietlaly po 5 to wynik zwykłego dzielenia wyjdzie 2,
            // a na 2 stronach nie zobaczy wszystkich elementów.
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

    }
}
