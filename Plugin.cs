using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;

namespace InfiniteAmmoPlugin
{
    public enum InfiniteAmmoMode
    {
        Disabled,
        OnlyAmmo,
        AutoReload
    }

    public class Plugin : Plugin<Config>
    {
        public override string Name => "Infinite Ammo";
        public override string Prefix => "ia";
        public override string Author => "Alpaca";
        public override Version Version => new Version(1, 0, 2);

        public static Plugin Instance { get; private set; } = null!;

        private EventHandlers? _handlers;
        private InfiniteAmmoMode _mode = InfiniteAmmoMode.Disabled;

        public override void OnEnabled()
        {
            Instance = this;
            _handlers = new EventHandlers(this);

            Exiled.Events.Handlers.Player.Shot += _handlers.OnShot;
            Exiled.Events.Handlers.Player.ReloadingWeapon += _handlers.OnReloading;

            Log.Info($"[{Name}] Plugin Loaded! Commands: ia autoreload | ia onlyammo | ia off");
        }

        public override void OnDisabled()
        {
            if (_handlers != null)
            {
                Exiled.Events.Handlers.Player.Shot -= _handlers.OnShot;
                Exiled.Events.Handlers.Player.ReloadingWeapon -= _handlers.OnReloading;
            }

            _handlers = null;
            Instance = null!;
        }

        public void SetMode(InfiniteAmmoMode newMode)
        {
            _mode = newMode;
            string status = _mode switch
            {
                InfiniteAmmoMode.AutoReload => "<color=green>ON + AUTO-RELOAD</color>",
                InfiniteAmmoMode.OnlyAmmo => "<color=green>ON (ONLY AMMO)</color>",
                _ => "<color=red>ВЫКЛЮЧЕНЫ</color>"
            };

            Map.Broadcast(5, $"Бесконечные боеприпасы {status} для всех!");
            Log.Info($"Infinita Ammo Mode: {_mode}");
        }

        public InfiniteAmmoMode CurrentMode => _mode;
    }
}
