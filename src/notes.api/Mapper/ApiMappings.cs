using AutoMapper;
using notes.api.Models;
using notes.application.Constants;
using notes.application.Models.Note;

namespace notes.api.Mapper
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<NotesRequestModel, NoteQueryModel>()
                .ForMember(t => t.Page, src => src.MapFrom(r => r.Page ?? 1))
                .ForMember(t => t.Size, src => src.MapFrom(r => r.Size ?? AppConstants.DefaultPageSize));
        }
    }
}
