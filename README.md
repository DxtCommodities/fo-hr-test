# Data Integration Exercise

## 🎯 Obiettivo
Integrare dati da fonti diverse:
- JSON con metadati e lista prodotti
- CSV con vendite
- HTML non formattato con info aggiuntive (garanzia, paese)

Salvare i dati in un database SQLite.
Stampare un report di riepilogo delle vendite aggregato per paese, non considerando le vendite fuori da periodo di validità del prodotto.

### Keep in mind:
 - Utilizzare C# .NET 10
 - Non over-engineerare: soluzione semplice e funzionale
 - Pensare alla memoria ed efficienza della soluzione
 - E' permesso usare librerie esterne (non a pagamento)
 - Per il report finale, va bene una stampa su console semplice oppure (se in grado) un server web minimale o una SPA javaScript.

---

## 📝 Compiti da svolgere
0. **Run server** → eseguire FileWrapper (`dotnet run` in un terminale oppure start without debugging da IDE)
1. **Loader** → legge le tre sorgenti (html, csv, json) contenute nella classe di configurazione `Settings`
2. **Parser** → converte i tre files in una lista di `Product`.
3. **Repository** → salva la lista di prodotti in db. Implementare entrambi i metodi `Save`
4. **Repository** → implementare il metodo `GetAggregations` che ritorna, *per ogni mese locale e nazione*:
   - AveragePriceOfProducts: media dei prezzi di vendita. i.e.: dati 3 prodotti diversi, ognuno con il suo prezzo, ritorna la media dei prezzi indipendentemente dalle vendite.
   - TotalNumberDistinctOfProducts: numero totale di prodotti distinti venduti in quel mese in quella nazione.
   - TotalRevenew: somma totale delle vendite (Quantity * Price) in quel mese in quella nazione.

---

## ✅ Test
I test esistenti verificano il funzionamento base di Parser e Repository.

---

## 🚀 Avvio
dotnet run