using UnityEngine;

/// <summary>
/// Add to the camera to make it follow the player.
/// </summary>
public class FollowCam : MonoBehaviour {

    [SerializeField]
    private Transform target;
    private Rigidbody rb;
    [SerializeField, Tooltip("Distance between camera and target.")]
    private float distance = 3f;
    [SerializeField, Tooltip("Height from target base to look at (or from capsule collider if applicable)")]
    private float heightOffset = 0.3f;
    [SerializeField, Tooltip("How much target velocity should be taken into account.\nNote that the target needs a rigidbody for this.")]
    private float velocityMultiplier = 0.2f;
    [SerializeField, Range(0f, 1f), Tooltip("0 = no camera rotation, 1 = instant rotation")]
    private float rotationDamping = 0.2f;
    [SerializeField, Range(0f, 1f), Tooltip("0 = no camera translation, 1 = instant translation")]
    private float translationDamping = 0.2f;

    private Vector3 targetVelocity, targetCenter, focalPoint, positionToBe;
    Quaternion targetDir;
    private CapsuleCollider capsule;
    private float offset;


#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Draws a line showing the focal point in the inspector during play (gizmos need to be enabled to see it)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + targetDir * Vector3.forward * distance);
    }
#endif

    private void Start()
    {
        positionToBe = transform.position;
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        rb = target.GetComponent<Rigidbody>();
        capsule = target.GetComponent<CapsuleCollider>();
    }

    void LateUpdate ()
    {
        if (!target)
            return;

       if (rb)
            targetVelocity = rb.velocity * velocityMultiplier;
        else
            targetVelocity = Vector3.zero;

        if (capsule)
            offset = heightOffset + capsule.height / 2;

        // Position of the target's center
        targetCenter = target.transform.position + target.transform.up * offset;
        // Point the camera is targeting
        focalPoint = targetCenter + targetVelocity;
        // Direction the camera wants to look
        targetDir = Quaternion.LookRotation(focalPoint - (targetCenter - distance * target.transform.forward), target.transform.up);
        // Position the camera wants to be
        positionToBe = Vector3.Lerp(positionToBe, (focalPoint + targetDir * Vector3.forward * -distance), translationDamping);

        // Applies position and rotation
        transform.position = Vector3.Lerp(transform.position, positionToBe, translationDamping);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetDir, rotationDamping);

        // Make
        RaycastHit[] hits = Physics.RaycastAll(transform.position, targetCenter - transform.position, (targetCenter - transform.position).magnitude);
        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer)
            {
                TransparentObscure transparentObscure = renderer.GetComponent<TransparentObscure>();
                if (!transparentObscure)
                    transparentObscure = renderer.gameObject.AddComponent<TransparentObscure>();
                transparentObscure.Hide();
            }
        }
    }
}
