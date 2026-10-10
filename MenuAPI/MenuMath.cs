namespace MenuAPI;

// Math.Clamp and float.IsFinite do not exist on .NET Framework 4.5.2, which the Legacy build targets.
internal static class MenuMath
{
    internal static int Clamp(int value, int min, int max) => value < min ? min : value > max ? max : value;

    internal static float Clamp(float value, float min, float max) => value < min ? min : value > max ? max : value;

    internal static bool IsFinite(float value) => !float.IsNaN(value) && !float.IsInfinity(value);
}
