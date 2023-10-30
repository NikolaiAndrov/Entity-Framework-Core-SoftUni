namespace Invoices
{
    using AutoMapper;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;

    public class InvoicesProfile : Profile
    {
        public InvoicesProfile()
        {
            //Address
            this.CreateMap<ImportAddressDto, Address>();

            //Client
            this.CreateMap<ImportClientDto, Client>()
                .ForMember(d => d.Addresses, opt => opt.Ignore());

            //Invoice
            this.CreateMap<ImportInvoiceDto, Invoice>()
                .ForMember(d => d.CurrencyType,
                opt => opt.MapFrom(s => (CurrencyType)s.CurrencyType));

            //Product
            this.CreateMap<ImportProductDto, Product>()
                .ForMember(d => d.CategoryType, 
                opt => opt.MapFrom(s => (CategoryType)s.CategoryType));
        }
    }
}
