using System.Collections;
using UnityEngine;
using Photon.Pun;

public class grave : MonsterAI
{
    private float hp = 800f;
    private float radius = 5f;
    // PhotonNetwork.Instantiate("Monster/Solborn", spawnPositions[sp].position, Quaternion.identity);
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
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3.5f);
        while (true)
        {
            Vector3 randomPos = GetRandomPos();
            PhotonNetwork.Instantiate("Monster/Solborn", randomPos, Quaternion.identity);
            yield return new WaitForSeconds(2f);
        }
    }
    public override void OnMonsterSpeedDown(float _time, float _moveSpeed)
    {

    }

    Vector3 GetRandomPos()
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 randomPos = new Vector3(randomCircle.x, 0, randomCircle.y);
        return transform.position + randomPos;
    }
    public override void MonsterDmged(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
