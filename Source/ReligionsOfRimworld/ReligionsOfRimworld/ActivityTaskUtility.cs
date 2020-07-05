using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReligionsOfRimworld
{
    public static class ActivityTaskUtility
    {
        private static ScheduledDay clipboard;

        public static ScheduledDay Clipboard
        {
            get
            {
                if (clipboard == null)
                    return null;
                return new ScheduledDay(clipboard);
            }
            set
            {
                clipboard = value;
            }
        }
    }
}
