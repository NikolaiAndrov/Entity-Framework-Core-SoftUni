namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {
            XmlParser xmlParser = new XmlParser();

            ExportClientDto[] clients = context.Clients
                .Where(c => c.Invoices.Any(i => i.IssueDate >= date))
                .Select(c => new ExportClientDto
                {
                    InvoicesCount = c.Invoices.Count,
                    ClientName = c.Name,
                    VatNumber = c.NumberVat,
                    Invoices = c.Invoices
                    .OrderBy(i => i.IssueDate)
                    .ThenByDescending(i => i.DueDate)
                    .Select(i => new ExportInvoiceDto
                    {
                        InvoiceNumber = i.Number,
                        InvoiceAmount = i.Amount,
                        DueDate = i.DueDate.ToString("MM/dd/yyyy"),
                        Currency = i.CurrencyType.ToString()
                    })
                    .ToArray()

                })
                .OrderByDescending(c => c.InvoicesCount)
                .ThenBy(c => c.ClientName)
                .ToArray();

            return xmlParser.Serialize(clients, "Clients");
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {
            var products = context.Products
                .Where(p => p.ProductsClients.Any() && p.ProductsClients.Any(pc => pc.Client.Name.Length >= nameLength))
                .Select(p => new
                {
                    p.Name,
                    p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients
                    .Where(pc => pc.Client.Name.Length >= nameLength)
                    .Select(pc => new
                    {
                        pc.Client.Name,
                        pc.Client.NumberVat
                    })
                    .OrderBy(c => c.Name)
                    .ToArray()
                })
                .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);
        }
    }
}