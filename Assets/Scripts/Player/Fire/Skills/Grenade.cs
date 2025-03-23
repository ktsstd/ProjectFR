using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Vector3 target;  // ��ǥ ��ġ
    public int skilltype;
    public Fire player;

    private float duration = 1.5f; // �̵� �ð�
    private float height = 5f; // �ְ��� ����

    void Start()
    {
        StartCoroutine(MoveParabolic(target, duration));
    }

    private void Update()
    {
        if (transform.position == target)
        {
            switch (skilltype)
            {
                case 0:
                    if (player.pv.IsMine)
                        player.pv.RPC("FlameGrenadeTest", Photon.Pun.RpcTarget.All, target);
                    break;
                case 1:
                    // �ұ�� ��ȯ
                    break;
            }
            Destroy(gameObject);
        }
    }

    IEnumerator MoveParabolic(Vector3 targetPos, float time)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

            currentPos.y += height * Mathf.Sin(t * Mathf.PI);

            transform.position = currentPos;
            yield return null;
        }

        transform.position = targetPos;
    }
}
