using System;
using System.Collections.Generic;
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

        public void SetDisplayText(int row, string content)
        {
            int line = row + 1;
            engineService.SendMessage(cubeEntity.Address + "|" + (int)DisplayCubeModel.Actions.SetDisplay + "|" + row + "|" + content);
        }

        public void TurnOnBacklight()
        {
            engineService.SendMessage(cubeEntity.Address + "|" + (int)DisplayCubeModel.Actions.SetBacklight + "|1");
        }

        public void TurnOffBacklight()
        {
            engineService.SendMessage(cubeEntity.Address + "|" + (int)DisplayCubeModel.Actions.SetBacklight + "|0");
        }

        public override void ProcessMessage(string messag)
        {
            messageItems = messag.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }


        public enum Actions
        {
            SetDisplay = 1,
            SetBacklight = 2
        }
    }
}
