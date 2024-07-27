using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Battle_Sim_V2.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pokemon_Battle_Sim_V2.Controllers
{
    public class GameController : Controller
    {
        private readonly Pokemon player1;
        private readonly Pokemon player2;

        public GameController(Pokemon first, Pokemon second)
        {
            this.player1 = first;
            this.player2 = second;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Move(List<Move> moves)
        {
            return View(moves);
        }

        public IActionResult Sprites(string spritePath)
        {
            if(player1.FrontSprite == spritePath)
            {
                return View(player1);
            }
            else
            {
                return View(player2);
            }
        }

        public IActionResult HP()
        {
            return View(player1);
        }
    }
}

