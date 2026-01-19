using MediatR;

namespace UserManagement.Application.Posts.Comands.DeletePostCommand
{
    public class DeletePostCommand : IRequest<DeleteResult>
    {
        public int PostId { get; set; }

        public int UserId { get; set; }
    }
}
