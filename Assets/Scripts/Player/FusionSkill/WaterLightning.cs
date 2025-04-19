using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterLightning : MonoBehaviour
{
    public GameObject thunder;
    public PhotonView pv;

    float attackTime = 1f;

    private void Start()
    {
        SoundManager.Instance.PlayPlayerSfx(23, transform.position);
        pv = GetComponent<PhotonView>();
        Invoke("SelfDestroy", 10.5f);
    }

    void Update()
    {
        if (attackTime >= 0)
        {
            attackTime -= Time.deltaTime;
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                pv.RPC("ThunderPosSetting", RpcTarget.MasterClient, null);
            }
            attackTime = 1f;
        }
    }

    Vector3[] attackPosList = new Vector3[3];

    [PunRPC]
    public void ThunderPosSetting()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies[0].GetComponent<MonsterAI>().monsterInfo.isBoss == true)
        {
            pv.RPC("BossAttack", RpcTarget.All, enemies[0].transform.position);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                attackPosList[i] = enemies[Random.Range(0, enemies.Length)].transform.position;
            }
            pv.RPC("EnemyAttack", RpcTarget.All, attackPosList);
        }
    }

    [PunRPC]
    public void BossAttack(Vector3 _targetPos)
    {
        Instantiate(thunder, _targetPos, thunder.transform.rotation);
    }

    [PunRPC]
    public void EnemyAttack(Vector3[] _targetPos)
    {
        SoundManager.Instance.PlayPlayerSfx(24, transform.position);
        for (int i = 0; i < 3; i++)
        {
            Instantiate(thunder, _targetPos[i], thunder.transform.rotation);
        }
    }

    void SelfDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
