using UnityEngine;
using System.Collections;

public static class MonoBehaviourExt
{
    public static Coroutine<T> StartCoroutine<T>(this MonoBehaviour obj, IEnumerator coroutine)
    {
        Coroutine<T> tempCoroutine = new Coroutine<T>();
        tempCoroutine.coroutine = obj.StartCoroutine(tempCoroutine.InternalRoutine(coroutine));
        return tempCoroutine;
    }
} 

public class Coroutine<T>
{
    private T returnVal;

    public T retVal
    {
        get { return returnVal; }
    }
    public Coroutine coroutine;

    public IEnumerator InternalRoutine(IEnumerator coroutine)
    {
        while(true)
        {
            //If the coroutine has comlpeted execution, end this coroutine
            if(!coroutine.MoveNext())
            {
                Debug.Log("Move next is false");
                yield break;
            }
            //Grab the current object from IEnumerator  
            //Like var for objects
            object yielded = coroutine.Current;

            //If yielded can be cast to type T assign to returnVal after casting and return from coroutine
            if(yielded != null && yielded.GetType() is T)
            {
                returnVal = (T)yielded;
                Debug.Log("The return value was found: " + returnVal);
                yield break;
            }
            else
            {
                Debug.Log("The return value was found, output continuous" + coroutine.Current);
                if(coroutine.Current is T) returnVal = (T)coroutine.Current; 
                yield return coroutine.Current;
            }
        }
    }
}
