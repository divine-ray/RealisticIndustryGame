using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour
{
   private Rigidbody rb;
   public Vector3 forceVector;
    public bool hasCollided = false;
    public int collisionCount = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }

    private void FixedUpdate()
    {
        
        //i forgot what this was for lmao
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(forceVector, ForceMode.Impulse);
        Test();
        collisionCount++;
        //if (collisionCount >= 5)
        //{
            //Vector3 = new Vector3(0, 0, 0); 
        //    transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        //}

        
    }
    bool Test()
    {
        hasCollided = true;
        Debug.Log($"{this} object collided: {hasCollided}");
        
        return hasCollided;
    }

}
