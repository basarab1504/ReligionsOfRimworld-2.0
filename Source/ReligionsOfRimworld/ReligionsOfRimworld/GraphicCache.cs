using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    [StaticConstructorOnStartup]
    internal class GraphicsCache
    {
        public static readonly Texture2D Info = ContentFinder<Texture2D>.Get("UI/Buttons/InfoButton", true);
        public static readonly Texture2D Faith = ContentFinder<Texture2D>.Get("Things/Symbols/Religion", true);
        public static readonly Texture2D Paste = ContentFinder<Texture2D>.Get("UI/Buttons/Paste", true);
        public static readonly Texture2D DeleteX = ContentFinder<Texture2D>.Get("UI/Buttons/Delete", true);
        public static readonly Texture2D Suspend = ContentFinder<Texture2D>.Get("UI/Buttons/Suspend", true);
        public static readonly Texture2D Copy = ContentFinder<Texture2D>.Get("UI/Buttons/Copy", true);
    }
}
