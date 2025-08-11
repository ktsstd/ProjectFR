using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject Point;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialManagement.Instance.NextProcess();
            Point.gameObject.SetActive(false);
        }
    }
}
