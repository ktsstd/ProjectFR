using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float damage;
    public TextMeshProUGUI damageText;

    void FixedUpdate()
    {
        damageText.transform.rotation = Camera.main.transform.rotation;
        damageText.text = (Mathf.FloorToInt(damage)).ToString();
    }

    private void ReturnPool()
    {
        PoolManager.Instance.text_Pools.Release(gameObject);
    }

    private void OnEnable()
    {
        GetComponent<Animation>().Play();
        Invoke("ReturnPool", 2f);
    }
}
