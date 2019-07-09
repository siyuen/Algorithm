using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch {

    private float startTime;
    public StopWatch()
    {
        startTime = Time.time;
    }

    public float ElapsedTime()
    {
        float curTime = Time.time;
        return curTime - startTime;
    }
}
