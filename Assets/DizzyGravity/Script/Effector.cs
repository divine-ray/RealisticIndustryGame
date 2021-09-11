using UnityEngine;

/// <summary>
/// An effector is a gravity field in a single direction.
/// Effectors override attractors, but as soon as the player leaves the effector area, it is deactivated.
/// Exception: Leaving an effector area with no attractors in range keeps the effector active until a new source of gravity is found.
/// </summary>
public class Effector : MonoBehaviour {

    [SerializeField, Tooltip("The force of gravity.")] private float gravityMultiplier = 1;
    [SerializeField, Tooltip("Direction in which gravity pulls.")] private Vector3 gravityDirection = Vector3.down;
    [SerializeField, Tooltip("Use local or global XYZ for gravity direction?")] private bool useLocalRotation = true;
    [HideInInspector] public Vector3 gravity;

#if UNITY_EDITOR
    private void Update()
    {
        UpdateGravity();
        // The gravity only updates constantly when running in the editor.
        // If you want to change an attractor's gravity during during the game,
        // either do it by directly modifying "gravity" or make UpdateGravity public and call it whenever necessary.
    }

    void OnDrawGizmos()
    {
        // Draws gravity direction arrows in the inspector (gizmos need to be enabled to see them)
        UpdateGravity();
        Vector3 gravityDir = Vector3.Normalize(gravity);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position - gravityDir);
        Gizmos.DrawRay(transform.position, Quaternion.LookRotation(gravityDir) * Quaternion.Euler(Vector3.up * 210) * Vector3.forward * 0.2f);
        Gizmos.DrawRay(transform.position, Quaternion.LookRotation(gravityDir) * Quaternion.Euler(Vector3.up * 150) * Vector3.forward * 0.2f);
    }
#endif

    private void OnEnable()
    {
        UpdateGravity();
    }

    private void UpdateGravity()
    {
        if (useLocalRotation)
            gravity = Vector3.Normalize(transform.rotation * gravityDirection) * gravityMultiplier * 4000;
        else
            gravity = Vector3.Normalize(gravityDirection) * gravityMultiplier * 4000;
    }
}
