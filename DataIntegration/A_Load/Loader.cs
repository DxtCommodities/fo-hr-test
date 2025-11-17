namespace DataIntegration.A_Load;

public class Loader
{
    public Loader(Settings settings)
    {
    }

    public async Task<LoadedData> LoadDataAsync()
    {
        var json = new MemoryStream();
        var html = new MemoryStream();
        var csv = new MemoryStream();

        // TODO: load data from external sources
        // json from file
        // html from file
        // csv from http

        return new LoadedData(json, html, csv);
    }
}
