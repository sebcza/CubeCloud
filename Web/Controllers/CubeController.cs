using System;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	public class CubeController : Controller
	{
		private readonly IEngineService _engineService;

		public CubeController(IEngineService engineService)
		{
			_engineService = engineService;
		}
		
		public async Task<IActionResult> Register(CreateCube createCube)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList()
				.Find(x => x.FullName.Contains(createCube.Type));
			var cubeType = assemblies.GetTypes()
				.FirstOrDefault(x => x.Name.Contains(createCube.Type + "Model"));

			Cube cube = (Cube)Activator.CreateInstance(cubeType);
			await cube.RegisterAsync(createCube);
			return Ok();
		}

		[HttpGet]
		public IActionResult GetGateway()
		{
			_engineService.SendMessage("asdasd", "CubeCloud");
			return Ok();

		}
	}
}