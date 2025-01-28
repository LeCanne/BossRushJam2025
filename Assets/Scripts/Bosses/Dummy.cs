using UnityEngine;

public class Dummy : Entity
{
    public int MaxHp;
    public float offsetGrabbedDefault;
    private Rigidbody2D rb;
    public override void Start()
    {
        base.Start();
        HP = MaxHp;
        offsetGrabbed = offsetGrabbedDefault;
        rb2D = GetComponent<Rigidbody2D>(); 
    }

    
    public override void Update()
    {
        base.Update(); 
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if(thrown == true)
        {
            
            InflictDamage(1, this);
        }
      
    }



}
