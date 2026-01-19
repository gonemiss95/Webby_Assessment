using System;
using System.Collections.Generic;

namespace UserManagement.DbContext.Models;

public partial class UserProfile
{
    public int UserProfileId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string ContactNo { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedTimeStamp { get; set; }

    public DateTime UpdatedTimeStamp { get; set; }

    public virtual User User { get; set; } = null!;
}
