using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using DB.Models;

namespace DisplayCube.Models
{
    public class DisplayCubeModel : Cube
    {
        public Dictionary<int,string> Content { get; set; }

        private String[] messageItems;
        private readonly Cube cubeEntity;
        private readonly DisplayCubeRepository cubeRepository;
        private readonly EngineService engineService;

        public DisplayCubeModel(DisplayCubeModel cubeEntity, DisplayCubeRepository cubeRepository, EngineService engineService)
        {
            this.cubeEntity = cubeEntity;
            this.cubeRepository = cubeRepository;
            this.engineService = engineService;
        }

	    public DisplayCubeModel()
	    {
		    Name = "Pokój";
	    }

        public void SetDisplayText(int row, string content)
        {
            int line = row + 1;
            engineService.SendMessage(cubeEntity.Address + "|" + (int)DisplayCubeModel.Actions.SetDisplay + "|" + row + "|" + content, cubeEntity.Address);
        }

        public void TurnOnBacklight()
        {
            engineService.SendMessage(cubeEntity.Address + "|" + (int)DisplayCubeModel.Actions.SetBacklight + "|1", cubeEntity.Address);
        }

        public void TurnOffBacklight()
        {
            engineService.SendMessage(cubeEntity.Address + "|" + (int)DisplayCubeModel.Actions.SetBacklight + "|0", cubeEntity.Address);
        }

        public override void ProcessMessage(string messag)
        {
            messageItems = messag.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

	    public override async Task RegisterAsync(CreateCube createCube)
	    {
			this.Content = new Dictionary<int, string>();
			this.Id = new Guid();
		    this.Address = createCube.Address;
		    this.Name = createCube.Name;
		    this.Type = this.GetType().ToString();
		    await cubeRepository.CreateCubeAsync(this);
	    }


	    public enum Actions
        {
            SetDisplay = 1,
            SetBacklight = 2
        }
    }
}
