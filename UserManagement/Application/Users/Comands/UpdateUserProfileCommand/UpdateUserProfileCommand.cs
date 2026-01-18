using MediatR;
using System.Text.Json.Serialization;

namespace UserManagement.Application.Users.Comands.UpdateUserProfileCommand
{
    public class UpdateUserProfileCommand : IRequest<bool>
    {
        [JsonIgnore]
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }
    }
}
