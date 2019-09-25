using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReligionsOfRimworld
{
    public interface IDescribable
    {
        IEnumerable<ReligionInfoEntry> GetInfoEntries();
    }
}
