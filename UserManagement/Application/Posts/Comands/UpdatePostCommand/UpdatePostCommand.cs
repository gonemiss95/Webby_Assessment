using MediatR;
using System.Text.Json.Serialization;

namespace UserManagement.Application.Posts.Comands.UpdatePostCommand
{
    public class UpdatePostCommand : IRequest<UpdateResult>
    {
        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public List<int> TagIdList { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
