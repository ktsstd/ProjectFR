using System.Collections;
using UnityEngine;
using Photon.Pun;

public class grave : MonsterAI
{
    private float hp = 800f;
    private float radius = 5f;
    public GameObject Solborn;
    public IEnumerator Start()
    {
        yield return new WaitForSeconds(3.5f);
        while (true)
        {
            Vector3 randomPos = GetRandomPos();
            //PhotonNetwork.Instantiate("Monster/Solborn", randomPos, Quaternion.identity);
            Instantiate(Solborn, randomPos, Quaternion.identity);
            SoundManager.Instance.PlayMonsterSfx(5, transform.position);
            yield return new WaitForSeconds(6f);
        }
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
