using UnityEngine;

public class Dummy : Entity
{
    public float offsetGrabbedDefault;
    private Rigidbody2D rb;
    public override void Start()
    {
        base.Start();
        offsetGrabbed = offsetGrabbedDefault;
        rb2D = GetComponent<Rigidbody2D>(); 
    }

    
    public override void  Update()
    {
        base.Update(); 
    }

   

    
}
