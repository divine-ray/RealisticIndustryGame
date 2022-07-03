using UnityEngine;

/// <summary>
/// An attractor acts like a planet.
/// When entering its collider, it pulls the player towards a specified gameObject (default is the parent object).
/// The pull stops once the player exits the maxSqrDistance perimiter (or once a new source of gravity acts on the player).
/// Can rotate using rotAxis and rotAngle; this also moves the player when on the ground.
/// </summary>
public class Attractor : MonoBehaviour {

    [SerializeField, Tooltip("The force of gravity.")] private float gravityMultiplier = 0.9f;
    [HideInInspector] public float gravity = 4000;

    public Vector3 rotAxis;
    public float rotAngle;
    [Tooltip("Turn on if the planet is irregularly shaped and the player should stand orthogonally to the ground.")] public bool standOnNormals;
    [Tooltip("Check the sphere gizmo in the scene view. If the player exits this sphere, the attractor is deactivated.")] public float maxSqrDistance = 200f;
    new public GameObject gameObject;

#if UNITY_EDITOR
    private void Update()
    {
        UpdateGravity();
        // The gravity only updates constantly when running in the editor.
        // If you want to change an attractor's gravity during during the game,
        // either do it by directly modifying "gravity" or make UpdateGravity public and call it whenever necessary.
    }

    void OnDrawGizmosSelected()
    {
        // Draws the maxSqrDistance perimiter in the inspector (gizmos need to be enabled to see it)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(maxSqrDistance));
    }
#endif

    private void UpdateGravity()
    {
        gravity = gravityMultiplier * 4000;
    }

    private void FixedUpdate()
    {
        gameObject.transform.Rotate(rotAxis, rotAngle, Space.World);
    }

    private void OnEnable ()
    {
        UpdateGravity();
        if (!gameObject)
            gameObject = transform.parent.gameObject;

        AttractorList.AddToList(this);
	}

    private void OnDisable ()
    {
        AttractorList.RemoveFromList(this);
    }
}
