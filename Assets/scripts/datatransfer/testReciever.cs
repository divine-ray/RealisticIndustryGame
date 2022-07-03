using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace elements
{
    public class testReciever : MonoBehaviour
    {
        private Light lamp;
        public int readNumber;

        testSender sender;
        bool connected = false;

        private void Awake() => lamp = GetComponent<Light>();

        private void OnCollisionEnter(Collision other)
        {
            connected = true;

            if (sender == null) other.gameObject.TryGetComponent<testSender>(out sender);

            //other.gameObject.TryGetComponent<testSender>(out testSender sender);
            //other.gameObject.TryGetComponent<testSender>(out testSender boo);


                //if (sender != null) readNumber = sender.number;

        }

        private void OnCollisionExit(Collision collision) => connected = false;

        private void Update()
        {
            if (connected && sender != null) readNumber = sender.number;

   
            ToggleLamp();
        }

        private void ToggleLamp()
        {
            if (readNumber < 25) lamp.enabled = true; 
            else lamp.enabled = false;

        }


    } //class
} //namespace
