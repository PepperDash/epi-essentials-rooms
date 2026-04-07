using System.Collections.Generic;
using PepperDash.Essentials.Core;
using PepperDash.Essentials.Core.Config;

namespace PDT.Plugins.Essentials.Rooms
{
    /// <summary>
    /// Base class for rooms with more than a single display
    /// </summary>
    public abstract class EssentialsNDisplayRoomBase : EssentialsRoomBase, IHasMultipleDisplays
    {
        //public event SourceInfoChangeHandler CurrentSingleSourceChange;

#if ESSENTIALS_V2
        public Dictionary<eSourceListItemDestinationTypes, IRoutingSink> Displays { get; protected set;}
#else
        public Dictionary<eSourceListItemDestinationTypes, IRoutingSinkWithSwitching> Displays { get; protected set;}
#endif

        public EssentialsNDisplayRoomBase(DeviceConfig config)
            : base (config)
        {
#if ESSENTIALS_V2
            Displays = new Dictionary<eSourceListItemDestinationTypes, IRoutingSink>();
#else
            Displays = new Dictionary<eSourceListItemDestinationTypes, IRoutingSinkWithSwitching>();
#endif

        }
    }
}