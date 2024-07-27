using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Battle_Sim_V2.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pokemon_Battle_Sim_V2.Controllers
{
    public class SelectionController : Controller
    {
        HttpClient _client = new HttpClient();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PokeInfo()
        {
            var poke1 = new Pokemon(_client);
            return View(poke1);
        }
    }
}

