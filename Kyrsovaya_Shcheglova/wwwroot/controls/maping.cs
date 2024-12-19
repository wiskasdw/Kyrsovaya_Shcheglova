using Microsoft.AspNetCore.Mvc;

namespace Kyrsovaya_Shcheglova.wwwroot.controls
{
    public class Mapping : Controller 
    {
        public IActionResult Admin()
        {
            return View("admin"); 
        }

        public IActionResult Index()
        {


