using System;
using Microsoft.AspNetCore.Mvc;
namespace DisplayCube.Controllers
{
    public class DisplayController : Controller
    {
        public string Index(){
            return "Działa";
        }

        [HttpPost]
        public IActionResult SetText(){
            
            return Ok();
        }
    }
}
