using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputManager
{
    public string InputName;


    public bool CheckForInputPressed()
    {
        return Input.GetButtonDown(InputName);
    }
    public bool CheckForInputHeld()
    {
        return Input.GetButton(InputName);
    }
    public float JoystickPosition()
    {
        return Input.GetAxisRaw(InputName);
    }
}



public class CharacterController : MonoBehaviour
{
    GameManager Manager;
    [Range(1,2)]
    public int CurrentPlayerNumber;
    public GameObject FollowingParent;
    public ParticleSystem CollideParticle;


    [Header("Controls")]
    public InputManager Horizontal;
    public InputManager Vertical;

    public InputManager Boost;
    public InputManager Jump;


    [Header("Collision Forces")]
    public float CollisionForce = 1;
    public float PushUpForce = 50;

    [Header("Movement Variables")]
    public float PushForce = 2;
    public float MaxVelocity = 2;

    public float BoostPush = 2;
    public float JumpHeight = 2;

    [HideInInspector]
    public Rigidbody RB;
    Camera MainCamera;
    SpringJoint MySpring;

    static GameObject ForwardBase;
    bool Boosting;
    float StartingBoostTimer;

    float BoostCooldown = 0;

    void Awake()
    {
        Manager = GameManager.FindManager();

        RB = GetComponent<Rigidbody>();
        MySpring = GetComponent<SpringJoint>();
        MainCamera = Camera.main;
        StartingBoostTimer = Time.time;
        ResetBoostCooldown();
        if (ForwardBase == null)
        {
            ForwardBase = new GameObject("ForwardBase");
            ForwardBase.transform.eulerAngles = new Vector3(CurrentPlayerNumber - 1, MainCamera.transform.eulerAngles.y, 0);
        }
    }

    void Update()
    {
        if (IsGrounded())
        {
            //RB.AddForce(InputToMovement(ForwardBase.transform.forward, ForwardBase.transform.right, PushForce));

            FollowingParent.transform.position += InputToMovement(ForwardBase.transform.forward, ForwardBase.transform.right, PushForce) * Time.deltaTime
                * ((Boosting) ? BoostPush : 1);
            CapVelocity(MaxVelocity);
            
            TriggerBoost();
            Manager.UpdatePlayerBoost(CurrentPlayerNumber - 1, BoostCooldownRatio());
        }
        else
        {
            if(MySpring != null)
            {
                Destroy(MySpring);
                RB.constraints = RigidbodyConstraints.None;
            }
        }

        if(JumpCheck())
        {
            JumpAction();
        }
    }


    Vector3 InputToMovement(Vector3 _Forward, Vector3 _Right, float _PushForce)
    {
        Vector3 Movement = new Vector3();
        if (Vector3.Distance(transform.position, FollowingParent.transform.position) < 1)
        {
            if (Vertical.JoystickPosition() > 0) Movement += _Forward * _PushForce * -Vertical.JoystickPosition();
            if (Vertical.JoystickPosition() < 0) Movement -= _Forward * _PushForce * Vertical.JoystickPosition();
            if (Horizontal.JoystickPosition() > 0) Movement += _Right * _PushForce * Horizontal.JoystickPosition();
            if (Horizontal.JoystickPosition() < 0) Movement -= _Right * _PushForce * -Horizontal.JoystickPosition();

            //if (Boosting) Movement *= 20;
        }
        //print(Movement);
        return Movement;
    }
    void JumpAction()
    {
        if (Input.GetButtonDown(Jump.InputName))
        {
            FollowingParent.transform.position += new Vector3(0, JumpHeight, 0);
        }
    }


    void TriggerBoost()
    {
        if (Time.time - StartingBoostTimer >= .1f) Boosting = false;
        if(Input.GetButtonDown(Boost.InputName))
        {
            if(!Boosting && BoostCooldownRatio() == 100)
            {
                StartingBoostTimer = Time.time;
                Boosting = true;
                ResetBoostCooldown();
            }
        }
    }
    void CapVelocity(float _MaxVelocity)
    {
        //RB.velocity = new Vector3(Mathf.Clamp(RB.velocity.x, -1, 1), RB.velocity.y, Mathf.Clamp(RB.velocity.z, -1, 1));
        RB.velocity = Vector3.ClampMagnitude(RB.velocity, 1);
    }
    bool GetKey(KeyCode _Key)
    {
        return Input.GetKey(_Key);
    }

    bool IsGrounded()
    {
        bool Returning = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, 1 << 0))
        {
            if (hit.transform.gameObject != gameObject)
            {
                Returning = true;
            }
        }

        return Returning;
    }
    bool JumpCheck()
    {
        bool Returning = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, 1 << 0))
        {
            if (hit.transform.gameObject != gameObject)
            {
                Returning = true;
            }
        }

        return Returning;
    }

    private void OnCollisionEnter(Collision collision)
    {
        CharacterController OtherController = collision.gameObject.GetComponent<CharacterController>();
        if (OtherController != null)
        {
            //TERRIBLE
            //OtherController.RB.AddForce(RB.velocity * 100 * Time.deltaTime, ForceMode.Impulse);
            //RB.AddForce((new Vector3(0,5,0) * PushUpForce) * (OtherController.RB.velocity.magnitude));
            FollowingParent.transform.position += Vector3.Scale(((collision.transform.position - transform.position) * -CollisionForce) * (OtherController.RB.velocity.magnitude), new Vector3(1,0,1));
            CollideParticle.gameObject.transform.position = collision.contacts[0].point;
            CollideParticle.Play();
        }
    }


    void ResetBoostCooldown()
    {
        BoostCooldown = Time.time;
    }
    float BoostCooldownRatio()
    {
        float Returning = new float();

        Returning = Mathf.Clamp(Time.time - BoostCooldown, 0, 10) * 10;
        return Returning;
    }

    public void Launch(float _Amount)
    {
        FollowingParent.transform.position += new Vector3(0, _Amount, 0);
    }
}
