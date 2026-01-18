using System;
using System.Collections.Generic;

namespace UserManagement.DbContext.Models;

public partial class Tag
{
    public int TagId { get; set; }

    public string TagName { get; set; } = null!;

    public string TagDescription { get; set; } = null!;

    public int CreatedUserId { get; set; }

    public DateTime CreatedTimeStamp { get; set; }

    public int UpdatedUserId { get; set; }

    public DateTime UpdatedTimeStamp { get; set; }
}
