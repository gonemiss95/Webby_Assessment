using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;

namespace UserManagement.Application.Posts.Comands.DeletePostCommand
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand, DeleteResult>
    {
        private readonly UserManagementDbContext _dbContext;

        public DeletePostHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteResult> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            DeleteResult result = new DeleteResult();
            Post post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId && x.UserId == request.UserId, cancellationToken);

            if (post != null)
            {
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync(cancellationToken);

                result.IsDeleteSuccessful = true;
                result.Message = "Post successfully deleted.";
            }
            else
            {
                result.IsDeleteSuccessful = false;
                result.Message = "Post not found.";
            }

            return result;
        }
    }
}
