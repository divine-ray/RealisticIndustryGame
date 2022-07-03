using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles most of the physics side for player characters.
/// </summary>
[RequireComponent(typeof(GravityCharacterController))]
public class PlayerPhysics : MonoBehaviour
{
    [SerializeField, Tooltip("Higher number -> faster rotation towards gravity.")]
    private float rotateSpeed = 6f;
    [SerializeField, Tooltip("How long to wait after a gravity switch before allowing new change.")]
    private float gravSwitchTimout = 0.5f;
    [SerializeField, Tooltip("When outside of gravity zones, how strong gravity should be.")]
    private float newtonGravityScale = 10f;

    public Effector effector; // Active effector (overrides active attractor)
    public Attractor attractor; // Active attractor
    private Rigidbody rb;
    private GravityCharacterController characterController;
    private List<Effector> insideEffectors; // List of effectors the player is inside of
    private float gravSwitchTime; // Time at which next gravity change is allowed
    private bool zombieEffector = false; // A "zombie" effector is still active even though the player has left its trigger (bc there is no other source of gravity)

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<GravityCharacterController>();
        insideEffectors = new List<Effector>();
    }

    private void FixedUpdate()
    {
        if (attractor)
        {
            // Standing on rotating ground
            if (characterController.m_IsGrounded)
                transform.RotateAround(attractor.gameObject.transform.position, attractor.rotAxis, attractor.rotAngle);
            
            // Leaving outer attractor bounds
            if ((attractor.gameObject.transform.position - transform.position).sqrMagnitude > attractor.maxSqrDistance)
                attractor = null;
        }

        Gravity();
        RotateTowardsGravity();

        // Panic button, mostly for testing purposes
        if (Input.GetKeyDown(KeyCode.R))
            DieInSpace("pressing R");
    }

    private void RotateTowardsGravity() // Note that it's possible to have no active source of gravity.
    {
        Quaternion prevRotation = transform.rotation;
        Quaternion targetRotation;
        if (effector) // Active effector
        {
            targetRotation = Quaternion.LookRotation(effector.gravity, transform.forward) * Quaternion.AngleAxis(-90f, Vector3.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
            PreventLeavingGravField(prevRotation, targetRotation);
        }
        else if (attractor)
        {
            if (attractor.standOnNormals) // Active attractor with standOnNormals
            {
                targetRotation = Quaternion.LookRotation(-characterController.m_GroundNormal, transform.forward) * Quaternion.AngleAxis(-90f, Vector3.right);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
            }
            else // Active regular attractor
            {
                Vector3 gravityDir = attractor.gameObject.transform.position - transform.position;
                targetRotation = Quaternion.LookRotation(gravityDir, transform.forward) * Quaternion.AngleAxis(-90f, Vector3.right);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
            }
            PreventLeavingGravField(prevRotation, targetRotation);
        }
        else // No active source of gravity; using pseudo-Newtonian physics
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(rb.velocity, transform.up),
                rotateSpeed * Time.fixedDeltaTime);
        }
    }

    private void PreventLeavingGravField(Quaternion prevRotation, Quaternion targetRotation) // Adds extra gravity force if rotating quickly to prevent twitching
    {
        float force = 1 + (Quaternion.Angle(prevRotation, targetRotation) / 45);
        if (force < 1.5f) return;
        
        Vector3 extraGravityForce = Vector3.zero;
        if (effector)
            extraGravityForce = Time.fixedDeltaTime * effector.gravity;
        else if (attractor)
            extraGravityForce = Time.fixedDeltaTime * attractor.gravity * (targetRotation * -Vector3.up);
        rb.AddForce(force * extraGravityForce);
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

            if (attractor.standOnNormals) // StandOnNormals
                rb.AddForce(Time.fixedDeltaTime * attractor.gravity * (-characterController.m_GroundNormal * gravityDirection.magnitude));
            else // Regular
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
        switch(other.tag)
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
                if(!insideEffectors.Contains(e)) insideEffectors.Add(e);
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

    private void RemoveZombieEffector ()
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
        Debug.Log("Resetting player for " + source);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.position = Vector3.zero;
        effector = null;
        insideEffectors.RemoveRange(0, insideEffectors.Count);
        zombieEffector = false;
    }
}
