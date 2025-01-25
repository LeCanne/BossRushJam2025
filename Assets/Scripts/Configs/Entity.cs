using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected int HP;
    protected int MaxHP;   
    protected float Speed;
    protected bool grabbed;
    protected bool thrown;
   
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
        if (grabbed == true)
        {
            transform.position = grabber.transform.position + grabber.transform.right * offsetGrabbed;
            collisionBox.enabled = false;
            return;
        }
        else
        {
            collisionBox.enabled = true;
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

            Debug.Log("hit");

                ContactPoint2D contact = collision.contacts[0];
                Vector2 newDir = Vector2.zero;
                var curDir = gameObject.transform.TransformDirection(Vector3.right);
                newDir = Vector3.Reflect(curDir, contact.normal);
                gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.right, newDir);
            
        }
    }

}
