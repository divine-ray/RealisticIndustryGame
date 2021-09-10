using UnityEngine;

/// <summary>
/// The bulk of character-related movement and animation.
/// Heavily modified from the standard assets' ThirdPersonCharacter.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerPhysics))]
public class GravityCharacterController : MonoBehaviour
{
	[SerializeField] float m_MovingTurnSpeed = 360;
	[SerializeField] float m_StationaryTurnSpeed = 45;
    [SerializeField] float m_NewtonianTurnSpeed = 2; // For rotation in pseudo-Newtonian space (when no attractors or effectors are nearby)
    [SerializeField] float m_JumpPower = 12f;
    [SerializeField] float m_IncreasedGravity = 1.3f; // By what factor gravity increases when not holding jump in mid-air
    [SerializeField] float m_RunCycleLegOffset = 0.2f; // Specific to the character in sample assets, will need to be modified to work with others
	[SerializeField] float m_RunSpeedMultiplier = 1.25f;
	[SerializeField] float m_GroundCheckDistance = 0.6f; // Higher value leads to more slope acceptance
    [SerializeField] float m_FallCheckDistance = 4f; // For standOnNormals planets, how far the ground is allowed to be under the player before ignoring standOnNormals

    [HideInInspector] public bool m_IsGrounded;
    [HideInInspector] public bool m_Crouching;
    [HideInInspector] public Vector3 m_GroundNormal;

    Rigidbody m_Rigidbody;
	Animator m_Animator;
    PlayerPhysics m_Phys;
    float m_OrigGroundCheckDistance;
	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;
	float m_CapsuleHeight;
	Vector3 m_CapsuleCenter;
	CapsuleCollider m_Capsule;
    GameObject m_prevGravity;


	void Start()
	{
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
        m_Phys = GetComponent<PlayerPhysics>();
        m_Capsule = GetComponent<CapsuleCollider>();
		m_CapsuleHeight = m_Capsule.height;
		m_CapsuleCenter = m_Capsule.center;

		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		m_OrigGroundCheckDistance = m_GroundCheckDistance;
	}


	public void Move(Vector3 move, bool crouch, bool jump)
    {
        if (move.magnitude > 1f) move.Normalize();
        CheckGroundStatus();

        m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;

		ApplyExtraTurnRotation();

        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
			HandleGroundedMovement(crouch, jump);
		else
			HandleAirborneMovement(move.x);

        ScaleCapsuleForCrouching(crouch);
		PreventStandingInLowHeadroom();

		// send input and other state parameters to the animator
		UpdateAnimator(move);
	}


