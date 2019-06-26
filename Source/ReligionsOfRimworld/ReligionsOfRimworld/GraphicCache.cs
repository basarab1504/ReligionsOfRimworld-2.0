using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    [StaticConstructorOnStartup]
    internal class GraphicsCache
    {
        public static readonly Texture2D Info = ContentFinder<Texture2D>.Get("UI/Buttons/InfoButton", true);
        public static readonly Texture2D Faith = ContentFinder<Texture2D>.Get("Things/Symbols/Religion", true);
    }
}
