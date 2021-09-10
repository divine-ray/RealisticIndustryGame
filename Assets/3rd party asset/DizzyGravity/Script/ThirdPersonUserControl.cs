using UnityEngine;

/// <summary>
/// The input that is forwarded to GravityCharacterController.
/// Only slightly modified from the standard assets' ThirdPersonUserControl:
/// Not using CrossPlatformInput & not creating a movement vector relative to camera view.
/// </summary>
[RequireComponent(typeof (GravityCharacterController))]
public class ThirdPersonUserControl : MonoBehaviour
{
    private GravityCharacterController m_Character; // A reference to the ThirdPersonCharacter on the object
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

        
    private void Start()
    {
        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<GravityCharacterController>();
    }


    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // movement vector is not relative to camera position
        m_Move = v * Vector3.forward + h * Vector3.right;

        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump);
        m_Jump = false;
    }
}