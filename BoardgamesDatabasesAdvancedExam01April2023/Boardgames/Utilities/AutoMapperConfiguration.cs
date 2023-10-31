namespace Boardgames.Utilities
{
    using AutoMapper;

    public static class AutoMapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            MapperConfiguration configuration = new MapperConfiguration(config =>
            {
                config.AddProfile<BoardgamesProfile>();
            });

            IMapper mapper = configuration.CreateMapper();

            return mapper;
        }
    }
}
