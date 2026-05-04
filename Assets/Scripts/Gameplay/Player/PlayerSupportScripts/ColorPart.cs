using System;
using UnityEngine;

public enum ColorPart
{
    R,
    G,
    B
}

public static class ColorPartExtensions
{
    public static ColorPart Next(this ColorPart part)
    {
        switch (part)
        {
            case ColorPart.R:
                return ColorPart.G;
            case ColorPart.G:
                return ColorPart.B;
            case ColorPart.B:
                return ColorPart.R;
        }

        throw new ArgumentException("Invalid ColorPart");
    }

    public static float GetColorPart(this Color color, ColorPart part)
    {
        switch (part)
        {
            case ColorPart.R:
                return color.r;
            case ColorPart.G:
                return color.g;
            case ColorPart.B:
                return color.b;
        }

        throw new ArgumentException("Invalid ColorPart");
    }

    public static Color Increase(this Color color, ColorPart part, float amount)
    {
        switch (part)
        {
            case ColorPart.R:
                color.r = IncreaseColorPart(color.r, amount);
                break;
            case ColorPart.G:
                color.g = IncreaseColorPart(color.g, amount);
                break;
            case ColorPart.B:
                color.b = IncreaseColorPart(color.b, amount);
                break;
        }

        return color;
    }

    private static float IncreaseColorPart(float colorPartValue, float amount)
    {
        colorPartValue += amount;

        if (colorPartValue < 0)
        {
            colorPartValue = 0;
        }
        if (colorPartValue > 1)
        {
            colorPartValue = 1;
        }

        return colorPartValue;
    }
}
