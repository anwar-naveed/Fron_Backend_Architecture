namespace Fron.Application.Abstractions.Application;

public interface IUserResolverService
{
    int GetUserId();
    string GetLoggedInUsername();
    bool IsUserAuthenticated();
}
