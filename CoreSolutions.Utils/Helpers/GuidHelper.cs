using System;

namespace CoreSolutions.Utils.Helpers
{
    public class GuidHelper
    {
        public static Guid ToGuid(long value)
        {
            var bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static long FromGuid(Guid value)
        {
            return BitConverter.ToInt64(value.ToByteArray(), 0);
        }
    }
}
