using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script1 : MonoBehaviour
{
    Script2 script2 = new Script2();

    // Start is called before the first frame update
    void Start()
    {
        script2.Test();
    }

    // Update is called once per frame
    void Update()
    { 
        float x;
        x = script2.Temp();
        Debug.Log(x);
         
    }

    public void something()
    {
        Debug.Log("meow");
    }
}
