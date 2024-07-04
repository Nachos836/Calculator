using System.Runtime.InteropServices;

namespace Calc.Domain.Functional.Outcome
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public readonly struct Unit
    {
        public static readonly Unit Instance = default;
    }
}
