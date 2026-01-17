using System;
using System.Collections.Generic;

namespace UserManagement.DbContext.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreatedTimeStamp { get; set; }

    public DateTime UpdatedTimeStamp { get; set; }

    public virtual ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
