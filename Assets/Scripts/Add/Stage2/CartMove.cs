using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.Demo.Cockpit;

public class CartMove : MonoBehaviourPun
{
    public States currentState;
    float health;
    float MaxHp = 15000f;
    bool GameOver = false;
    [SerializeField] ParticleSystem DefeatEffect;
    [SerializeField] GameManager Gms;
    [SerializeField] Slider ObjectHp;

    public enum States
    {
        Idle,
        Move,
        Destroy
    }
    void Start()
    {
        health = MaxHp;
        currentState = States.Move;
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
        ObjectHp.value = health / MaxHp;
    }

    void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * 1f);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WavePoint")
        {
            Gms.Stage2Wave();
        }
        else if (other.gameObject.tag == "Stop")
        {
            Gms.Stage2ObstacleWave();
        }
        else if (other.gameObject.tag == "Finish")
        {
            PlayerController playerS = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerS.photonView.RPC("LookAtTarget", RpcTarget.All, gameObject.name, 55555f);
            Invoke("GameEnd", 6f);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Stop")
        {
            currentState = States.Idle;
        }
    }
}
