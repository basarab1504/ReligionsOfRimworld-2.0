using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace ReligionsOfRimworld
{
    public class Pawn_ReligionCompability
    {
        private Pawn pawn;
        private Dictionary<Religion, float> compabilities;

        public Pawn_ReligionCompability(Pawn pawn)
        {
            this.pawn = pawn;
            compabilities = new Dictionary<Religion, float>();
            RecalculateCompabilities();
        }

        public IEnumerable<KeyValuePair<Religion, float>> Compabilities => compabilities;

        public void RecalculateCompabilities()
        {
            compabilities.Clear();
            foreach (Religion religion in ReligionManager.GetReligionManager().AllReligions)
            {
                compabilities.Add(religion, CalculateCompabilityForReligion(religion.JoiningRestrictionsSettings));
            }
        }

        public Religion MostSuitableReligion()
        {
            return compabilities.RandomElementByWeight(x => x.Value).Key;
        }

        private float CalculateCompabilityForReligion(ReligionSettings_JoiningRestriction settings)
        {
            float currentCompability = 1f;

            foreach (ReligionPermission permission in settings.Permissions)
            {
                currentCompability *= (1 - permission.PermissionRate(pawn));
            }
            return currentCompability;
        }

        public float CompabilityFor(Religion religion)
        {
            return compabilities[religion];
        }
    }
}
