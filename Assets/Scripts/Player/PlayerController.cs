using JetBrains.Annotations;
using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : Entity
{
    private Rigidbody2D rb;
   
    //Dash Parameters
    private bool Dash;
    private bool dashPossible = true;
    private float currentDashTime;
    public float dashCooldown;
    private float attackTimer;
    private bool grab;

    [Header ("Player Parameters")]
    public int EntityHP;
    public int EntityMaxHP;
    public float EntitySpeed;
    public float dashSpeed;
    public float acceleration;

    Entity grabbedEntity;
    InputAction MoveAction;
    InputAction DashGrabAction;

    private Vector2 desiredVelocity;
    private Vector2 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    // Update is called once per frame
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

        //GrabbedAnEnemy
        if(grab == true)
        {
            Grab();
        }
    }
    void Movement()
    {
        if (grab == true)
            return;
        
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

    void Grab()
    {
        
        grabbedEntity.GrabState(GetComponent<Entity>(), grab);
        rb.linearVelocity = Vector2.zero;
    }

    void InterruptDash()
    {
        velocity = Vector2.ClampMagnitude(velocity, Speed);
        attackTimer = 0;
        Dash = false;
        dashPossible = false;
    }

    void Cooldowns()
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
                RotateToward(collision.transform);
                grabbedEntity = collision.gameObject.GetComponent<Entity>();
                
                grab = true;
            }
            InterruptDash();

        }
    }

    public void RotateToward(Transform target)
    {
        Vector3 from = transform.right;
        Vector3 to = target.position - transform.position;

        float angle = Vector3.SignedAngle(from, to, transform.forward);
        transform.Rotate(0.0f, 0.0f, angle);
    }
}
