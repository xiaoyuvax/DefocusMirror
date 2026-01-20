namespace DefocusMirror;


public static class BlurCalculator
{
    public const float PupilDiameter = 6.5f;

    public static Dictionary<string, float> LCA { get; set; } = new()
        {
            { "R",-0.23f },
            {"G", 0.24f },
            { "B",1.1f }
        };


    public static Dictionary<string, float> LCARedToFocus { get; set; } = new()
        {
            { "R",0.086923077f },
            {"G", 0.434615385f },
            { "B",1.13f }
        };


    public static float CalculateG(float LCA, float distance) => 1000f / ((1000f / distance) + LCA);



    public static (float R, float G, float B) GetRGBBlurCircleDiameters(float distance)
    {
        var R = GetSimulatedBlurCircleDiameter(LCARedToFocus["R"], distance);
        var G = GetSimulatedBlurCircleDiameter(LCARedToFocus["G"], distance);
        var B = GetSimulatedBlurCircleDiameter(LCARedToFocus["B"], distance);
        return (R, G, B);
    }

    public static float GetSimulatedBlurCircleDiameter(float LCA, float distance)
    {
        var G = CalculateG(LCA, distance);
        return PupilDiameter * (distance - G) / G;
    }
}