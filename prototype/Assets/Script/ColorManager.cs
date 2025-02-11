using UnityEngine;

public static class ColorManager
{
    public static Color GetColor(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Red: return Color.red;
            case ColorType.Blue: return Color.blue;
            case ColorType.Green: return Color.green;
            case ColorType.Yellow: return Color.yellow;
            default: return Color.white;
        }
    }
}