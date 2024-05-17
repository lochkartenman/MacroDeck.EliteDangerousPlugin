using System;

namespace Lochkartenman.EliteDangerous
{
    internal class EliteStatusJson {
        public DateTimeOffset Timestamp = DateTimeOffset.MinValue;
        public string Event = string.Empty;
        public int Flags = 0;
        public int Flags2 = 0;
        public int[] Pips = [0, 0, 0];
        public int GuiFocus = 0;
        public int FireGroup = 0;
    }
}