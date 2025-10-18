using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarInv : MonoBehaviour
{
    private Coroutine disCoroutine;
    private void OnEnable()
    {
        disCoroutine = StartCoroutine(DisableAfterTime(4f));        
    }
    public void ResetTimer()
    {
        if (disCoroutine != null)
        {
            StopCoroutine(disCoroutine);
        }
        disCoroutine = StartCoroutine(DisableAfterTime(4f));
    }

    private IEnumerator DisableAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
