using AutoMapper;
using epp_be.Data;
using epp_be.DTOs;

namespace epp_be.Profiles
{
    public class LessonMapping : Profile
    {
        public LessonMapping()
        {
            CreateMap<Lesson, LessonResponseDTO>();
            CreateMap<LessonRequestDTO,Lesson>();
        }
    }
}
