using UnityEngine;
using System.Collections;

public static class Easing
{
    public delegate float EasingFunction(float a, float b, float t, EasingType e);
    public delegate Vector3 VectorEasingFunction(Vector3 a, Vector3 b, float t, EasingType e);

    public static EasingFunction EaseIn = EaseInFloat;
    public static EasingFunction EaseOut = EaseOutFloat;
    public static EasingFunction EaseInOut = EaseInOutFloat;

    public static VectorEasingFunction VectorEaseIn = EaseInVector;
    public static VectorEasingFunction VectorEaseOut = EaseOutVector;
    public static VectorEasingFunction VectorEaseInOut = EaseInOutVector;

    public static float EaseInFloat(float a, float b, float t, EasingType easeType)
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
    public static float EaseOutFloat(float a, float b, float t, EasingType easeType)
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

    public static float EaseInOutFloat(float a, float b, float t, EasingType easeType)
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
	
    public static Vector3 EaseInVector(Vector3 a, Vector3 b, float t, EasingType easeType)
    {
        if (a.Equals(b)) return b;

        Vector3 temp = a;
        temp.x = EaseIn(a.x, b.x, t, easeType);
        temp.y = EaseIn(a.y, b.y, t, easeType);
        temp.z = EaseIn(a.z, b.z, t, easeType);

        return temp;
    }

    public static Vector3 EaseOutVector(Vector3 a, Vector3 b, float t, EasingType easeType)
    {
        if (a.Equals(b)) return b;

        Vector3 temp = a;
        temp.x = EaseOut(a.x, b.x, t, easeType);
        temp.y = EaseOut(a.y, b.y, t, easeType);
        temp.z = EaseOut(a.z, b.z, t, easeType);

        return temp;
    }

    public static Vector3 EaseInOutVector(Vector3 a, Vector3 b, float t, EasingType easeType)
    {
        if (a.Equals(b)) return b;

        Vector3 temp = a;
        temp.x = EaseInOut(a.x, b.x, t, easeType);
        temp.y = EaseInOut(a.y, b.y, t, easeType);
        temp.z = EaseInOut(a.z, b.z, t, easeType);

        return temp;
    }

    //Get interpolated float with given ease type and easing function
    public static float InterpolateFloat(float a, float b, float t, EasingType easeType, EasingFunction easeFunction)
    {
        return easeFunction(a, b, t, easeType);
    }

    //Get interpolated Vector3 with given ease type and easing function
    public static Vector3 InterpolateVector(Vector3 a, Vector3 b, float t, EasingType easeType, VectorEasingFunction easeFunction)
    {
        return easeFunction(a, b, t, easeType);
    }

    //Not really useful until returns are a thing
    public static IEnumerator CoEase(float a, float b, float duration, EasingType easeType, EasingFunction easeFunction)
    {
        float rate = 1 / duration;
        float t = 0;
        float outFloat = 1.0f;

        while (t <= 1)
        {
            outFloat = easeFunction(a, b, t, easeType);
            t += rate;
            yield return outFloat;
        }

        yield return outFloat;
    }

    //Not really useful until returns are a thing
    public static IEnumerator MoveObjectToTarget(Transform a, Transform b, float duration, EasingType easeType, VectorEasingFunction easeFunction)
    {
        float rate = 1 / duration;
        float t = 0;
        Vector3 startPos = a.position;

        while (t <= 1)
        {
            a.position = easeFunction(startPos, b.position, t, easeType);
            t += rate;
            yield return a;
        }

        yield return null;
    }

    private static float PolynomialEaseIn(float a, float b, float t, int degree)
    {
        return (1 - Mathf.Pow(t, degree)) * a + Mathf.Pow(t, degree) * b;
    }

    private static float PolynomialEaseOut(float a, float b, float t, int degree)
    {
        return ((a - b) * Mathf.Pow(1 - t, degree) + b);
    }

    private static float PolynomialEaseInOut(float a, float b, float t, int degree)
    {
        float halfInterval = (b - a) / 2;

        if (t <= 0.5f)
            return PolynomialEaseIn(a, halfInterval + a, 2*t, degree);
        else
            return PolynomialEaseOut(halfInterval + a, b, 2 * t - 1, degree);
    }

    private static float SinEaseIn(float a, float b, float t, float period)
    {
        float i = Mathf.Cos(t * Mathf.PI * period);
        return -(b - a) * i +  b;
    }

    private static float SinEaseOut(float a, float b, float t, float period)
    {
        float i = Mathf.Sin( t * Mathf.PI * period);
        return (b - a) * i + a;
    }

    private static float SinEaseInOut(float a, float b, float t, float period)
    {
        float halfInterval = (b - a) / 2;

        if (t <= 0.5f)
            return SinEaseIn(a, halfInterval  + a, 2*t, period);
        else
            return SinEaseOut(halfInterval + a, b, 2 * t - 1, period);
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

