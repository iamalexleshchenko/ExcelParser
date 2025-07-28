using AutoMapper;
using ExcelParser.DTOs;
using ExcelParser.Entities;

namespace ExcelParser.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PersonEntity, Person>()
            .PreserveReferences()
            .ReverseMap();
    }
}