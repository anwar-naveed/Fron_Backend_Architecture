using System;
using System.Collections.Generic;

namespace Fron.Domain.Entities;

public partial class FileStorage
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string FileExtension { get; set; } = null!;

    public string StorageUrl { get; set; } = null!;

    public long Size { get; set; }

    public int FileCategoryId { get; set; }

    public bool Support { get; set; }

    public string? TemplateName { get; set; }

    public int? TemplateExtensionId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime ModifiedOn { get; set; }

    public virtual FileCategory FileCategory { get; set; } = null!;

    public virtual TemplateExtension? TemplateExtension { get; set; }
}
