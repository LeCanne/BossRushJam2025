using UnityEngine;

public class Dummy : Entity
{
    public float offsetGrabbedDefault;
    public override void Start()
    {
        base.Start();
        offsetGrabbed = offsetGrabbedDefault;
    }

    
    public override void  Update()
    {
        base.Update(); 
    }

   

    
}
