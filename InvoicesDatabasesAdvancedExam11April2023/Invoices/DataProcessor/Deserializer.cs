namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using AutoMapper;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;
    using Invoices.Common;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            XmlParser xmlParser = new XmlParser();

            ICollection<Client> validClients = new HashSet<Client>();

            ImportClientDto[] importClientDtos = xmlParser.Deserialize<ImportClientDto[]>(xmlString, "Clients");

            foreach (var clientDto in importClientDtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = mapper.Map<Client>(clientDto);

                foreach (var addressDto in clientDto.Address)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Address address = mapper.Map<Address>(addressDto);

                    client.Addresses.Add(address);
                }

                validClients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
            }

            context.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture
            };

            ImportInvoiceDto[] importInvoiceDtos = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString, settings);

            ICollection<Invoice> validInvoices = new HashSet<Invoice>();

            foreach (var invoiceDto in importInvoiceDtos)
            {
                if (!IsValid(invoiceDto) ||
                    invoiceDto.DueDate < invoiceDto.IssueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice invoice = mapper.Map<Invoice>(invoiceDto);
                validInvoices.Add(invoice);

                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, invoice.Number));
            }

            context.AddRange(validInvoices);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IMapper mapper = AutoMapperConfiguration.CreateMapper();

            ICollection<int> validClientsIds = context.Clients
                .Select(c => c.Id)
                .ToHashSet();

            ImportProductDto[] importProductDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);

            ICollection<Product> validProducts = new HashSet<Product>();

            foreach (var productDto in importProductDtos)
            {
                if (!IsValid(productDto) ||
                    productDto.Price < ValidationConstants.ProductPriceMinValue ||
                    productDto.Price > ValidationConstants.ProductPriceMaxValue)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product product = mapper.Map<Product>(productDto);

                foreach (var clientIdDto in productDto.Clients)
                {
                    if (!validClientsIds.Contains(clientIdDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient productClient = new ProductClient
                    {
                        Product = product,
                        ClientId = clientIdDto
                    };

                    product.ProductsClients.Add(productClient);
                }

                validProducts.Add(product);
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count()));
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
