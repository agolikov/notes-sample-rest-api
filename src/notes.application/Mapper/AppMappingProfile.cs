using AutoMapper;
using notes.application.Models.Note;
using notes.application.Models.User;
using notes.data.Entities;
using System.Linq;

namespace notes.application.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserModel>()
                .ForMember(r => r.Id, src => src.MapFrom(e => e.Id));
            
            CreateMap<SignUpModel, User>();

            CreateMap<Note, NoteModel>()
                .ForMember(t => t.Tags, src => src.MapFrom(r => r.Tags.Select(t => t.Value).ToArray()));
            
            CreateMap<CreateNoteModel, Note>()
                .ForMember(t => t.Tags, src => src.MapFrom(r => r.Tags.Select(t => new Tag { Value = t }).ToArray()));

            CreateMap<UpdateNoteModel, Note>()
                .ForMember(t => t.Tags, src => src.MapFrom(r => r.Tags.Select(t => new Tag { Value = t }).ToArray()));
        }
    }
}