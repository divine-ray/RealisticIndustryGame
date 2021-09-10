using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script2
{
    public float localTemp;
    public void Test()
    {
        Debug.Log("hello world");

    }

    public float Temp()
    {
       return localTemp++;
    }

}

