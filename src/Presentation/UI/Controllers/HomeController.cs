namespace GamaEdtech.Backend.UI.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using System;

    public class HomeController(Lazy<ILogger<HomeController>> logger) : Farsica.Framework.Core.ControllerBase<HomeController>(logger)
    {
        public IActionResult Index() => Redirect("/swagger");
    }
}
