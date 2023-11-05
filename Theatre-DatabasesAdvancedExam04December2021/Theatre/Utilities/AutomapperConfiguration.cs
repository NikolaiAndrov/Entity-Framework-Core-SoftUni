namespace Theatre.Utilities
{
    using AutoMapper;

    public class AutomapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<TheatreProfile>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}
