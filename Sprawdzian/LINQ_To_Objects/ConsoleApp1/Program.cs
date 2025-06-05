namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // rezerwacje od 13 do nastepnego dnia 11
            // zapis: 12/31/2020-01/01/2021 MM/dd/yyyy rezerwacje rozdzielone ','

            var booking = "12/31/2020-02/01/2021";
            var free = "11/15/2020-11/17/2020, 01/06/2021-01/07/2021, 01/08/2021-01/10/2021, 01/12/2021-02/01/2021, 01/31/2020-02/01/2020";

            var result = HotelRoom_GetWhenRoomIsFree(booking, free);
            Console.WriteLine(result);

        }

        public static string HotelRoom_GetWhenRoomIsFree(string dateSpan, string datesWhenRoomIsFree)
        {
            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var spanParts = dateSpan.Split('-', StringSplitOptions.RemoveEmptyEntries);
            DateTime spanStart = DateTime.ParseExact(spanParts[0], "MM/dd/yyyy", culture);
            DateTime spanEnd = DateTime.ParseExact(spanParts[1], "MM/dd/yyyy", culture);

            var freeRanges = datesWhenRoomIsFree
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Select(x =>
                {
                    var parts = x.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    return (From: DateTime.ParseExact(parts[0], "MM/dd/yyyy", culture),
                            To: DateTime.ParseExact(parts[1], "MM/dd/yyyy", culture));
                })
                .ToList();
            var clippedFreeRanges = freeRanges
                .Select(r => (
                    From: r.From < spanStart ? spanStart : r.From,
                    To: r.To > spanEnd ? spanEnd : r.To))
                .Where(r => r.From <= r.To)
                .ToList();

            return string.Join(", ",
                clippedFreeRanges.Select(r => $"{r.From:MM/dd/yyyy}-{r.To:MM/dd/yyyy}"));
        }


    }
}
