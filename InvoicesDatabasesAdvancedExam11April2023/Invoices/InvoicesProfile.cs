namespace Invoices
{
    using AutoMapper;
    using Invoices.Data.Models;
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
        }
    }
}
