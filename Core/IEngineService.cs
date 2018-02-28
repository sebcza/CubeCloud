using System.Threading.Tasks;

namespace Core
{
	public interface IEngineService
	{
		void SetUp();
		Task SendMessage(string message, string destinationDeviceAddress);
	}
}