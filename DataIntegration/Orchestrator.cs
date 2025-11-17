using DataIntegration.A_Load;
using DataIntegration.B_Parse;
using DataIntegration.Db;
using DataIntegrationExercise.Models;

namespace DataIntegration;

public class Orchestrator
{
    private readonly Loader loader;
    private readonly Parser parser;
    private readonly Repository repository;
    public Orchestrator(Loader loader, Parser parser, Repository repository)
    {
        this.loader = loader;
        this.parser = parser;
        this.repository = repository;
    }

    public async Task<Product[]> RunAsync()
    {
        // TODO: Caricare prodotti e vendite
        // TODO: Raggruppare vendite per prodotto
        // TODO: Salvare risultato con _saver.SaveData(...)
        throw new NotImplementedException();
    }
}
