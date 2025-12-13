using Models.Models;

namespace Models.Util;

public class SearchBoxResult
{
    public IEnumerable<Box>? Boxes { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int BoxesPerPage { get; set; }
    public int TotalBoxes { get; set; }
}