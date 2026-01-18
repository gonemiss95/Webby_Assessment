using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;

namespace UserManagement.Application.Posts.Comands.CreatePostCommand
{
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, CreatePostResult>
    {
        private readonly UserManagementDbContext _dbContext;

        public CreatePostHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CreatePostResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            CreatePostResult result = new CreatePostResult();
            Post post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.PostAbbr == request.PostAbbr && x.UserId == request.UserId, cancellationToken);

            if (post != null)
            {
                result.IsCreateSuccessful = false;
                result.Message = "Post abbreviation already exists.";
            }
            else
            {
                List<int> distTagIdList = request.TagIdList.Distinct().OrderBy(x => x).ToList();

                List<int> existingTagIdList = await _dbContext.Tags
                    .Where(x => distTagIdList.Contains(x.TagId))
                    .Select(x => x.TagId)
                    .ToListAsync(cancellationToken);

                List<int> missingTagIdList = distTagIdList.Except(existingTagIdList).ToList();

                if (missingTagIdList.Count > 0)
                {
                    result.IsCreateSuccessful = false;
                    result.Message = $"Tag Id(s) {string.Join(", ", missingTagIdList)} do not exist.";
                }
                else
                {
                    DateTime dateUtcNow = DateTime.UtcNow;

                    Post newPost = new Post()
                    {
                        PostAbbr = request.PostAbbr,
                        UserId = request.UserId,
                        PostTitle = request.PostTitle,
                        CreatedTimeStamp = dateUtcNow,
                        UpdatedTimeStamp = dateUtcNow,

                        PostTags = distTagIdList.Select(x => new PostTag()
                        {
                            TagId = x,
                            CreatedTimeStamp = dateUtcNow,
                            UpdatedTimeStamp = dateUtcNow
                        })
                        .ToList()
                    };
                    await _dbContext.Posts.AddAsync(newPost, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    result.IsCreateSuccessful = true;
                    result.Message = "Post created successfully.";
                }
            }

            return result;
        }
    }
}
