using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Battle_Sim_V2.Models;

namespace Pokemon_Battle_Sim_V2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPokeAPI _pokeApi;

    public HomeController(ILogger<HomeController> logger, IPokeAPI pokeApi)
    {
        _logger = logger;
        _pokeApi = pokeApi;
    }


    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(Pokemon model)
    {
        if (!ModelState.IsValid)
        {
            return View(model); // Return the view with validation errors
        }

        var result = await _pokeApi.GetBasicInfo(model.Name, model.Level, model.Nature);
        return RedirectToAction("Result", new { name = result.Name, level = result.Level, nature = result.Nature });
    }

    public IActionResult Result (PokeBasic result)
    {      
        return View(result);
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

