using BAMK.Core.Controllers;
using Microsoft.Extensions.Logging;

namespace BAMK.API.Controllers
{
    public abstract class BaseController : BAMK.Core.Controllers.BaseController
    {
        protected BaseController(ILogger logger) : base(logger)
        {
        }
    }
}
