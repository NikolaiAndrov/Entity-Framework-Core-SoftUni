namespace Theatre
{
    using AutoMapper;
    using Theatre.Data.Models;
    using Theatre.DataProcessor.ImportDto;

    public class TheatreProfile : Profile
    {
        public TheatreProfile()
        {
            //Cast
            this.CreateMap<ImportCastDto, Cast>();

            //Ticket
            this.CreateMap<ImportTicketDto, Ticket>();

            //Theatre
            this.CreateMap<ImportTheatreDto, Theatre>()
                .ForMember(d => d.Tickets, opt => opt.Ignore());
        }
    }
}
