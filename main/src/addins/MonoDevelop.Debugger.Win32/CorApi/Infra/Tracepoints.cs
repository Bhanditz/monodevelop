using System;

namespace Microsoft.Samples.Debugging.CorMetadata.NativeApi.Infra
{
    public static class Tracepoints
    {
        public static Say New(string prefix)
        {
            double fLast = 0;
            return mf =>
            {
                if(mf.HasValue)
                    fLast = mf.Value;
                Console.Error.WriteLine($"{prefix} {fLast++}");
            };
        }

        public static Say None(string prefix)
        {
            return value => { };
        }

        public delegate void Say(double? value = null);
    }
}