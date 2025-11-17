
# Data Integration Exercise

## 🎯 Objective
Integrate data from different sources:
- JSON with metadata and product list
- CSV with sales
- Unformatted HTML with additional info (warranty, country)

Save the data in an SQLite database. Print a summary sales report aggregated by country, excluding sales outside the product validity period.

### Keep in mind:
- Use **C# .NET 10**
- Do not over-engineer: keep the solution simple and functional
- Consider memory usage and efficiency
- External libraries are allowed (as long as they are free)
- For the final report, a simple console output is fine, or (if possible) a minimal web server or a JavaScript SPA.

---

## 📝 Tasks
0. **Run server** → Execute FileWrapper (`dotnet run` in a terminal or *Start Without Debugging* from IDE)
1. **Loader** → Reads the three sources (HTML, CSV, JSON) specified in the `Settings` configuration class
2. **Parser** → Converts the three files into a list of `Product`
3. **Repository** → Saves the product list into the database. Implement both `Save` methods
4. **Repository** → Implement the `GetAggregations` method that returns, *for each local month and country*:
   - **AveragePriceOfProducts**: average of product prices (e.g., given 3 different products, each with its own price, return the average price regardless of sales)
   - **TotalNumberDistinctOfProducts**: total number of distinct products sold in that month in that country
   - **TotalRevenue**: total sum of sales (Quantity * Price) in that month in that country

---

## ✅ Tests
Existing tests verify basic functionality of Parser and Repository.

---

## 🚀 Start
`dotnet run`


## 🔄 Submission
- **Fork** the project repository
- Complete the exercise in your fork
- Once finished, **send us the link to your forked repository**

