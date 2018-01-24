using System;
using System.Threading.Tasks;
using DB.Models;

namespace DB.Repository
{
    public interface ICubeRepository<TCube>
    {
        Task CreateCubeAsync(TCube cube);
        Task<TCube> GetCubeAsync(string address);
        void UpdateCubeAsync(TCube cube);
    }
}
