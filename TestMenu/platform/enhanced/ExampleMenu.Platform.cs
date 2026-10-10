using CitizenFX.FiveM.Client;
using CitizenFX.FiveM.Shared.Script;

namespace TestMenu;

public partial class ExampleMenu : IScript
{
    private static void Log(string message) => API.Log.Info(message);
}
