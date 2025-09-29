using Microsoft.AspNetCore.Mvc;
using WebAppAsync.Models;
using WebAppAsync.Service;

namespace WebAppAsync.Controllers;

public class HomeController : Controller
{
    private readonly GizmoService _gizmoService;

    public HomeController(GizmoService gizmoService)
    {
        _gizmoService = gizmoService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GizmosAsync()
    {
        ViewBag.SyncOrAsync = "Asynchronous";
        var gizmos = await _gizmoService.GetGizmosAsync();
        return View("Gizmos", gizmos);
    }

    public async Task<IActionResult> GizmosCancelAsync(CancellationToken cancellationToken)
    {
        try
        {
            ViewBag.SyncOrAsync = "Asynchronous with CancellationToken";
            
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(150));
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            var gizmos = await _gizmoService.GetGizmosAsync(combinedCts.Token);
            return View("Gizmos", gizmos);
        }
        catch (OperationCanceledException)
        {
            return View("TimeoutError");
        }
    }

    public IActionResult Gizmos()
    {
        ViewBag.SyncOrAsync = "Synchronous";
        var gizmos = _gizmoService.GetGizmos();
        return View("Gizmos", gizmos);
    }

    public IActionResult Error()
    {
        return View();
    }

    public IActionResult TimeoutError()
    {
        return View();
    }
}
