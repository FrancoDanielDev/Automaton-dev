using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MyCoroutines
{
    private static Dictionary<UnityAction, Coroutine> _runningCoroutines = new();

    public static void StartDelayedMethod(this MonoBehaviour mono, UnityAction method, float delay)
    {
        _runningCoroutines[method] = mono.StartCoroutine(ExecuteAction(method, delay));
    }

    public static void StopDelayedMethod(this MonoBehaviour mono, UnityAction method)
    {
        if (_runningCoroutines.ContainsKey(method))
        {
            Coroutine coroutine = _runningCoroutines[method];
            mono.StopCoroutine(coroutine);
            _runningCoroutines.Remove(method);
        }
    }

    public static void StopAllMyDelayedMethods(this MonoBehaviour mono)
    {
        mono.StopAllCoroutines();
        _runningCoroutines.Clear();
    }

    private static IEnumerator ExecuteAction(UnityAction action, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        action.Invoke();
    }
}
