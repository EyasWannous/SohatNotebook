using AutoMapper;
using SohatNotebook.Entities.DbSet;
using SohatNotebook.Entities.DTOs.Incoming;
using SohatNotebook.Entities.DTOs.Outcoming.Profile;

namespace SohatNotebook.Api.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDTO, User>()
        .ForMember(
            dest => dest.FirstName,
            from => from.MapFrom(user => $"{user.FirstName}")
        )
        .ForMember(
            dest => dest.LastName,
            from => from.MapFrom(user => $"{user.LastName}")
        ).ForMember(
            dest => dest.Email,
            from => from.MapFrom(user => $"{user.Email}")
        ).ForMember(
            dest => dest.Phone,
            from => from.MapFrom(user => $"{user.Phone}")
        ).ForMember(
            dest => dest.Country,
            from => from.MapFrom(user => $"{user.Country}")
        ).ForMember(
            dest => dest.DateOfBirth,
            from => from.MapFrom(user => Convert.ToDateTime(user.DateOfBirth))
        ).ForMember(
            dest => dest.Status,
            from => from.MapFrom(user => 1)
        );

        CreateMap<User, ProfileDTO>()
            .ForMember(
                dest => dest.FirstName,
                from => from.MapFrom(user => $"{user.FirstName}")
            )
            .ForMember(
                dest => dest.LastName,
                from => from.MapFrom(user => $"{user.LastName}")
            ).ForMember(
                dest => dest.Email,
                from => from.MapFrom(user => $"{user.Email}")
            ).ForMember(
                dest => dest.Phone,
                from => from.MapFrom(user => $"{user.Phone}")
            ).ForMember(
                dest => dest.Country,
                from => from.MapFrom(user => $"{user.Country}")
            ).ForMember(
                dest => dest.DateOfBirth,
                from => from.MapFrom(user => $"{user.DateOfBirth.ToShortDateString()}")
            );
    }
}