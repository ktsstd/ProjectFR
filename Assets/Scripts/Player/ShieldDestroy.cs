using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDestroy : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Disable", 1f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
