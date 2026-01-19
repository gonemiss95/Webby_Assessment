using System;
using System.Collections.Generic;

namespace UserManagement.DbContext.Models;

public partial class PostTag
{
    public int PostTagId { get; set; }

    public int PostId { get; set; }

    public int TagId { get; set; }

    public DateTime CreatedTimeStamp { get; set; }

    public DateTime UpdatedTimeStamp { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
