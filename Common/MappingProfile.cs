using AutoMapper;
using GameStore.Dtos;
using GameStore.Models.Games;

namespace GameStore.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameCreateDto, Game>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src =>
                src.Tags.Select(tag => new Tag { Name = tag }).ToList()));
        }

    }
}
