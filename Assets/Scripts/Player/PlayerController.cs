using JetBrains.Annotations;
using System.Collections;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : Entity
{
    private Rigidbody2D rb;

    //Dash Parameters
    [Header ("Dash Parameter")]
    private bool Dash;
    private bool dashPossible = true;
    private float currentDashTime;
    public float dashCooldown;
    private float attackTimer;
    private bool grab;
    public float delta;
    private bool hold;

    [Header ("Player Parameters")]
    public int EntityHP;
    public int EntityMaxHP;
    public float EntitySpeed;
    public float dashSpeed;
    public float rotationSpeed;
    public float acceleration;
    public float angularAcceleration;
    

    Entity grabbedEntity;
    InputAction MoveAction;
    InputAction DashGrabAction;
    

    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Vector3 angularVelocity;

    
    public override void Start()
    {
        base.Start();
        //Set basic values for entity
        HP = EntityMaxHP;
        MaxHP = EntityMaxHP;
        Speed = EntitySpeed;

        //Set player controls
        MoveAction = InputSystem.actions.FindAction("Move");
        DashGrabAction = InputSystem.actions.FindAction("Attack"); 

        //Set essential components
        rb = GetComponent<Rigidbody2D>();


    }

   
    public override void Update()
    {

        
        //All the Player Inputs.
        MovementInput();
        //All the Cooldowns.
        Cooldowns();
       
       
    }

    private void FixedUpdate()
    {
        velocity = rb.linearVelocity;
       
        if(Dash == true)
        {
            
            DashGrab();
        }

        //GrabbedAnEnemy
        if (grab == true)
        {
            Grab();
        }


        Movement();
    }

    void MovementInput()
    {
        //Movement Input
        if (Dash == false)
        {
            desiredVelocity = MoveAction.ReadValue<Vector2>() * Speed;
        }


        //Dash Input
        if (DashGrabAction.triggered && Dash == false && dashPossible == true)
        {
            Dash = true;
        }

        hold = DashGrabAction.IsPressed();

        

        
       
    }
    void Movement()
    {
        
        if (grab == true)
        {
            float rotSpeedChange = angularAcceleration * Time.fixedDeltaTime;
            rb.freezeRotation = false;
            angularVelocity.z = Mathf.MoveTowards(angularVelocity.z, rotationSpeed, rotSpeedChange);
            rb.angularVelocity = angularVelocity.z;
            grabbedEntity.transform.rotation = transform.rotation;  
            if (hold == false) 
            {
                
                UnGrab();
                grab = false;
            }

            return;
        }

        rb.freezeRotation = true;
       
        //Add Velocity accordingly
        RotateToward(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        float maxSpeedChange =  acceleration * Time.fixedDeltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
        rb.linearVelocity = velocity;
    }

    void DashGrab()
    {
        velocity = desiredVelocity.normalized * dashSpeed;

        
        attackTimer += Time.fixedDeltaTime;

        if(attackTimer > 0.20f)
        {
            InterruptDash();
           
        }
    }

    void Grab() //If we grab, then the velocity should always be 0.
    {
        
        grabbedEntity.GrabState(GetComponent<Entity>(), grab);
        rb.linearVelocity = Vector2.zero;
    }

    void UnGrab()
    {
        grabbedEntity.GrabState(GetComponent<Entity>(), false);
        grabbedEntity = null;
    }

    void InterruptDash() //Used to stop player dash after certain events.
    {
        velocity = Vector2.ClampMagnitude(velocity, Speed);
        attackTimer = 0;
        Dash = false;
        dashPossible = false;
    }

    void Cooldowns() //Used to refill dash after a time.
    {
        if(dashPossible == false)
        {
            currentDashTime += Time.deltaTime;

            if(currentDashTime > dashCooldown)
            {
                currentDashTime = 0;
                dashPossible = true;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(Dash == true)
        {
            if(collision.gameObject.tag == "Enemy")
            {
                //Rotate to the grabbed enemy
                RotateToward(collision.transform.position);
                grabbedEntity = collision.gameObject.GetComponent<Entity>();
                
                grab = true;
            }
            InterruptDash();

        }
    }

    public void RotateToward(Vector3 target)
    {
        Vector3 targ = target;
        targ.z = 0f;

        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    
}
