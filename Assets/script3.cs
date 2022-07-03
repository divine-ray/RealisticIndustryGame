using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script3 : MonoBehaviour
{

    public Script1 ScriptOnCamOne;


    // Start is called before the first frame update
    void Start()
    {
        ScriptOnCamOne = GetComponent<Script1>();
    }

    // Update is called once per frame
    void Update()
    {
        ScriptOnCamOne.something();
        var go = ScriptOnCamOne.gameObject;
    }
}
