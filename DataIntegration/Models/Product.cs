namespace DataIntegrationExercise.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public string? Category { get; set; }
    public string? Country { get; set; }
    public decimal? Price { get; set; } // if not specified, considered as 0

    public DateOnly OnSaleFromMonth { get; set; } // first day of availability of the product
    public DateOnly? WithdrawnFromSaleInMonth { get; set; } // day when the product is withdrawn from sale. If null, the product is still on sale

    public required List<Sale> Sales { get; set; }
}
