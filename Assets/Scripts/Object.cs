using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Object : MonoBehaviourPunCallbacks
{
    private float health = 5500f;
    private GameObject[] Monster;
    private int MonsterCount;
    private float detectRadius = 31f;

    private void Update()
    {
        Monster = GameObject.FindGameObjectsWithTag("Enemy");
        MonsterCount = Monster.Length;

        foreach (GameObject monster in Monster)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);

            if (distance <= detectRadius)
            {
                Mugolin mugolin = monster.GetComponent<Mugolin>();
                if (mugolin != null)
                {
                    mugolin.StandUp();
                }
            }
        }
    }

    [PunRPC]
    public void Damaged(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyImmediate(gameObject);
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            health = (float)stream.ReceiveNext();
        }
    }
}
