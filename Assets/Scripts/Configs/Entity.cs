using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected int HP;
    protected int MaxHP;   
    protected float Speed;
    protected bool grabbed;
   
    protected Collider2D collisionBox;
   
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

    public virtual void Throw()
    {

    }

    public virtual void GrabState(Entity Currentgrabber, bool grab)
    {
        grabber = Currentgrabber;
        grabbed = grab;
        
       
    }


    
}
