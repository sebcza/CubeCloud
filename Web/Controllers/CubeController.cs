using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class CubeController : Controller
    {
        public string Create(){
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList()
                                      .Find(x => x.FullName.Contains("DisplayCube"));
            var cubeType = assemblies.GetTypes()
               .FirstOrDefault(x => x.Name.Contains("DisplayCubeModel"));
            return "OK";
        }
    }
}
