using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Items;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.API.Features.Pickups;

namespace InfiniteAmmoPlugin
{
    public class EventHandlers
    {
        private readonly Plugin _plugin;

        public EventHandlers(Plugin plugin)
        {
            _plugin = plugin;
        }

        public void OnShot(ShotEventArgs ev)
        {
            if (_plugin.CurrentMode == InfiniteAmmoMode.Disabled
                || ev.Player == null
                || !ev.Player.IsAlive
                || ev.Firearm == null)
                return;

            var ammoType = ev.Firearm.AmmoType;

            if (_plugin.CurrentMode == InfiniteAmmoMode.OnlyAmmo)
            {
                SetAmmoTo101(ev.Player, ammoType);
            }
            else
            {
                ev.Firearm.MagazineAmmo++;
            }
        }

        public void OnReloading(ReloadingWeaponEventArgs ev)
        {
            if (_plugin.CurrentMode != InfiniteAmmoMode.OnlyAmmo
                || ev.Player == null
                || !ev.Player.IsAlive)
                return;

            SetAmmoTo101(ev.Player, ev.Firearm.AmmoType);
            ClearAmmoPickups();
        }

        private void SetAmmoTo101(Player player, AmmoType ammoType)
        {
            ushort current = player.GetAmmo(ammoType);
            if (current < 101)
            {
                player.AddAmmo(ammoType, (ushort)(101 - current));
            }
        }

        private void ClearAmmoPickups()
        {
            foreach (var pickup in Pickup.List)
            {
                if (IsAmmoPickup(pickup.Type))
                {
                    pickup.Destroy();
                }
            }
        }

        private bool IsAmmoPickup(ItemType type)
        {
            return type is ItemType.Ammo9x19 or ItemType.Ammo762x39 or
                   ItemType.Ammo556x45 or ItemType.Ammo12gauge;
        }
    }
}
