using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    float damage;
    float speed = 6f;
    float destroytime = 1f;

    private void Awake()
    {
        damage = 110;
        Destroy(gameObject, destroytime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerS = other.GetComponent<PlayerController>();
            playerS.photonView.RPC("OnPlayerHit", RpcTarget.All, damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Object"))
        {
            Object objectS = other.GetComponent<Object>();
            objectS.Damaged(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Summon"))
        {
            SummonAI summonS = other.GetComponent<SummonAI>();
            summonS.photonView.RPC("OnSummonHit", RpcTarget.All, damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }            
    }
}
