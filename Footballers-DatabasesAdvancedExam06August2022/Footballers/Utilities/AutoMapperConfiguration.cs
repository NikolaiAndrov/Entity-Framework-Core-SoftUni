namespace Footballers.Utilities
{
    using AutoMapper;

    public static class AutoMapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<FootballersProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
        }
    }
}
