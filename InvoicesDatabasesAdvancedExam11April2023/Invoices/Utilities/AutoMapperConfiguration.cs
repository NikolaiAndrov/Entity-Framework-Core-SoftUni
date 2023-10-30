namespace Invoices.Utilities
{
    using AutoMapper;

    public static class AutoMapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<InvoicesProfile>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}
