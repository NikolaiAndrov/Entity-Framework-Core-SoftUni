namespace Artillery.Utilities
{
    using AutoMapper;

    public class AutoMapperConfiguration
    {
        public static IMapper CreateMapper()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(opt =>
            {
                opt.AddProfile<ArtilleryProfile>();
            });

            IMapper mapper = mapperConfiguration.CreateMapper();

            return mapper;
        }
    }
}
