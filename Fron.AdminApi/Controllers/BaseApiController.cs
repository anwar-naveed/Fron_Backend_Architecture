using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fron.AdminApi.Controllers;

[ApiController]
[Authorize]
public class BaseApiController : ControllerBase
{
}
