// Some stupid rigidbody based movement by Dani

using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Assingables
    public Transform playerCam;
    public Transform orientation;

    //Other
    private Rigidbody rb;
    public bool dead;
    public bool paused;
    public bool onMovingPlatform;
    public Rigidbody prb;       // rigidbody of the moving platform

    //Rotation and look
    private float xRotation;
    private float sensitivity;



    //Movement
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public bool disableCM;      //Disable counter-movement
    public bool disableAR;      //Disable air-reduction
    public float airMovementMultiplier = 0.5f;

    public float counterMovement = 0.175f;
    public float airReduceAmt = .01f;
    public float extraGravity = 3000f;
    public float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1.5f, 0.5f, 1.5f);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    private Vector3 normalVector = Vector3.up;
    public float slidePerameter = 3f;
    public float crouchMovementMultiplier = 0.4f;
    public float crouchMaxSpeed = 8f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    private int jumpCharge = 1;
    public float jumpForce = 550f;
    public float secondJumpForce = 550f;
    public float jumpResetDelay = 6f;
    public float wallJumpUpModifier = 1.5f;
    public float wallJumpModifier = 1.5f;

    //GroundPound
    public float poundSpeed = 40;

    //Input
    public float x, y;
    bool jumping, sprinting, crouching;

    public Animator anim;

    //Audio
    public AudioSource Step;
    private bool stepping;
    public AudioSource Jumping;
    public AudioSource Slide;
    private bool sliding;

    void Awake()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
        {
            sensitivity = PlayerPrefs.GetFloat("Sensitivity");
        }
        rb = GetComponent<Rigidbody>();
        disableCM = false;
        disableAR = false;
        dead = false;
        stepping = false;
        sliding = false;
        onMovingPlatform = false;
    }

    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        if (!dead && !paused)
        {
            MyInput();
            Look();
        }
        else
        {
            if (!crouching && dead)
            {
                StartCrouch();
                crouching = true;
            }
        }
        if (!crouching && ((!onMovingPlatform && rb.velocity.magnitude > 4f) || (onMovingPlatform && (rb.velocity - prb.velocity).magnitude > 4f)))
        {
            if (!stepping && grounded)
            {
                Step.Play(0);
                stepping = true;
            }
            else if (stepping && !grounded)
            {
                Step.Stop();
                stepping = false;
            }
            anim.SetBool("moving", true);
        }
        else
        {
            if (stepping)
            {
                Step.Stop();
                stepping = false;
            }
            anim.SetBool("moving", false);
        }
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");

        //Crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            // only checks for walls
            int layerMask = 1 << 6;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, slidePerameter, layerMask))
                StartCrouch();
            else
            {
                rb.velocity = new Vector3(0, -poundSpeed, 0);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) && crouching)
            StopCrouch();
    }

    private void StartCrouch()
    {
        crouching = true;
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            rb.AddForce(orientation.transform.forward * slideForce);
        }
    }

    private void StopCrouch()
    {
        crouching = false;
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * extraGravity);

        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Find velocity of the moving platform relative to where player is looking
        Vector2 pmag;       // mag for platform
        if (onMovingPlatform)
            pmag = FindPlatformVelRelativeToLook();
        else
            pmag = Vector2.zero;
        float xPmag = pmag.x, yPmag = pmag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag - pmag);
        AirReduction(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed
        float maxSpeed;
        if (!crouching)
            maxSpeed = this.maxSpeed;
        else
            maxSpeed = this.crouchMaxSpeed;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouching && grounded && readyToJump)
        {
            Debug.Log(rb.velocity.magnitude);
            if (!sliding && rb.velocity.magnitude > 3.5f)
            {
                Slide.Play(0);
                sliding = true;
            }
            if (sliding && rb.velocity.magnitude < 3.5f)
            {
                Slide.Stop();
                sliding = false;
            }
            rb.AddForce(Vector3.down * Time.deltaTime * 800);
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag - xPmag > maxSpeed) x = 0;
        if (x < 0 && xMag - xPmag < -maxSpeed) x = 0;
        if (y > 0 && yMag - yPmag > maxSpeed) y = 0;
        if (y < 0 && yMag - yPmag < -maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f;

        // Movement in air
        if (!grounded)
        {
            if (sliding)
            {
                Slide.Stop();
                sliding = false;
            }
            multiplier = airMovementMultiplier;
        }
        else if (crouching)
            multiplier = crouchMovementMultiplier;

        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        if ((grounded || jumpCharge > 0) && readyToJump && !dead && !crouching)
        {
            readyToJump = false;

            //Add jump forces
            if (grounded)
            {
                rb.AddForce(Vector2.up * jumpForce * wallJumpUpModifier);
                if(Vector3.Angle(Vector2.up, normalVector) > 35)
                    rb.AddForce(normalVector * jumpForce * wallJumpModifier);
                else
                    rb.AddForce(normalVector * jumpForce * (2f - wallJumpUpModifier));
            }
            else
            {
                rb.AddForce(Vector2.up * secondJumpForce * 2f);
                jumpCharge--;
            }

            //Audio
            Jumping.Play(0);
            Step.Stop();
            stepping = false;

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping || disableCM) return;

        //Slow down sliding
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }
        
        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (!onMovingPlatform && Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    private void AirReduction(float x, float y, Vector2 mag)
    {
        if (grounded || disableAR) return;

        //Counter movement
        if ((x > 0 && mag.x < 0) || (x < 0 && mag.x > 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * x * airReduceAmt);
        }
        if ((y > 0 && mag.y < 0) || (y < 0 && mag.y > 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * y * airReduceAmt);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    public Vector2 FindPlatformVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(prb.velocity.x, prb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = prb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    private void OnCollisionExit(Collision other)
    {
        grounded = false;
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < 95;
    }

    // Allows player to jump away from wall
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Enemy")
        {
            for (int i = 0; i < other.contactCount; i++)
            {
                Vector3 normal = other.contacts[i].normal;
                //FLOOR
                if (IsFloor(normal))
                {
                    grounded = true;
                    normalVector = normal;
                    jumpCharge = 1;
                }
            }
            
        }

    }

    
}