using MediatR;
using System.Text.Json.Serialization;

namespace UserManagement.Application.Posts.Comands.CreatePostCommand
{
    public class CreatePostCommand : IRequest<CreateResult>
    {
        public string PostAbbr { get; set; }

        public string PostTitle { get; set; }

        public List<int> TagIdList { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
