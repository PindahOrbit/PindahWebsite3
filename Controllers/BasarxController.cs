using Microsoft.AspNetCore.Mvc;

namespace PindahWebsite3.Controllers;

/// <summary>
/// Legacy /basarx routes redirect to the dedicated BasaRx product site.
/// </summary>
public class BasarxController : Controller
{
    private const string BasarxBase = "https://basarx.com";

    public IActionResult Index() => RedirectPermanent($"{BasarxBase}/");

    public IActionResult Dashboard() => RedirectPermanent($"{BasarxBase}/dashboard");

    public IActionResult Dispensing() => RedirectPermanent($"{BasarxBase}/dispensing");

    public IActionResult Ehr() => RedirectPermanent($"{BasarxBase}/ehr");

    public IActionResult Inventory() => RedirectPermanent($"{BasarxBase}/inventory");

    public IActionResult Refills() => RedirectPermanent($"{BasarxBase}/refills");

    public IActionResult Claims() => RedirectPermanent($"{BasarxBase}/claims");

    [HttpGet("/basarx/claims-processing-gateway")]
    public IActionResult ClaimsProcessingGateway() =>
        RedirectPermanent($"{BasarxBase}/claims-processing-gateway");

    public IActionResult Patients() => RedirectPermanent($"{BasarxBase}/patients");

    public IActionResult Integration() => RedirectPermanent($"{BasarxBase}/integration");
}
