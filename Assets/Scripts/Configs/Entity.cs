using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected int HP;
    protected int MaxHP;   
    protected float Speed;
    protected bool grabbed;
    protected bool thrown;
    protected bool vulnerable;
   
    protected Collider2D collisionBox;
    protected Rigidbody2D rb2D;
   
    protected float offsetGrabbed;
    private Entity grabber;

    
    public virtual void Start()
    {
        collisionBox = GetComponent<Collider2D>();
    }
    public virtual void Update()
    {
        if(thrown == true)
        {
            //Prevent Player from picking up enemies during damage animations.
           
            vulnerable = false;
        }
        if (grabbed == true)
        {
            //Changes the position of the grabbed enemy when it is rotating around the player
            transform.position = grabber.transform.position + grabber.transform.right * offsetGrabbed;
            collisionBox.enabled = false;
            return;
        }
        else
        {
            collisionBox.enabled = true;
        }

        
        
    }

    public virtual void FixedUpdate()
    {
        if(thrown == true)
        {
            Recover();
        }
        
    }
    public virtual void InflictDamage(int damage, Entity entity)
    {
        
        entity.HP -= damage;
        CheckHP(entity);
    }

    protected virtual void CheckHP(Entity entity)
    {
        if(HP <= 0)
        {
            Destroy(gameObject);

        }
    }

    protected virtual void Recover()
    {
        //Recover when velocity slows, the boss gets up if that happens.
        Debug.Log(rb2D.linearVelocity.magnitude);
        if(rb2D.linearVelocity.magnitude < 10) 
        {
            rb2D.linearDamping = 2f;

            if(rb2D.linearVelocity.magnitude < 2f)
            {
                //Clamp it back to 0, then do the boss stuff.
                thrown = false;
                rb2D.linearDamping = 0f;
            }
            
        }
    }

    public virtual void Throw(Entity grabbedEntity, float speed)
    {
        grabbedEntity.rb2D.linearVelocity = grabbedEntity.transform.right * speed;
        grabbedEntity.thrown = true;
    }

    public virtual void GrabState(Entity Currentgrabber, bool grab)
    {
        grabber = Currentgrabber;
        grabbed = grab;
        
       
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (grabbed == false && thrown == true) 
        {

            

                ContactPoint2D contact = collision.contacts[0];
                Vector2 newDir = Vector2.zero;
                var curDir = gameObject.transform.TransformDirection(Vector3.right);
                newDir = Vector3.Reflect(curDir, contact.normal);
                gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.right, newDir);
            
        }
    }

}