	void ScaleCapsuleForCrouching(bool crouch)
	{
		if (m_IsGrounded && crouch)
		{
			if (m_Crouching) return;
			m_Capsule.height = m_Capsule.height / 2f;
			m_Capsule.center = m_Capsule.center / 2f;
			m_Crouching = true;
		}
		else
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + transform.up * m_Capsule.radius * k_Half, transform.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
				return;
			}
			m_Capsule.height = m_CapsuleHeight;
			m_Capsule.center = m_CapsuleCenter;
			m_Crouching = false;
		}
	}

	void PreventStandingInLowHeadroom()
	{
		// prevent standing up in crouch-only zones
		if (!m_Crouching)
		{
			Ray crouchRay = new Ray(m_Rigidbody.position + transform.up * m_Capsule.radius * k_Half, transform.up);
			float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
			if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				m_Crouching = true;
			}
		}
	}


	void UpdateAnimator(Vector3 move)
	{
		// update the animator parameters
		m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
		m_Animator.SetBool("Crouch", m_Crouching);
		m_Animator.SetBool("OnGround", m_IsGrounded);
		if (!m_IsGrounded)
		{
			m_Animator.SetFloat("Jump", Vector3.Dot(m_Rigidbody.velocity, transform.up));
		}

		// calculate which leg is behind, so as to leave that leg trailing in the jump animation
		// (This code is reliant on the specific run cycle offset in our animations,
		// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
		float runCycle =
			Mathf.Repeat(
				m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
		float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
		if (m_IsGrounded)
		{
			m_Animator.SetFloat("JumpLeg", jumpLeg);
		}

		// the anim speed multiplier affects the movement speed because of the root motion,
        // it's used only for a very simple run here
		if (m_IsGrounded && move.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
		{
			m_Animator.speed = m_RunSpeedMultiplier;
		}
		else
		{
			// don't use that while airborne
			m_Animator.speed = 1;
		}
	}


	void HandleAirborneMovement(float turn)
    {
        m_GroundCheckDistance = m_OrigGroundCheckDistance;

        if (!(m_Phys.effector || m_Phys.attractor))
        {
            // Rotation outside of direct gravitational influence
            transform.Rotate(-transform.forward * turn * m_NewtonianTurnSpeed, Space.Self);
        }
        else if (!Input.GetButton("Jump"))
        {
            // Increase force of gravity if player does not hold down the jump button
            ExtraGravity(m_IncreasedGravity);
        }
    }

    /// <summary>
    /// Gives an additional pull of gravity and lowers lateral velocity.
    /// </summary>
    private void ExtraGravity(float factor)
    {
        Vector3 extraGravityForce;
        if (m_Phys.effector)
            extraGravityForce = Time.fixedDeltaTime * m_Phys.effector.gravity;
        else if (m_Phys.attractor)
            extraGravityForce = Time.fixedDeltaTime * m_Phys.attractor.gravity * Vector3.Normalize(m_Phys.attractor.gameObject.transform.position - transform.position);
        else
            return;

        m_Rigidbody.velocity *= (1 - factor) + (factor * Mathf.Abs(Vector3.Dot(m_Rigidbody.velocity.normalized, extraGravityForce.normalized)));
        m_Rigidbody.AddForce(factor * extraGravityForce);
    }

    void HandleGroundedMovement(bool crouch, bool jump)
	{
        // check whether conditions are right to allow a jump:
        if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump (adds a bit of forward velocity as well if the player was moving)
            m_Rigidbody.velocity = m_Rigidbody.velocity
                + transform.up * m_JumpPower
                + transform.forward * m_ForwardAmount * 5;
            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            m_GroundCheckDistance = 0.1f;
        }
        else // Slow down the rigidbody if grounded
            m_Rigidbody.velocity = m_Rigidbody.velocity * 0.8f;
    }

    void ApplyExtraTurnRotation()
	{
		// help the character turn faster (this is in addition to root rotation in the animation)
		float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0, Space.Self);
	}

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        // Draws the CheckGroundStatus SphereCast in the inspector (gizmos need to be enabled to see it)
        // It displays the endpoint of the SphereCast, not the full cast!
        float radius = 0.3f; // Set this to the collider capsule radius if you change the capsule
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.up * radius) - (transform.up * m_GroundCheckDistance), radius - 0.05f);
    }
#endif

    void CheckGroundStatus()
	{
        float verticalSpeed = Vector3.Dot(m_Rigidbody.velocity, transform.up); // Only check ground if not moving upwards
		RaycastHit hitInfo;
        // Casts a sphere along a ray for a ground collision check.
        // The sphere is 0.05f smaller than the collider capsule and starts at the bottom point of the capsule, giving it a 0.1f offset for safety.
		if ((verticalSpeed <= 0.85f * m_JumpPower) && Physics.SphereCast(transform.position + (transform.up * m_Capsule.radius), m_Capsule.radius - 0.05f, -transform.up, out hitInfo, m_GroundCheckDistance))
        {
            m_IsGrounded = true;
			m_Animator.applyRootMotion = true;
            m_GroundNormal = hitInfo.normal;
        }
		else
		{
            // Airborne
			m_IsGrounded = false;
			m_Animator.applyRootMotion = false;
            
            if (m_Phys.effector) // Effector: set ground normal to direction of gravity
                m_GroundNormal = Vector3.Normalize(m_Phys.effector.gravity);
            else if (m_Phys.attractor) // Attractor:
            {
                if (m_prevGravity != m_Phys.attractor.gameObject) // After switching planets, set ground normal to direction of gravity
                    GetGroundNormalFromGravity();
                else if (m_Phys.attractor.standOnNormals && // On StandOnNormals planets...
                    ((!Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, m_FallCheckDistance)) || // If ground is not under player
                            (Vector3.Angle(hitInfo.normal, transform.up) > 45))) // or at too steep of a slope
                    GetGroundNormalFromGravity(); // Make sure that the player does not fall off the planet
            }
        }

        // Update the previous source of gravity
        if (m_Phys.effector)
            m_prevGravity = m_Phys.effector.gameObject;
        else if (m_Phys.attractor)
            m_prevGravity = m_Phys.attractor.gameObject;
	}

    private void GetGroundNormalFromGravity()
    {
        m_GroundNormal = Vector3.Normalize(transform.position - m_Phys.attractor.gameObject.transform.position);
    }
}
