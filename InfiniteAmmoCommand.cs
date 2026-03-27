using CommandSystem;
using Exiled.API.Features;

namespace InfiniteAmmoPlugin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class InfiniteAmmoCommand : ICommand
    {
        public string Command => "infiniteammo";
        public string[] Aliases => new[] { "ia" };
        public string Description => "Управление бесконечными боеприпасами: ia autoreload | ia onlyammo | ia off";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string arg = "";
            if (arguments.Count > 0 && arguments.Array != null)
            {
                arg = arguments.Array[arguments.Offset].ToLower();
            }

            if (arg == "autoreload")
            {
                Plugin.Instance.SetMode(InfiniteAmmoMode.AutoReload);
                response = "✅ Режим: бесконечные патроны + авто-перезарядка";
                return true;
            }

            if (arg == "onlyammo")
            {
                Plugin.Instance.SetMode(InfiniteAmmoMode.OnlyAmmo);
                response = "✅ Режим: только бесконечные патроны (альпака будет срать вам патроны в инвентарь без авто-перезарядки)";
                return true;
            }

            if (arg == "off" || arg == "disable")
            {
                Plugin.Instance.SetMode(InfiniteAmmoMode.Disabled);
                response = "✅ Бесконечные боеприпасы выключены";
                return true;
            }

            if (Plugin.Instance.CurrentMode == InfiniteAmmoMode.Disabled)
            {
                Plugin.Instance.SetMode(InfiniteAmmoMode.AutoReload);
                response = "✅ Включён режим autoreload";
            }
            else
            {
                Plugin.Instance.SetMode(InfiniteAmmoMode.Disabled);
                response = "✅ Выключено";
            }
            return true;
        }
    }
}
