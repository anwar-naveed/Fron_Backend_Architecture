using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fron.SharedApi.Controllers;

[ApiController]
[Authorize]
public class BaseApiController : ControllerBase
{
}
