using UserManagement.Application.Tags.Queries.Dto;

namespace UserManagement.Application.Posts.Queries.Dto
{
    public class PostDto
    {
        public string PostAbbr { get; set; }

        public string PostTitle { get; set; }

        public List<TagDto> TagList { get; set; }
    }
}
