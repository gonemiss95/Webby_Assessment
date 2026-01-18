using MediatR;
using Microsoft.EntityFrameworkCore;
using UserManagement.DbContext;
using UserManagement.DbContext.Models;

namespace UserManagement.Application.Tags.Comands.CreateTagCommand
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, CreateResult>
    {
        private readonly UserManagementDbContext _dbContext;

        public CreateTagHandler(UserManagementDbContext dbContext)
        {
            _dbContext = dbContext;
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

                result.IsCreateSuccessful = true;
                result.Message = "New tag successfully created.";
            }

            return result;
        }
    }
}
