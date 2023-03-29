using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{




    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    //[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    //[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    //[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    //const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    //public BoolEvent OnCrouchEvent;
    ////private bool m_wasCrouching = false;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        //if (OnCrouchEvent == null)
        //	OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }


    public void Move(float move, bool jump) //bool crouch,
    {
        //// If crouching, check to see if the character can stand up
        //if (!crouch)
        //{
        //	// If the character has a ceiling preventing them from standing up, keep them crouching
        //	if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
        //	{
        //		crouch = true;
        //	}
        //}

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            //// If crouching
            //if (crouch)
            //{
            //	if (!m_wasCrouching)
            //	{
            //		m_wasCrouching = true;
            //		OnCrouchEvent.Invoke(true);
            //	}

            //	// Reduce the speed by the crouchSpeed multiplier
            //	move *= m_CrouchSpeed;

            //	// Disable one of the colliders when crouching
            //	if (m_CrouchDisableCollider != null)
            //		m_CrouchDisableCollider.enabled = false;
            //}
            //else
            //{
            //	// Enable the collider when not crouching
            //	if (m_CrouchDisableCollider != null)
            //		m_CrouchDisableCollider.enabled = true;

            //	if (m_wasCrouching)
            //	{
            //		m_wasCrouching = false;
            //		OnCrouchEvent.Invoke(false);
            //	}
            //}


            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }



    //[SerializeField] private float speed = 5;
    //[SerializeField] GameObject dervy;
    //public float airWalkSpeed = 5f;
    //public float jumpImpulse = 7f;
    //private Vector2 moveInput;
    ////Collisions collisions;
    ////Damageable damageable;
    //public Rigidbody2D rb;
    //Animator anim;
    ////ParticleAnimations particleAnim;
    ////PlayerInventory playerInv;
    //public bool doubleJump;
    //private bool jumpPressed;
    //private bool canDash = true;
    //private bool isDashing;
    //private float dashingPower = 24;
    //private float dashingTime = 0.2f;
    //private float dashingCooldown = 1f;
    //public AudioSource runningAudio;
    //public Animator animPU;
    ////public bool collectedDoubleJump = false;
    ////public bool collectedDash = false;
    ////public bool collectedAirDash = false;

    ////public new PlayerAudio audio;
    ////public ParticleSystem dustParticles;

    //[SerializeField] private TrailRenderer tr;
    //// CurrentSpeed function 
    //public float CurrentSpeed
    //{
    //    get
    //    {
    //        // If the player canMove(is not attacking)
    //        if (CanMove)
    //        {
    //            // If is moving and is not colliding with a wall
    //            if (IsMoving && !collisions.IsOnWall)
    //            {
    //                // If is on the ground
    //                if (collisions.IsGrounded)
    //                {
    //                    // Get the player speed on ground         
    //                    return speed;
    //                }
    //                else
    //                {
    //                    // If is not on the ground get the player speed on air that is a different var (we'll use it to manage the difficulty increment ofthe   game)
    //                    return airWalkSpeed;
    //                }
    //            }
    //            else
    //            {
    //                // Idle speed is 0
    //                return 0;
    //            }
    //        }
    //        else
    //        {
    //            // Movement locked 
    //            return 0;
    //        }
    //    }
    //}

    //[SerializeField] private bool _isMoving = false;
    //// IsMoving function 
    //public bool IsMoving
    //{
    //    get
    //    {
    //        // Return the value inside the isMoving variable just created
    //        return _isMoving;
    //    }
    //    private set
    //    {
    //        // Set isMoving to the value is gonna be passed into the set
    //        _isMoving = value;

    //        // Set the boolean in the animator with the same value static strings
    //        anim.SetBool(AnimationStrings.isMoving, value);
    //    }
    //}

    //[SerializeField] private bool _isFacingRight = true;
    //// IsFacingRight function
    //public bool IsFacingRight
    //{
    //    get
    //    {
    //        // Return the value inside the variable that is updated inside the code
    //        return _isFacingRight;
    //    }
    //    private set
    //    {
    //        // If get false as a paramether
    //        if (_isFacingRight != value)
    //        {
    //            // Flip the local scale to make the player face the opposite direction
    //            transform.localScale *= new Vector2(-1, 1);
    //        }

    //        // Set the variable with the value passet in the set 
    //        _isFacingRight = value;
    //    }
    //}

    //public bool CanMove
    //{
    //    // Still the same logic as above
    //    get
    //    {
    //        return anim.GetBool(AnimationStrings.canMove);
    //    }
    //}

    //public bool isAlive
    //{
    //    get
    //    {
    //        return anim.GetBool(AnimationStrings.isAlive);
    //    }
    //}


    //// It's called when the script is loaded (when the game start)
    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    anim = GetComponent<Animator>();
    //    animPU = GetComponent<Animator>();
    //    //collisions = GetComponent<Collisions>();
    //    //damageable = GetComponent<Damageable>();
    //    //playerInv = GetComponent<PlayerInventory>();
    //    //runningAudio = GetComponent<AudioSource>();
    //}

    //// It's called every fixed frame-rate frame.
    //private void FixedUpdate()
    //{
    //    // Prevent the player to do things while dashing
    //    if (isDashing)
    //    {
    //        return;
    //    }

    //    // If player is not being hit right now 
    //    if (!damageable.LockVelocity)
    //    {
    //        // Move the player
    //        rb.velocity = new Vector2(moveInput.x * CurrentSpeed, rb.velocity.y);
    //    }

    //    // Update the animator paramether with the current vertical velocity to update the air state machine in the animator 
    //    anim.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);

    //    // Check if is on the ground and is not jymping to reset the double jump bool and be able to double jump again
    //    if (collisions.IsGrounded && !jumpPressed)
    //    {
    //        doubleJump = false;
    //    }

    //    if (IsMoving && collisions.IsGrounded)
    //    {
    //        //particleAnim.anim.SetBool("isMoving", true);

    //        if (!runningAudio.isPlaying)
    //        {
    //            runningAudio.Play();
    //        }
    //    }
    //    else
    //    {
    //        runningAudio.Stop();
    //    }

    //    if (Vector3.Distance(dervy.transform.position, transform.position) < 1f)
    //    {
    //        TheCurseOfDervyJernz();
    //    }
    //}

    //// It's called while the player is moving(takes the parametheres setted on the Input System controller)
    //public void OnMove(InputAction.CallbackContext context)
    //{
    //    // Get the player position
    //    moveInput = context.ReadValue<Vector2>();

    //    // If player is alive
    //    if (isAlive)
    //    {
    //        // IsMoving setter = it pass true if the vector is actually moving and vice versa
    //        IsMoving = moveInput != Vector2.zero;

    //        // Change sprite direction
    //        SetFacingDirection(moveInput);

    //        // Check to prevent the player from kepp walking into the wall and don't fall 
    //        if (context.started && collisions.IsOnWall)
    //        {
    //            IsMoving = false;
    //        }
    //        // If is not alive
    //    }
    //    else
    //    {
    //        // Block movement
    //        IsMoving = false;
    //    }
    //}

    //private void SetFacingDirection(Vector2 moveInput)
    //{
    //    // If the player is moving right and is not facing right
    //    if (moveInput.x > 0 && !IsFacingRight)
    //    {
    //        // Face the right
    //        IsFacingRight = true;

    //        // If the player is moving left and is facing right    
    //    }
    //    else if (moveInput.x < 0 && IsFacingRight)
    //    {
    //        // Face the left
    //        IsFacingRight = false;
    //    }
    //}

    //public void OnJump(InputAction.CallbackContext context)
    //{        // Add interactions in OnJump?
    //    var jumpAction = context.interaction;

    //    // If can't double jump yet 
    //    if (PlayerPrefs.GetInt("purchasedDoubleJump") == 0)
    //    {
    //        // Check if the key is pressed  and if player can move and if player is on the ground or can doubleJump
    //        if (context.started && CanMove && collisions.IsGrounded)
    //        {
    //            //player jump audio 
    //            audio.PlayjumpAudio();

    //            // Update animator paramether using static strings  
    //            anim.SetTrigger(AnimationStrings.jump);

    //            // Add jump inpulse on the y axis 
    //            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
    //        }
    //    }
    //    // If purchased double jump or colleted
    //    if (PlayerPrefs.GetInt("purchasedDoubleJump") == 1 || playerInv.TemporaryDoubleJump)
    //    {
    //        // If jump key and can move
    //        if (context.started && CanMove)
    //        {
    //            if (collisions.IsGrounded || doubleJump)
    //            {
    //                // Setting this for the check in the update
    //                jumpPressed = true;

    //                //player jump audio 
    //                audio.PlayjumpAudio();

    //                // Update animator paramether using static strings  
    //                anim.SetTrigger(AnimationStrings.jump);

    //                // Add jump inpulse on the y axis 
    //                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);

    //                // Update double jump bool
    //                doubleJump = !doubleJump;
    //            }
    //        }

    //        if (context.canceled)
    //        {
    //            // Finish jump for the update 
    //            jumpPressed = false;
    //        }
    //    }
    //}

    //public void OnAttack(InputAction.CallbackContext context)
    //{
    //    if (context.started)
    //    {
    //        // Attack updating animator paramether
    //        anim.SetTrigger(AnimationStrings.attack);

    //        if (PlayerPrefs.GetInt("swordAttackPowerUp") == 1)
    //        {
    //            Debug.Log("POwer up prefs setted to 1");
    //            animPU.SetTrigger(AnimationStrings.attack);
    //        }

    //        //player attack Audio
    //        audio.PlayattackAndHitAudio();
    //    }
    //}

    //public void OnRangedAttack(InputAction.CallbackContext context)
    //{
    //    // If can shoot
    //    if (context.started && playerInv.ThrowingSwords > 0)
    //    {
    //        // Shoot
    //        anim.SetTrigger(AnimationStrings.rangedAttack);

    //        // Update the remaining swords
    //        playerInv.ThrowingSwords--;
    //    }
    //}
    //public void OnHit(int damage, Vector2 knockback)
    //{
    //    // Apply knockback inpulse 
    //    rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);

    //    //player damage Audio
    //    audio.PlaytakeDamageAudio();
    //}

    //public void OnDash(InputAction.CallbackContext context)
    //{

    //    if (PlayerPrefs.GetInt("purchasedDash") == 1 || playerInv.TemporaryDash)
    //    {
    //        if (collisions.IsGrounded)
    //        {
    //            if (context.started && canDash)
    //            {
    //                StartCoroutine(Dash());
    //            }
    //        }
    //    }
    //    if (PlayerPrefs.GetInt("purchasedAirDash") == 1 || playerInv.TemporaryAirDash)
    //    {
    //        if (context.started && canDash)
    //        {
    //            StartCoroutine(Dash());
    //        }
    //    }
    //}

    //private void TheCurseOfDervyJernz()
    //{
    //    if (Vector3.Distance(dervy.transform.position, transform.position) < 1f)
    //    {
    //        damageable.Health -= 1;

    //        if (Vector3.Distance(dervy.transform.position, transform.position) > 1f)
    //        {
    //            return;
    //        }
    //    }

    //}

    //private IEnumerator Dash()
    //{
    //    canDash = false;
    //    isDashing = true;

    //    // Store the current gravity value
    //    float originalGravity = rb.gravityScale;

    //    // Disable gravity during the dash 
    //    rb.gravityScale = 0f;

    //    // Create the dash inpulse 
    //    rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

    //    // Display the trail
    //    tr.emitting = true;

    //    // Stop dashing after a certain amount of time 
    //    yield return new WaitForSeconds(dashingTime);

    //    tr.emitting = false;
    //    rb.gravityScale = originalGravity;
    //    isDashing = false;

    //    // Give the player a cooldown to not let him abuse of the dash power
    //    yield return new WaitForSeconds(dashingCooldown);
    //    canDash = true;
    //}
}
