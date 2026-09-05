using Microsoft.AspNetCore.Mvc;

namespace PindahWebsite3.Controllers;

public class BasarxController : Controller
{
    public IActionResult Index() => View();

    public IActionResult Dashboard() => View();

    public IActionResult Dispensing() => View();

    public IActionResult Ehr() => View();

    public IActionResult Inventory() => View();

    public IActionResult Refills() => View();

    public IActionResult Claims() => View();

    public IActionResult Patients() => View();

    public IActionResult Integration() => View();
}
