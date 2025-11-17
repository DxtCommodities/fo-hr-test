namespace DataIntegration.Queries;

public class AggregationResult
{
    public required String Country { get; set; }

    public DateOnly Month { get; set; }

    public decimal AveragePriceOfProducts { get; set; }

    public int TotalNumberDistinctOfProducts { get; set; }

    public decimal TotalRevenew { get; set; }
}
