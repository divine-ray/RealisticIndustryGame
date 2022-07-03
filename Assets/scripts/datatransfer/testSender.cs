using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace elements
{
    public class testSender : MonoBehaviour
    {
        public int number = 0;
        public bool signal = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            number++;

            if (number >= 50)
            {
                number = 0;

                signal = !signal;
            }
        }


    } //class
} //namespace