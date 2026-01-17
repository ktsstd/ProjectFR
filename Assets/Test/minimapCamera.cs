using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapCamera : MonoBehaviour
{
    GameObject playerObj;
    public Vector3 center = Vector3.zero;
    public float radius = 50f;

    [SerializeField] GameObject indicatorPrefab;
    [SerializeField] GameObject enemyIndicatorPrefab;
    [SerializeField] RectTransform minimapRect;
    private List<GameObject> indicators = new List<GameObject>();

    private void Start()
    {
        FindLocalPlayer();
    }

    private void Update()
    {
        if (playerObj == null)
        {
            FindLocalPlayer();
        }
    }

    private void LateUpdate()
    {
        if (playerObj != null)
        {
            if (SceneManagerHelper.ActiveSceneName == "Stage1")
            {
                Vector3 newPosition = playerObj.transform.position;
                newPosition.y = transform.position.y;

                Vector3 offset = new Vector3(newPosition.x - center.x, 0, newPosition.z - center.z);
                if (offset.magnitude > radius)
                {
                    offset = offset.normalized * radius;
                    newPosition = center + offset;
                    newPosition.y = transform.position.y;
                }

                transform.position = newPosition;
            }
            else if (SceneManagerHelper.ActiveSceneName == "Stage2")
            {
                Vector3 newPosition = playerObj.transform.position;
                newPosition.y = transform.position.y;
                transform.position = newPosition;
            }
            UpdateEnemyIndicators();
        }
    }

    void FindLocalPlayer()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView pv = obj.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                playerObj = obj;
                break;
            }
        }
    }

    void UpdateEnemyIndicators()
    {
        foreach (GameObject indicator in indicators)
        {
            Destroy(indicator);
        }
        indicators.Clear();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.name.Contains("Stop"))
            {
                continue;
            }
            Camera minimapCamera = GetComponent<Camera>();
            Vector3 viewportPos = minimapCamera.WorldToViewportPoint(enemy.transform.position);

            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
            {
                float xPos = Mathf.Clamp(viewportPos.x, 0.03f, 0.97f) * minimapRect.sizeDelta.x;
                float yPos = Mathf.Clamp(viewportPos.y, 0.03f, 0.97f) * minimapRect.sizeDelta.y;
                Vector2 indicatorPos = new Vector2(xPos - minimapRect.sizeDelta.x / 2, yPos - minimapRect.sizeDelta.y / 2);

                GameObject indicator = Instantiate(indicatorPrefab, minimapRect);
                RectTransform indicatorRT = indicator.GetComponent<RectTransform>();
                indicatorRT.anchoredPosition = indicatorPos;

                Vector3 dir = enemy.transform.position - playerObj.transform.position;
                Vector2 dir2D = new Vector2(dir.x, dir.z);
                float angle = Mathf.Atan2(dir2D.y, dir2D.x) * Mathf.Rad2Deg;
                indicatorRT.rotation = Quaternion.Euler(0, 0, angle);

                indicators.Add(indicator);
            }
            else if (viewportPos.x > 0 || viewportPos.x < 1 || viewportPos.y > 0 || viewportPos.y < 1)
            {
                GameObject indicator = Instantiate(enemyIndicatorPrefab, minimapRect);
                RectTransform indicatorRT = indicator.GetComponent<RectTransform>();
                indicatorRT.anchoredPosition = new Vector2((viewportPos.x - 0.5f) * minimapRect.sizeDelta.x, (viewportPos.y - 0.5f) * minimapRect.sizeDelta.y);
                indicators.Add(indicator);
            }
        }
    }
}
