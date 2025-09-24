using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class CartMove : MonoBehaviourPun
{
    public States currentState;
    float health;
    float MaxHp = 15000f;
    bool GameOver = false;
    [SerializeField] ParticleSystem DefeatEffect;
    [SerializeField] Slider ObjectHp;
 
    public enum States
    {
        Idle,
        Move,
        Destroy
    }
    void Awake()
    {
        currentState = States.Idle;
    }

    void Update()
    {
        switch (currentState)
        {
            case States.Idle:
                break;
            case States.Move:
                Move();
                break;
            case States.Destroy:
                break;
        }
    }

    void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 0.1f);
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
        if (health <= 0)
        {
            PlayerController playerS = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerS.photonView.RPC("LookAtTarget", RpcTarget.All, gameObject.name, 55555f);
            DefeatEffect.Play();
            GameOver = true;
            Invoke("GameEnd", 6f);
        }
    }

    public virtual void GameEnd()
    {
        GameManager.Instance.GoToMain();
    }


    // 어느 지점에 부딫히면 Idle로 바꾸고 뭐 어쩌고 저쩌고 이런거 넣기
}
