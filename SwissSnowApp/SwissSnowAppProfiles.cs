using System;
using AutoMapper;
using SwissSnowApp.Dtos.Plz;
using SwissSnowApp.Dtos.SnowStatistics;
using SwissSnowApp.Entities;

namespace SwissSnowApp;

public class SwissSnowAppProfiles : Profile
{
    public SwissSnowAppProfiles()
    {
        CreateMap<RecordDto, CityDto>()
            .ForMember(dest => dest.RecordId, opt => opt.MapFrom(x => x.Recordid))
            .ForMember(dest => dest.BfsNumber, opt => opt.MapFrom(x => x.Fields.Bfsnr))
            .ForMember(dest => dest.Area, opt => opt.MapFrom(x => x.Fields.Kanton))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(x => x.Fields.Ortbez27))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(x => x.Fields.Postleitzahl))
            .ForMember(dest => dest.OnRp, opt => opt.MapFrom(x => x.Fields.Onrp))
            .ForMember(dest => dest.ZipType, opt => opt.MapFrom(x => x.Fields.PlzTyp));

        CreateMap<SnowStatisticsEntity, SnowStatisticsDto>();

        CreateMap<FeatureDto, SnowStatisticsEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(x => Guid.NewGuid().ToString("D")))
            .ForMember(dest => dest.AltitudeInM, opt => opt.MapFrom(x => double.Parse(x.Properties.Altitude)))
            .ForMember(dest => dest.PosX, opt => opt.MapFrom(x => x.Geometry.Coordinates[0]))
            .ForMember(dest => dest.PosY, opt => opt.MapFrom(x => x.Geometry.Coordinates[1]))
            .ForMember(dest => dest.SnowInCm, opt => opt.MapFrom(x => x.Properties.Value))
            .ForMember(dest => dest.SnowMeasureDate,
                opt => opt.MapFrom(x => ConvertReferenceTsToDateTime(x.Properties.ReferenceTs)))
            .ForMember(dest => dest.StationId, opt => opt.MapFrom(x => x.Id))
            .ForMember(dest => dest.StationName, opt => opt.MapFrom(x => x.Properties.StationName));
    }

    private static DateTime? ConvertReferenceTsToDateTime(string referenceTs)
    {
        if (!DateTime.TryParse(referenceTs, out var result)) return null;

        return result;
    }
}