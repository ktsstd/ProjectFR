using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effectsc : MonoBehaviour
{
    public float time;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(time);
        DestroyImmediate(gameObject);
    }
}
