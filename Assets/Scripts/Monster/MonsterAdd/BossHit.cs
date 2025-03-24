using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHit : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(4f);
        DestroyImmediate(gameObject);
    }
}
