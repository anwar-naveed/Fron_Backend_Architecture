namespace Fron.Domain.Dto.Login;
public sealed record LoginResponseDto(string GroupId,
    string UserId,
    string? CompanyId,
    string? CompanyName,
    string? UserName,
    string? UserFullName,
    string? Role,
    string? Department,
    string[]? MenuRights,
    //IEnumerable<ETTUserPrivilegeResponseDto>? UserRightsDetails,
    string? Exp1,
    string? Exp2,
    string Token);