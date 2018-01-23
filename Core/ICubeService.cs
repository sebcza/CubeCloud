using System;
using DB.Models;
using DB.Repository;

namespace Core
{
    public interface ICubeService
    {
        void ProcessMessage(string messag);
    }
}
