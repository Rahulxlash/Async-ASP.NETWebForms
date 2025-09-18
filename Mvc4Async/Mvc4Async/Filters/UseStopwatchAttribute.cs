using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Mvc4Async.Filters;

public class UseStopwatchAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        if (context.Controller is Microsoft.AspNetCore.Mvc.Controller controller)
        {
            controller.ViewData["stopWatch"] = stopWatch;
            controller.ViewBag.stopWatch = stopWatch;
        }
    }

    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Controller is Microsoft.AspNetCore.Mvc.Controller controller)
        {
            if (controller.ViewBag.stopWatch is Stopwatch stopWatch)
            {
                stopWatch.Stop();

                double et = stopWatch.Elapsed.Seconds +
                   (stopWatch.Elapsed.Milliseconds / 1000.0);

                controller.ViewBag.elapsedTime = et.ToString();
            }
        }
    }
}
