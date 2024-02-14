using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AutoGlassDesafioApi.Presentation.SharedContext.Controller
{
    [Route("api/[controller]")]    
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public abstract class BaseAbstractController : ControllerBase { }
}
