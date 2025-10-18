using System.Collections;
using UnityEngine;
using Photon.Pun;

public class grave : MonsterAI
{
    private float radius = 5f;
    public override void Awake()
    {
        currentState = States.Idle;
        CurHp = 700f;
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            CurHp *= 0.6f;
        }
        else if (PhotonNetwork.PlayerList.Length == 1)
        {
            CurHp *= 0.5f;
        }
        
        MaxHp = CurHp;
        StartCoroutine(StartSpawn());
        animator = GetComponent<Animator>();
        HpBarObj.SetActive(false);
    }
    public override void Update()
    {
        //if (!PhotonNetwork.IsMasterClient) return;
        switch (currentState)
        {
            case States.Idle:
                break;
            case States.Stop:
                break;
            case States.Attack:
                break;
            case States.Die:
                break;
            case States.Stun:
                break;
        }
        Transform target = GameObject.FindWithTag("Object").transform;
        Vector3 directionToTarget = target.position - transform.position;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);
        }
    }
    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(3.5f);
        while (CurHp > 0 && PhotonNetwork.IsMasterClient)
        {
            Vector3 randomPos = GetRandomPos();
            PhotonNetwork.Instantiate("Monster/Stage1/Solborn", randomPos, Quaternion.identity);
            photonView.RPC("PlaySound", RpcTarget.All);
            yield return new WaitForSeconds(6f);
        }
    }
    [PunRPC]
    public void PlaySound()
    {
        SoundManager.Instance.PlayMonsterSfx(5, transform.position);
    }
    public override void OnMonsterSpeedDown(float _time, float _moveSpeed)
    {

    }
    public override void OnMonsterKnockBack(Transform _transform)
    {

    }

    Vector3 GetRandomPos()
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 randomPos = new Vector3(randomCircle.x, 0, randomCircle.y);
        return transform.position + randomPos;
    }
}
