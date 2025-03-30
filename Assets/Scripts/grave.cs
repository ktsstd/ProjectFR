using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class grave : MonoBehaviourPun, IPunObservable
{
    private float hp = 800f;
    private float radius = 5f;
    // PhotonNetwork.Instantiate("Monster/Solborn", spawnPositions[sp].position, Quaternion.identity);
    private void Update()
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

    Vector3 GetRandomPos()
    {
        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 randomPos = new Vector3(randomCircle.x, 0, randomCircle.y);
        return transform.position + randomPos;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(hp);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            hp = (float)stream.ReceiveNext();
        }
    }
}
