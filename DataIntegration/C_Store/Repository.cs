using DataIntegration.Queries;
using DataIntegrationExercise.Models;

namespace DataIntegration.Db;

public class Repository
{
    private readonly Settings settings;

    public Repository(Settings settings)
    {
        this.settings = settings;
    }
    public Task Save(Product product)
    {
        // save to database
        throw new NotImplementedException();
    }

    public Task Save(Product[] products)
    {
        // save to database
        throw new NotImplementedException();
    }

    public List<AggregationResult> GetAggregations(Product[] products, DateOnly fromMonth, DateOnly toMonthExcluded)
    {
        throw new NotImplementedException();
    }
}
