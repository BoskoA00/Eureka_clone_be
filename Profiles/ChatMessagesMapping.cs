using AutoMapper;
using epp_be.Data;
using epp_be.DTOs;

namespace epp_be.Profiles
{
    public class ChatMessagesMapping : Profile
    {
        public ChatMessagesMapping()
        {
            CreateMap<ChatMessage,ChatMessagesResponseDTO>();
            CreateMap< ChatMessagesRequestDTO,ChatMessage>();
        }
    }
}
