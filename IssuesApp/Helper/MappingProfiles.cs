using AutoMapper;
using IssuesApp.Dto;
using IssuesApp.Models;

namespace IssuesApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Admin, AdminDto>();
            CreateMap<AdminDto, Admin>();
            CreateMap<ClientUser, ClientUserDto>();
            CreateMap<ClientUserDto, ClientUser>();
            CreateMap<Issue, IssueDto>();
            CreateMap<IssueDto, Issue>();
            CreateMap<IssueComment, IssueCommentDto>();
            CreateMap<IssueCommentDto, IssueComment>();
        }
    }
}
