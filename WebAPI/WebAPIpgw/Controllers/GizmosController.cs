using Microsoft.AspNetCore.Mvc;
using WebAPIpgw.Models;
using WebAPIpgw.Utility;

namespace WebAPIpgw.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GizmosController : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Gizmo>> GetAllGizmos(CancellationToken cancelToken = default)
    {
        // "Simulate" this operation took a long time, but was able to run without
        // blocking the calling thread (i.e., it's doing I/O operations which are async)
        // We use Task.Delay rather than Thread.Sleep, because Task.Delay returns
        // the thread immediately back to the thread-pool, whereas Thread.Sleep blocks it.
        // Task.Delay is essentially the asynchronous version of Thread.Sleep:

        await Task.Delay(Util.GetDelay(), cancelToken);

        return Gizmo.GetGizmos();
    }       
}

