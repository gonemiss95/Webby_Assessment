using MediatR;
using System.Text.Json.Serialization;

namespace UserManagement.Application.Tags.Comands.CreateTagCommand
{
    public class CreateTagCommand : IRequest<CreateResult>
    {
        public string TagName { get; set; }

        public string TagDescription { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
    }
}
