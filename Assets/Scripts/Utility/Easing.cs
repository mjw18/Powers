using UnityEngine;
using System.Collections;

public class Easing
{
    public float EaseIn(float a, float b, float t, EasingType easeType)
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
            case (EasingType.Quadric):
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
    public float EaseOut(float a, float b, float t, EasingType easeType)
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
            case (EasingType.Quadric):
                return PolynomialEaseOut(a, b, t, 4);
            case (EasingType.Quintic):
                return PolynomialEaseOut(a, b, t, 5);
            case (EasingType.Sin):
                return SinEaseOut(a, b, t, 0.5f);
        }
        //Return initial value for invalid easeType
        return a;
    }

    public float EaseInOut(float a, float b, float t, EasingType easeType)
    {
        t = Mathf.Clamp(t, 0, 1);

        if(t == 1)
        {
            return b;
        }

        switch (easeType)
        {
            case EasingType.Linear:
                return (1 - t) * a + t * b;
            case EasingType.Quadratic:
                return PolynomialEaseInOut(a, b, t, 2);
            case (EasingType.Cubic):
                return PolynomialEaseInOut(a, b, t, 3);
            case (EasingType.Quadric):
                return PolynomialEaseInOut(a, b, t, 4);
            case (EasingType.Quintic):
                return PolynomialEaseInOut(a, b, t, 5);
            case (EasingType.Sin):
                return SinEaseInOut(a, b, t, 0.5f);
        }
        //Return initial value for invalid easeType
        return a;
    }
	
    private float PolynomialEaseIn(float a, float b, float t, int degree = 0)
    {
        return (1 - Mathf.Pow(t, degree)) * a + Mathf.Pow(t, degree) * b;
    }

    private float PolynomialEaseOut(float a, float b, float t, int degree = 0)
    {
        return ((b - a) * Mathf.Pow(1 - t, degree) + b);
    }

    private float PolynomialEaseInOut(float a, float b, float t, int degree = 0)
    {
        float startVal = PolynomialEaseIn(1, a, (b - a) / 2, degree);

        if (t <= 0.5f)
            return PolynomialEaseIn(a, b, t, degree);
        else
            return PolynomialEaseOut(2 * t - 1, startVal, b, degree);
    }

    private float SinEaseIn(float a, float b, float t, float period)
    {
        float i = Mathf.Cos(t * Mathf.PI * period);
        return -(b - a) * i +  b;
    }

    private float SinEaseOut(float a, float b, float t, float period)
    {
        float i = Mathf.Sin( (t + 1) * Mathf.PI * period);
        return (1 - i) * b + i * a;
    }

    private float SinEaseInOut(float a, float b, float t, float period)
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
    Quadric,
    Quintic,
    Sin
}

