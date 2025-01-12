using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected int HP;
    protected int MaxHP;   
    protected float Speed;
    public virtual void InflictDamage(int damage, Entity entity)
    {
        entity.HP -= damage;
        CheckHP();
    }

    public virtual void CheckHP()
    {
        if(HP <= 0)
        {
            Destroy(gameObject);

        }
    }

    
}
