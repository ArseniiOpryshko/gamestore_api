using AutoMapper;
using GameStore.Dtos.GameDtos;
using GameStore.Models.Games;

namespace GameStore.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameCreateDto, Game>()
                .ForMember(dest => dest.Tags, opt => opt.Ignore());

            CreateMap<GameUpdateDto, Game>()
                .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.Ignore());
        }

    }
}
