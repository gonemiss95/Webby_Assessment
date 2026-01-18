using System;
using System.Collections.Generic;

namespace UserManagement.DbContext.Models;

public partial class Post
{
    public int PostId { get; set; }

    public int UserId { get; set; }

    public string PostAbbr { get; set; } = null!;

    public string PostTitle { get; set; } = null!;

    public DateTime CreatedTimeStamp { get; set; }

    public DateTime UpdatedTimeStamp { get; set; }

    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

    public virtual User User { get; set; } = null!;
}
