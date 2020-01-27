using RimWorld.Planet;
using System;
using Verse;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReligionsOfRimworld
{
    public static class ReligionsBuffer
    {
        public static List<Religion> religions = new List<Religion>();
        private static int lastID = 0;

        public static int GetNextID()
        {
            int ID = lastID;
            lastID++;
            return ID;
        }
    }
}
