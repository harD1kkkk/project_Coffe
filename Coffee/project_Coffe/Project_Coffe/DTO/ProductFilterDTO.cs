namespace Project_Coffe.DTO
{
    public class ProductFilterDto
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public bool? SortLowOrHighPrice { get; set; }
    }
}
