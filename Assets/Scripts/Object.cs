using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Object : MonoBehaviourPunCallbacks
{
    public float health = 500f;
    PhotonView pv;
    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    [PunRPC]
    public void Damaged(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
