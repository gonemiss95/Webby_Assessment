using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;
using UserManagement.Services;

namespace UserManagement.Application.Posts.Comands.DeletePostCommand
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand, DeleteResult>
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly IRedisCacheService _cacheService;

        public DeletePostHandler(UserManagementDbContext dbContext, IRedisCacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<DeleteResult> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            DeleteResult result = new DeleteResult();
            Post post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId && x.UserId == request.UserId, cancellationToken);

            if (post != null)
            {
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await _cacheService.RemoveCache("post:*");

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
