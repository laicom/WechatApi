using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nlab.WeChatApi
{
    [Flags]
    public enum MessageTypes
    {
        Text=1,
        Image,
        Location,
        Link,
        Event,
        Music,
        News,
        MenuClick,
        MenuView,
        QRScan,
        Subscribe,
        UnSubscribe,
        Voice,
        Video,

        All = Text | Image | Location | Link | Event | Music | News | MenuClick | MenuView | QRScan | Subscribe | UnSubscribe,
    }
}
