using System;
using System.Collections;
using UnityEngine;


//I HATE COROUTINES. This is a wrapper class that simplifies using them that doesn't make me angry.
//It's essentially a fire-and-forget coroutine runner. Starts a worker, checks done when we need to.
//Supportes both IENumerator and regular methods, for maximum compatibility with existing code.
//Multiple workers can run simultaneously.
public class ParallelWorker : MonoBehaviour
{
    //Puling inspiration from Drake. Making a singleton instance.
    private static ParallelWorker _instance;

    private static ParallelWorker Instance
    {
        get
        {
            if(_instance == null)
            {
                var hostObject = new GameObject("[ParallelWorker]");
                DontDestroyOnLoad(hostObject);
                _instance = hostObject.AddComponent<ParallelWorker>();
            }
            return _instance;
        }


    }

    //Publically accessible flags.
    public bool done { get; private set; } = false;
    public bool isRunning { get; private set; } = false;

    private Coroutine _coroutine;


    //Start methods
    //Starts a coroutine, returns a worker handle
    public static ParallelWorker StartParallelWorker(IEnumerator routine)
    {
        var worker = new GameObject("[Worker]").AddComponent<ParallelWorker>();
        worker.transform.SetParent(Instance.transform);
        worker._coroutine = worker.StartCoroutine(worker.Run(routine));
        return worker;
    }

    public static ParallelWorker StartParallelWorker(Action method)
    {
        return StartParallelWorker(Wrapper(method));
    }

    private static IEnumerator Wrapper(Action method)
    {
        method();
        yield return null;
    }


//Internal runner stuff

private IEnumerator Run(IEnumerator routine)
    {
        isRunning = true;
        done = false;

        yield return routine;

        done = true;
        isRunning = false;

        //Self cleanup, done on next frame
        StartCoroutine(SelfDestruct());
    }

private IEnumerator SelfDestruct()
    {
        yield return null;
        if (Application.isPlaying)
        {
            Destroy(gameObject);
        }
    }


//Controls

public void Stop()
    {
        if(_coroutine != null) StopCoroutine(_coroutine);
        done = false;
        isRunning = false;

        Destroy(gameObject);
    }

}
