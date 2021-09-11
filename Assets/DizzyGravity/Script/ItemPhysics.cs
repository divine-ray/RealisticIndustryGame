using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ItemPhysics : MonoBehaviour
{
    [SerializeField, Tooltip("How long to wait after a gravity switch before allowing new change.")]
    private float gravSwitchTimout = 0.5f;
    [SerializeField, Tooltip("When outside of gravity zones, how strong gravity should be.")]
    private float newtonGravityScale = 50f;

    public Effector effector; // Active effector (overrides active attractor)
    public Attractor attractor; // Active attractor
    private Rigidbody rb;
    private List<Effector> insideEffectors; // List of effectors the item is inside of
    private float gravSwitchTime; // Time at which next gravity change is allowed
    private bool zombieEffector = false; // A "zombie" effector is still active even though the player has left its trigger (bc there is no other source of gravity)

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        insideEffectors = new List<Effector>();
    }

    private void FixedUpdate()
    {
        if (attractor)
        {
            // Leaving outer attractor bounds
            if ((attractor.gameObject.transform.position - transform.position).sqrMagnitude > attractor.maxSqrDistance)
                attractor = null;
        }

        Gravity();
    }

    private void Gravity() // Note that it's possible to have no active source of gravity.
    {
        if (effector) // Active effector
        {
            rb.AddForce(Time.fixedDeltaTime * effector.gravity);
        }
        else if (attractor) // Active attractor
        {
            Vector3 gravityDirection = Vector3.Normalize(attractor.gameObject.transform.position - transform.position);
            rb.AddForce(Time.fixedDeltaTime * attractor.gravity * gravityDirection);
        }
        else // No active source of gravity; using pseudo-Newtonian physics
        {
            foreach (Attractor a in AttractorList.list)
            {
                Vector3 distance = (a.gameObject.transform.position - transform.position);
                Vector3 pull = newtonGravityScale * a.gravity * distance.normalized / (distance.sqrMagnitude * distance.sqrMagnitude);
                rb.AddForce(pull);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Effector": // Note that effectors can switch instantly after a switch; otherwise they can be easily skipped past
                if ((attractor) && Time.time < gravSwitchTime)
                {
                    insideEffectors.Add(other.gameObject.GetComponent<Effector>());
                    break;
                }
                gravSwitchTime = Time.time + gravSwitchTimout;

                if (zombieEffector)
                    RemoveZombieEffector();
                attractor = null; // Note that effectors cancel attractors, but not the other way around

                // Set new effector as active
                Effector e = other.gameObject.GetComponent<Effector>();
                if (!insideEffectors.Contains(e)) insideEffectors.Add(e);
                effector = e;

                break;
            case "Attractor":
                if ((attractor || (effector && !zombieEffector)) && Time.time < gravSwitchTime)
                    break;
                gravSwitchTime = Time.time + gravSwitchTimout;

                // Set new attractor as active
                Attractor a = other.gameObject.GetComponent<Attractor>();
                attractor = a;
                if (zombieEffector)
                    RemoveZombieEffector();

                break;
            case "SpaceDeath":
                // InstaDeath zones
                DieInSpace("entering instaDeath zone " + other.gameObject);

                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "Effector":
                if (insideEffectors.Count > 1)
                {
                    // If this is not the only effector the player is in,
                    // just remove this one & set the next one active if necessary
                    Effector e = other.gameObject.GetComponent<Effector>();
                    insideEffectors.Remove(e);
                    if (e = effector)
                        effector = insideEffectors[0];
                }
                else // If this is the only one, it's more complicated
                    LeaveEffector();

                break;
            case "Attractor":
                if (!attractor || insideEffectors.Count == 0)
                    break;
                effector = insideEffectors[0];
                attractor = null;
                break;
            case "LevelArea":
                // Leaving the level area: instaDeath
                // You don't need to set up a level area, but it's recommended in case a player flies off
                DieInSpace("leaving level area");

                break;
        }
    }

    /// <summary>
    /// Called if leaving an effector and there are no others to switch to.
    /// Checks whether any attractors are in range to switch to.
    /// Switches to the best attractor in range (depending on distance and gravity) or
    /// leaves this effector active and marks it as a zombieEffector (which is instantly overruled by any new source of gravity).
    /// </summary>
    private void LeaveEffector()
    {
        float bestScore = 0;
        if (!attractor)
            foreach (Attractor a in AttractorList.list)
            {
                // Check each attractor to see if any are in range, set the best one as active
                float distance = (transform.position - a.gameObject.transform.position).sqrMagnitude;
                if (distance < a.maxSqrDistance)
                {
                    float score = a.gravity / distance;
                    if (score > bestScore)
                        attractor = a;
                }
            }

        if (attractor)
        {
            insideEffectors.Remove(effector);
            effector = null;
            zombieEffector = false;
        }
        else
            zombieEffector = true;
    }

    private void RemoveZombieEffector()
    {
        insideEffectors.RemoveAt(0);
        effector = null;
        zombieEffector = false;
    }

    /// <summary>
    /// Put your code for dying by leaving the level bounds here.
    /// For simplicity, this is also the instaDeath and reset function; change that in OnTriggerEnter and FixedUpdate.
    /// </summary>
    /// <param name="source">Reason for the death. Useful for debugging.</param>
    void DieInSpace(string source)
    {
        Debug.Log("Destroying item " + gameObject + " for " + source);
        Destroy(gameObject);
    }
}
