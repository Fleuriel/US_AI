using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeManager : MonoBehaviour
{
    public int health;
    public int attack;


    public void TakeDamage(int amount)
    {
        health -= amount; 
        if (health <= 0)
        {
            // Handle the object's death (e.g., destroy the game object)
            Destroy(gameObject);
        }
    }


    public void DealDamage(GameObject target)
    {
        var atm = GetComponent<AttributeManager>();
        if(atm!= null)
        {
            atm.TakeDamage(attack);
        }
    }
}
