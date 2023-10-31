namespace Boardgames
{
    using AutoMapper;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;

    public class BoardgamesProfile : Profile
    {
        public BoardgamesProfile()
        {
            //BoardGame
            this.CreateMap<ImportBoardgameDto, Boardgame>()
                .ForMember(d => d.CategoryType, 
                opt => opt.MapFrom(s => (CategoryType)s.CategoryType));

            //Creator
            this.CreateMap<ImportCreatorDto, Creator>()
                .ForMember(d => d.Boardgames, opt => opt.Ignore());

            //Seller
            this.CreateMap<ImportSellerDto, Seller>()
                .ForMember(d => d.BoardgamesSellers, opt => opt.Ignore());
        }
    }
}