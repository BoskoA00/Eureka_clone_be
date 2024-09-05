using AutoMapper;
using epp_be.Data;
using epp_be.DTOs;

namespace epp_be.Profiles
{
    public class CourseMapping : Profile
    {
        public CourseMapping()
        {
            CreateMap<Course, CourseResponseDTO>();
            CreateMap<CourseRequestDTO, Course>();
        }
    }
}
