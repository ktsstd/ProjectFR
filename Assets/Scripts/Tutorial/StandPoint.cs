using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandPoint : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (TutorialManagement.Instance.curTutorialProcess == 5)
            { 
                TutorialManagement.Instance.NextProcess();
            }
        }
    }
}
