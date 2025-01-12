using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{
    private Rigidbody2D rb;
    public int EntityHP;
    public int EntityMaxHP;
    public float EntitySpeed;
    public float acceleration;
    InputAction MoveAction;

    private Vector2 desiredVelocity;
    private Vector2 velocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set basic values for entity
        HP = EntityMaxHP;
        MaxHP = EntityMaxHP;
        Speed = EntitySpeed;

        //Set player controls
        MoveAction = InputSystem.actions.FindAction("Move");

        //Set essential components
        rb = GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {
        desiredVelocity = MoveAction.ReadValue<Vector2>() * Speed;
    }

    private void FixedUpdate()
    {
        velocity = rb.linearVelocity;
        Movement();
    }
    void Movement()
    {
       float maxSpeedChange =  acceleration * Time.fixedDeltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.y = Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
        rb.linearVelocity = velocity;
    }
}
