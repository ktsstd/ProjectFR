using System.Collections;
using UnityEngine;
using Photon.Pun;

public class grave : MonsterAI
{
    private float radius = 5f;
    public override void Awake()
    {
        StartCoroutine(StartSpawn());
        CurHp = 700f;
        animator = GetComponent<Animator>();
    }
    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(3.5f);
        while (CurHp > 0 && PhotonNetwork.IsMasterClient)
        {
            Vector3 randomPos = GetRandomPos();
            PhotonNetwork.Instantiate("Monster/Solborn", randomPos, Quaternion.identity);
            photonView.RPC("PlaySound", RpcTarget.All);
            yield return new WaitForSeconds(6f);
        }
    }
    [PunRPC]
    public void PlaySound()
    {
        SoundManager.Instance.PlayMonsterSfx(5, transform.position);
    }
    public override void Update()
    {
        Transform target = GameObject.FindWithTag("Object").transform;
        Vector3 directionToTarget = target.position - transform.position;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2.5f);
        }
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
