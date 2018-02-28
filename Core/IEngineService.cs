using System.Threading.Tasks;

namespace Core
{
	public interface IEngineService
	{
		Task SetUp();
		Task SendMessage(string message, string destinationDeviceAddress);
	}
}