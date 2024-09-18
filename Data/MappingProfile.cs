using AutoMapper;
using GameStore.Dtos;
using GameStore.Models.Games;

namespace GameStore.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameCreateDto, Game>();
        }
            
    }
}
