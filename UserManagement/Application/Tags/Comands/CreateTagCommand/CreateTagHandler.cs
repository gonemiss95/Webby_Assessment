using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;
using UserManagement.Services;

namespace UserManagement.Application.Tags.Comands.CreateTagCommand
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, CreateResult>
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly IRedisCacheService _cacheService;

        public CreateTagHandler(UserManagementDbContext dbContext, IRedisCacheService cacheService)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
        }

        public async Task<CreateResult> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            CreateResult result = new CreateResult();
            Tag tag = await _dbContext.Tags.FirstOrDefaultAsync(x => x.TagName == request.TagName, cancellationToken);

            if (tag != null)
            {
                result.IsCreateSuccessful = false;
                result.Message = "Tag name already exists.";
            }
            else
            {
                DateTime dateUtcNow = DateTime.UtcNow;

                Tag newTag = new Tag()
                {
                    TagName = request.TagName,
                    TagDescription = request.TagDescription,
                    CreatedUserId = request.UserId,
                    CreatedTimeStamp = dateUtcNow,
                    UpdatedUserId = request.UserId,
                    UpdatedTimeStamp = dateUtcNow,
                };
                await _dbContext.Tags.AddAsync(newTag, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);

                await _cacheService.RemoveCache("tag:*");

                result.IsCreateSuccessful = true;
                result.Message = "New tag successfully created.";
            }

            return result;
        }
    }
}
