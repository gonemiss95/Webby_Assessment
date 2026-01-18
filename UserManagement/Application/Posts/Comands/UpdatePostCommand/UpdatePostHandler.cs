using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;

namespace UserManagement.Application.Posts.Comands.UpdatePostCommand
{
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, UpdateResult>
    {
        private readonly UserManagementDbContext _dbContext;

        public UpdatePostHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateResult> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            UpdateResult result = new UpdateResult();
            Post post = await _dbContext.Posts
                .Include(x => x.PostTags)
                .FirstOrDefaultAsync(x => x.PostId == request.PostId && x.UserId == request.UserId, cancellationToken);

            if (post != null)
            {
                List<int> distTagIdList = request.TagIdList.Distinct().OrderBy(x => x).ToList();

                List<int> existingTagIdList = await _dbContext.Tags
                    .Where(x => distTagIdList.Contains(x.TagId))
                    .Select(x => x.TagId)
                    .ToListAsync(cancellationToken);

                List<int> missingTagIdList = distTagIdList.Except(existingTagIdList).ToList();

                if (missingTagIdList.Count > 0)
                {
                    result.IsUpdateSuccessful = false;
                    result.Message = $"Tag Id(s) {string.Join(", ", missingTagIdList)} do not exist.";
                }
                else
                {
                    DateTime dateUtcNow = DateTime.UtcNow;

                    List<int> allTagIdList = post.PostTags.Select(x => x.TagId).ToList();
                    List<int> tagIdToAdd = distTagIdList.Except(allTagIdList).ToList();
                    List<int> tagIdToRemove = allTagIdList.Except(distTagIdList).ToList();

                    _dbContext.PostTags.RemoveRange(post.PostTags.Where(x => tagIdToRemove.Contains(x.TagId)));

                    post.PostTitle = request.PostTitle;
                    post.UpdatedTimeStamp = dateUtcNow;
                    post.PostTags = tagIdToAdd
                        .Select(x => new PostTag()
                        {
                            PostId = post.PostId,
                            TagId = x,
                            CreatedTimeStamp = dateUtcNow,
                            UpdatedTimeStamp = dateUtcNow
                        })
                        .ToList();
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    result.IsUpdateSuccessful = true;
                    result.Message = "Post successfully updated.";
                }
            }
            else
            {
                result.IsUpdateSuccessful = false;
                result.Message = "Post not found.";
            }

            return result;
        }
    }
}
