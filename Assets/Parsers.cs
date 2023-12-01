using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorPick
{
    public static Color ToColor(this string color)
    {
        return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
    }
}
public static class Converter
{
    public static float ToFloat(this string number)
    {
        return float.Parse(number);
    }
    public static int ToInt(this string number)
    {
        return Int32.Parse(number);
    }
    public static byte ToByte(this string number)
    {
        return Byte.Parse(number);
    }
}