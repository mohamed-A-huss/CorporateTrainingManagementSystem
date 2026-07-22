using CorporateTrainingManagementSystem.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace CorporateTrainingManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer _localizer;

        public HomeController(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create("SharedResource", "CorporateTrainingManagementSystem");
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login","Account", new { area = SD.IDENTITY_AREA });
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(returnUrl))
                returnUrl = "/";

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });

            return LocalRedirect(returnUrl);
        }
        public IActionResult Test()
        {
            var value = _localizer["Dashboard"];

            return Content($"""
Value = {value.Value}
ResourceNotFound = {value.ResourceNotFound}
""");
        }


        public IActionResult CultureTest()
          {
        return Content(
            $"CurrentCulture = {CultureInfo.CurrentCulture.Name}\n" +
            $"CurrentUICulture = {CultureInfo.CurrentUICulture.Name}");
         }   
}
}
