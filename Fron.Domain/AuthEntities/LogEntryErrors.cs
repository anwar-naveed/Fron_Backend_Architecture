using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fron.Domain.AuthEntities;
public class LogEntryErrors
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Exception { get; set; } = null!;

    public string Message { get; set; } = null!;

    public int StatusCode { get; set; }

    public string StackTrace { get; set; } = null!;

    public string? UserDescription { get; set; }

    public string RequestMethod { get; set; } = null!;

    public string RequestPath { get; set; } = null!;

    public string RequestHeaders { get; set; } = null!;

    public string Source { get; set; } = null!;
}
