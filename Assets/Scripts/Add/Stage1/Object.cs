using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Object : MonoBehaviourPunCallbacks
{
    private float health;
    private float MaxHp = 15000f;
    private bool GameOver = false;
    private GameObject[] Monster;
    private int MonsterCount;
    private float detectRadius = 15.5f;
    [SerializeField] private PlayableDirector DefeatEffect;
    [SerializeField] private Slider ObjectHp;

    private void Start()
    {
        health = MaxHp;
    }
    private void Update()
    {
        Monster = GameObject.FindGameObjectsWithTag("Enemy");
        MonsterCount = Monster.Length;
        ObjectHp.value = health / MaxHp;

        //foreach (GameObject monster in Monster)
        //{
        //    float distance = Vector3.Distance(transform.position, monster.transform.position);

        //    if (distance <= detectRadius)
        //    {
        //        Mugolin mugolin = monster.GetComponent<Mugolin>();
        //        if (mugolin != null)
        //        {
        //            mugolin.StandUp();
        //        }
        //    }
        //}
    }
    public void Damaged(float damage)
    {
        if (GameOver || !PhotonNetwork.IsMasterClient) return;
        photonView.RPC("ObjDmged", RpcTarget.All, damage);
    }
    [PunRPC]
    public void ObjDmged(float damage)
    {
        health -= damage;
        // Debug.Log("남은 체력:" + health);
        if (health <= 0)
        {
            PlayerController playerS = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerS.photonView.RPC("LookAtTarget", RpcTarget.All, gameObject.name, 55555f);
            DefeatEffect.Play();
            GameOver = true;
            Invoke("GameEnd", 13f);
        }
    }

    public virtual void GameEnd()
    {
        GameManager.Instance.GoToMain();
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

    [PunRPC]
    public void HealthRecovery(float _num) // 회복할 배율 입력
    {
        health += MaxHp * _num;
        if (health > MaxHp)
            health = MaxHp;
    }
}
