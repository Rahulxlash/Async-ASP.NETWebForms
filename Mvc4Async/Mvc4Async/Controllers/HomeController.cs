using Microsoft.AspNetCore.Mvc;
using Mvc4Async.Models;
using Mvc4Async.Filters;
using Mvc4Async.Service;

namespace Mvc4Async.Controllers;

[UseStopwatch]
[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
public class HomeController : Controller
{
    private readonly WidgetService _widgetService;
    private readonly ProductService _productService;
    private readonly GizmoService _gizmoService;

    public HomeController(WidgetService widgetService, ProductService productService, GizmoService gizmoService)
    {
        _widgetService = widgetService;
        _productService = productService;
        _gizmoService = gizmoService;
    }

    public async Task<IActionResult> PWGasync()
    {
        ViewBag.SyncType = "Asynchronous";

        var widgetTask = _widgetService.GetWidgetsAsync();
        var prodTask = _productService.GetProductsAsync();
        var gizmoTask = _gizmoService.GetGizmosAsync();

        await Task.WhenAll(widgetTask, prodTask, gizmoTask);

        var pwgVM = new ProdGizWidgetVM(
           widgetTask.Result,
           prodTask.Result,
           gizmoTask.Result
           );

        return View("PWG", pwgVM);
    }

    public async Task<IActionResult> PWGtimeOut(CancellationToken cancellationToken)
    {
        try
        {
            ViewBag.SyncType = "Asynchronous with CancellationToken";

            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            var widgetTask = _widgetService.GetWidgetsAsync(combinedCts.Token);
            var prodTask = _productService.GetProductsAsync(combinedCts.Token);
            var gizmoTask = _gizmoService.GetGizmosAsync(combinedCts.Token);

            await Task.WhenAll(widgetTask, prodTask, gizmoTask);

            var pwgVM = new ProdGizWidgetVM(
               widgetTask.Result,
               prodTask.Result,
               gizmoTask.Result
               );

            return View("PWG", pwgVM);
        }
        catch (OperationCanceledException)
        {
            return View("TimeoutError");
        }
    }

    public IActionResult PWG()
    {
        ViewBag.SyncType = "Synchronous";

        var pwgVM = new ProdGizWidgetVM(
            _widgetService.GetWidgets(),
            _productService.GetProducts(),
            _gizmoService.GetGizmos()
           );

        return View("PWG", pwgVM);
    }

    public async Task<IActionResult> WidgetsAsync()
    {
        ViewBag.SyncOrAsync = "Asynchronous";
        return View("Widgets", await _widgetService.GetWidgetsAsync());
    }

    public IActionResult Widgets()
    {
        ViewBag.SyncOrAsync = "Synchronous";
        return View("Widgets", _widgetService.GetWidgets());
    }

    public async Task<IActionResult> GizmosCancelAsync(CancellationToken cancellationToken)
    {
        try
        {
            ViewBag.SyncOrAsync = "Asynchronous";

            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(150));
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutCts.Token);

            return View("Gizmos", await _gizmoService.GetGizmosAsync(combinedCts.Token));
        }
        catch (OperationCanceledException)
        {
            return View("TimeoutError");
        }
    }

    public async Task<IActionResult> GizmosAsync()
    {
        ViewBag.SyncOrAsync = "Asynchronous";
        return View("Gizmos", await _gizmoService.GetGizmosAsync());
    }

    public IActionResult Gizmos()
    {
        ViewBag.SyncOrAsync = "Synchronous";
        return View("Gizmos", _gizmoService.GetGizmos());
    }

    public async Task<IActionResult> ProductsAsync()
    {
        ViewBag.SyncOrAsync = "Asynchronous";
        return View("Products", await _productService.GetProductsAsync());
    }

    public IActionResult Products()
    {
        ViewBag.SyncOrAsync = "Synchronous";
        return View("Products", _productService.GetProducts());
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}

