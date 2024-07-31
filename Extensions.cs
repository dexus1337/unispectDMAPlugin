using System;
using Vmmsharp;

namespace unispectDMAPlugin
{
    internal static class Extensions
    {
        /// <summary>
        /// Custom Memory Read Method.
        /// Does not resize partial reads.
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="va"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static unsafe byte[] MemReadCustom(this VmmProcess proc, ulong va, int cb)
        {
            var buffer = new byte[cb];
            fixed (byte* pb = buffer)
            {
                if (!proc.MemRead(va, pb, (uint)cb, out _))
                    throw new Exception("Memory Read Failed!");
            }
            return buffer;
        }
    }
}
