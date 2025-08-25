using AutoMapper;
using ForkliftAPI.Application.DTOs;
using ForkliftAPI.Domain.Entities;
using System.Globalization;

namespace ForkliftAPI.Application.MappingProfiles
{
    public class ForkliftProfile : Profile
    {
        private static readonly GregorianCalendar Gregorian = new GregorianCalendar();

        public ForkliftProfile()
        {
            // DTO -> Entity
            CreateMap<ForkliftDto, Forklift>()
                .ForCtorParam("name", opt => opt.MapFrom(src => src.Name ?? string.Empty))
                .ForCtorParam("modelNumber", opt => opt.MapFrom(src => src.ModelNumber ?? string.Empty))
                .ForCtorParam("manufacturingDate", opt => opt.MapFrom(src => ParseDate(src.ManufacturingDate)));

            // Entity -> DTO
            CreateMap<Forklift, ForkliftDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ModelNumber, opt => opt.MapFrom(src => src.ModelNumber))
                .ForMember(dest => dest.ManufacturingDate,
                           opt => opt.MapFrom(src => FormatDate(src.ManufacturingDate)));

            CreateMap<ForkliftCommand, ForkliftCommandDto>()
                .ForMember(dest => dest.ModelNumber, opt => opt.MapFrom(src => src.ModelNumber))
                .ForMember(dest => dest.Command, opt => opt.MapFrom(src => src.Command))
                .ForMember(dest => dest.ActionDate,
                           opt => opt.MapFrom(src => src.ActionDate.ToString("o")));

            CreateMap<ForkliftCommandDto, ForkliftCommand>()
                .ForMember(dest => dest.ModelNumber, opt => opt.MapFrom(src => src.ModelNumber))
                .ForMember(dest => dest.Command, opt => opt.MapFrom(src => src.Command))
                .ForMember(dest => dest.ActionDate, opt => opt.MapFrom(src => ParseDate(src.ActionDate)));

        }

        private static DateTime ParseDate(string? dateString)
        {
            var provider = CultureInfo.GetCultureInfo("en-US");

            return DateTime.TryParseExact(
                dateString ?? string.Empty,
                "yyyy-MM-dd",
                provider,
                DateTimeStyles.None,
                out var date) ? date : DateTime.MinValue;
        }

        private static string FormatDate(DateTime date)
        {
            var provider = CultureInfo.GetCultureInfo("en-US");
            return date.ToString("yyyy-MM-dd", provider);
        }
    }
}
