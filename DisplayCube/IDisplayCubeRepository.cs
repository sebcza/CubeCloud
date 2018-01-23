using System;
using DB.Repository;
using DisplayCube.Models;

namespace DisplayCube
{
    public interface IDisplayCubeRepository : ICubeRepository<DisplayCubeModel>
    {
    }
}
