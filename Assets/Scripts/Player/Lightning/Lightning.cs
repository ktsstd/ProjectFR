using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : PlayerController
{
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (currentStates == States.Dash)
        {
            if (other.tag == "Enemy")
            {
                
            }
        }
    }
}