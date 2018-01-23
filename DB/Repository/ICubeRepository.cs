using System;
using System.Threading.Tasks;
using DB.Models;

namespace DB.Repository
{
    public interface ICubeRepository<TCube>
    {
        void CreateCubeAsync(TCube cube);
        Task<TCube> GetCubeAsync(string address);
        void UpdateCubeAsync(TCube cube);
    }
}
