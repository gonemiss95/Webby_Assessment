using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;
using UserManagement.Services;

namespace UserManagement.Application.Posts.Comands.CreatePostCommand
{
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, CreateResult>
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly IRedisCacheService _cacheService;

        public CreatePostHandler(UserManagementDbContext dbContext, IRedisCacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<CreateResult> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            CreateResult result = new CreateResult();
            Post post = await _dbContext.Posts.FirstOrDefaultAsync(x => x.PostAbbr == request.PostAbbr && x.UserId == request.UserId, cancellationToken);

            if (post != null)
            {
                result.IsCreateSuccessful = false;
                result.Message = "Post abbreviation already exists.";
            }
            else
            {
                List<int> distTagIdList = request.TagIdList.Distinct().OrderBy(x => x).ToList();
                List<Tag> allTagList = await _cacheService.GetCache<List<Tag>>("tag:all");

                if (allTagList == null)
                {
                    allTagList = await _dbContext.Tags.ToListAsync(cancellationToken);
                    await _cacheService.SetCache("tag:all", allTagList, TimeSpan.FromMinutes(30));
                }

                List<int> existingTagIdList = allTagList
                    .Where(x => distTagIdList.Contains(x.TagId))
                    .Select(x => x.TagId)
                    .ToList();

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
                        PostTags = distTagIdList
                            .Select(x => new PostTag()
                            {
                                TagId = x,
                                CreatedTimeStamp = dateUtcNow,
                                UpdatedTimeStamp = dateUtcNow
                            })
                            .ToList()
                    };
                    await _dbContext.Posts.AddAsync(newPost, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    await _cacheService.RemoveCache("post:*");

                    result.IsCreateSuccessful = true;
                    result.Message = "New post successfully created.";
                }
            }

            return result;
        }
    }
}
