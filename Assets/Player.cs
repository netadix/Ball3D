using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson {
[RequireComponent(typeof (ThirdPersonCharacter))]
public class Player : MonoBehaviour {

    public GameObject Missile;

    private bool previousKeyState;
    private bool currentKeyState;
    private bool currentRightKeyState;
    private bool currentLeftKeyState;

    // Use this for initialization
    void Start () {
		StartX ();
        VariableCollection.TheEnd = false;
        previousKeyState = false;
        currentKeyState = false;
        currentRightKeyState = false;
        currentLeftKeyState = false;
    }

    // Update is called once per frame
    void Update () {
		UpdateX ();
		FixedUpdateX ();
        float force = 4.0f;
        Vector3 currentPlayerPosition = this.gameObject.transform.position;
        //currentPlayerPosition.z = 8.0f;
        this.gameObject.transform.position = currentPlayerPosition;

        Rigidbody rigid = GetComponent<Rigidbody>();
		if (Input.GetKey(KeyCode.RightArrow))
		{
			rigid.AddForce(new Vector3(force, 0f, 0f), ForceMode.Impulse);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			rigid.AddForce(new Vector3(-force, 0f, 0f), ForceMode.Impulse);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			rigid.AddForce(new Vector3(0f, 0f, -force), ForceMode.Impulse);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			rigid.AddForce(new Vector3(0f, 0f, force), ForceMode.Impulse);
		}

        previousKeyState = currentKeyState;
        currentKeyState = Input.GetKey(KeyCode.Z);
        if (currentKeyState & !previousKeyState)
        {
            VariableCollection.audioClip = Resources.Load<AudioClip>("bullet_whizzing");
            VariableCollection.AudioPlayFlag = true;

            GameObject bullets = GameObject.Instantiate(Missile) as GameObject;
            float horizontalSpeed = 0f;
            if (currentLeftKeyState)
            {
                horizontalSpeed = -2f;
                bullets.transform.rotation = Quaternion.Euler(-20f, 0f, 0f);
            }
            if (currentRightKeyState)
            {
                horizontalSpeed = 2f;
                bullets.transform.rotation = Quaternion.Euler(20f, 0f, 0f);
            }
            Rigidbody rigidbullets = bullets.GetComponent<Rigidbody>();
            rigidbullets.AddForce(new Vector3(horizontalSpeed, 15f, 0f), ForceMode.Impulse);


            // Rigidbodyに力を加えて発射
            // 弾丸の位置を調整
            currentPlayerPosition = transform.position;
            currentPlayerPosition.y += 4.0f;
            bullets.transform.position = currentPlayerPosition;
            bullets.SetActive(true);
        }
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

    }
    void OnCollisionEnter(Collision col)
    {
        string objectName = col.gameObject.name;
        //if (col.gameObject.name.Length >= 6)
        //{
        //    if (objectName.Substring(0, 6) == "X_Left")
        //    {
        //        transform.position = new Vector3(WallLeft.transform.position.x, playerYAxis, playerZAxis);
        //    }
        //    else if (objectName.Substring(0, 6) == "X_Right")
        //    {
        //        transform.position = new Vector3(WallRight.transform.position.x, playerYAxis, playerZAxis);
        //    }
        //}
        if (col.gameObject.name.Length >= 10)
        {
            if (col.gameObject.name.Substring(0, 10) == "RollerBall")     // ball
            {
                VariableCollection.TheEnd = true;
            }
        }

    }


	private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
	private Transform m_Cam;                  // A reference to the main camera in the scenes transform
	private Vector3 m_CamForward;             // The current forward direction of the camera
	private Vector3 m_Move;
	private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

	private void StartX()
	{
		// get the transform of the main camera
		if (Camera.main != null)
		{
			m_Cam = Camera.main.transform;
		}
		else
		{
			//Debug.LogWarning(
			//	"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		// get the third person character ( this should never be null due to require component )
		m_Character = GetComponent<ThirdPersonCharacter>();
	}


	private void UpdateX()
	{
		if (!m_Jump)
		{
			m_Jump = true;//CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}


	// Fixed update is called in sync with physics
	private void FixedUpdateX()
	{
		// read inputs
		float h = 5f;//CrossPlatformInputManager.GetAxis("Horizontal");
		float v = 7f;//CrossPlatformInputManager.GetAxis("Vertical");
		bool crouch = Input.GetKey(KeyCode.C);

		// calculate move direction to pass to character
		if (m_Cam != null)
		{
			// calculate camera relative direction to move:
			m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
			m_Move = v*m_CamForward + h*m_Cam.right;
		}
		else
		{
			// we use world-relative directions in the case of no main camera
			m_Move = v*Vector3.forward + h*Vector3.right;
		}
		#if !MOBILE_INPUT
		// walk speed multiplier
		if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
		#endif

		// pass all parameters to the character control script
		m_Character.Move(m_Move, crouch, m_Jump);
		m_Jump = false;
	}
}

}

