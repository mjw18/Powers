using UnityEngine;
using System.Collections;

public static class Easing
{
    public static float EaseIn(float a, float b, float t, EasingType easeType)
    {
        t = Mathf.Clamp(t, 0, 1);

        switch (easeType)
        {
            case EasingType.Linear:
                return (1 - t) * a + t * b;
            case EasingType.Quadratic:
                return PolynomialEaseIn(a, b, t, 2);
            case (EasingType.Cubic):
                return PolynomialEaseIn(a, b, t, 3);
            case (EasingType.Quartic):
                return PolynomialEaseIn(a, b, t, 4);
            case (EasingType.Quintic):
                return PolynomialEaseIn(a, b, t, 5);
            case (EasingType.Sin):
                return SinEaseIn(a, b, t, 0.5f);
        }
        //Return initial value for invalid easeType
        return a;
    }

    //Return linear for invalid easeType
    public static float EaseOut(float a, float b, float t, EasingType easeType)
    {
        t = Mathf.Clamp(t, 0, 1);

        switch (easeType)
        {
            case EasingType.Linear:
                return (1 - t) * a + t * b;
            case EasingType.Quadratic:
                return PolynomialEaseOut(a, b, t, 2);
            case (EasingType.Cubic):
                return PolynomialEaseOut(a, b, t, 3);
            case (EasingType.Quartic):
                return PolynomialEaseOut(a, b, t, 4);
            case (EasingType.Quintic):
                return PolynomialEaseOut(a, b, t, 5);
            case (EasingType.Sin):
                return SinEaseOut(a, b, t, 0.5f);
        }
        //Return initial value for invalid easeType
        return a;
    }

    public static float EaseInOut(float a, float b, float t, EasingType easeType)
    {
        t = Mathf.Clamp(t, 0, 1);

        if(a == b) return b;

        switch (easeType)
        {
            case EasingType.Linear:
                return (1 - t) * a + t * b;
            case EasingType.Quadratic:
                return PolynomialEaseInOut(a, b, t, 2);
            case (EasingType.Cubic):
                return PolynomialEaseInOut(a, b, t, 3);
            case (EasingType.Quartic):
                return PolynomialEaseInOut(a, b, t, 4);
            case (EasingType.Quintic):
                return PolynomialEaseInOut(a, b, t, 5);
            case (EasingType.Sin):
                return SinEaseInOut(a, b, t, 0.5f);
        }
        //Return initial value for invalid easeType
        return a;
    }
	
    private static float PolynomialEaseIn(float a, float b, float t, int degree)
    {
        return (1 - Mathf.Pow(t, degree)) * a + Mathf.Pow(t, degree) * b;
    }

    private static float PolynomialEaseOut(float a, float b, float t, int degree)
    {
        return ((b - a) * Mathf.Pow(1 - t, degree) + a);
    }

    private static float PolynomialEaseInOut(float a, float b, float t, int degree)
    {
        float startVal = PolynomialEaseIn(1, a, (b - a) / 2, degree);

        if (t <= 0.5f)
            return PolynomialEaseIn(a, b, t, degree);
        else
            return PolynomialEaseOut(2 * t - 1, startVal, b, degree);
    }

    private static float SinEaseIn(float a, float b, float t, float period)
    {
        float i = Mathf.Cos(t * Mathf.PI * period);
        return -(b - a) * i +  b;
    }

    private static float SinEaseOut(float a, float b, float t, float period)
    {
        float i = Mathf.Sin( (t + 1) * Mathf.PI * period);
        return (1 - i) * b + i * a;
    }

    private static float SinEaseInOut(float a, float b, float t, float period)
    {
        float startVal = SinEaseIn(1, a, (b - a) / 2, period);

        if (t <= 0.5f)
            return SinEaseIn(a, b, t, period);
        else
            return SinEaseOut(2 * t - 1, startVal, b, period);
    }
}

public enum EasingType
{
    Linear,
    Quadratic,
    Cubic,
    Quartic,
    Quintic,
    Sin
}

