using System;
using System.Collections.Generic;

namespace Fron.Domain.Entities;

public partial class FileCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int FileCategoryEnum { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public virtual ICollection<FileStorage> FileStorage { get; set; } = new List<FileStorage>();
}
