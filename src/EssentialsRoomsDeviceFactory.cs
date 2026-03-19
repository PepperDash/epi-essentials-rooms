using System;
using System.Collections.Generic;
using PepperDash.Core;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Config;

namespace PDT.Plugins.Essentials.Rooms
{
#if ESSENTIALS_V2
    /// <summary>
    /// Registers all room types from this plugin with the Essentials v2 DeviceFactory.
    /// Discovered and called automatically by DeviceFactory via reflection on startup.
    /// </summary>
    public class EssentialsRoomsDeviceFactory : IDeviceFactory
    {
        public List<string> TypeNames { get; } = new List<string>
        {
            "huddle",
            "huddlevtc1",
            "dualdisplay",
            "combinedhuddlevtc1",
            "techroom"
        };

        public Type FactoryType { get; } = typeof(EssentialsRoomBase);

        public EssentialsDevice BuildDevice(DeviceConfig dc)
        {
            switch (dc.Type.ToLower())
            {
                case "huddle":
                    return new EssentialsHuddleSpaceRoom(dc);
                case "huddlevtc1":
                    return new EssentialsHuddleVtc1Room(dc);
                case "dualdisplay":
                    return new EssentialsDualDisplayRoom(dc);
                case "combinedhuddlevtc1":
                    return new EssentialsCombinedHuddleVtc1Room(dc);
                case "techroom":
                    return new EssentialsTechRoom(dc);
                default:
                    Debug.Console(0, "EssentialsRoomsDeviceFactory: Unknown room type '{0}'", dc.Type);
                    return null;
            }
        }
    }
#else
    /// <summary>
    /// Registers all room types from this plugin with the Essentials v1 DeviceFactory.
    /// Discovered and called automatically by DeviceFactory via reflection on startup.
    /// </summary>
    public class EssentialsRoomsDeviceFactory : IDeviceFactory
    {
        public void LoadTypeFactories()
        {
            DeviceFactory.AddFactoryForType("huddle",               dc => new EssentialsHuddleSpaceRoom(dc));
            DeviceFactory.AddFactoryForType("huddlevtc1",           dc => new EssentialsHuddleVtc1Room(dc));
            DeviceFactory.AddFactoryForType("dualdisplay",          dc => new EssentialsDualDisplayRoom(dc));
            DeviceFactory.AddFactoryForType("combinedhuddlevtc1",   dc => new EssentialsCombinedHuddleVtc1Room(dc));
            DeviceFactory.AddFactoryForType("techroom",             dc => new EssentialsTechRoom(dc));
        }
    }
#endif
}
