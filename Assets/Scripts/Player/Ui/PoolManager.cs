using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    public IObjectPool<GameObject> text_Pools { get; private set; }

    public GameObject damageTextPrefab;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        text_Pools = new ObjectPool<GameObject>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnedToPool,
            OnDestroyPoolObject,
            true,
            50,
            100
            );
    }

    private GameObject CreatePooledItem()
    {
        GameObject newObject = Instantiate(damageTextPrefab);
        return newObject;
    }

    private void OnTakeFromPool(GameObject textPre)
    {
        textPre.SetActive(true);
    }

    private void OnReturnedToPool(GameObject textPre)
    {
        textPre.SetActive(false);
    }

    private void OnDestroyPoolObject(GameObject textPre)
    {
        Destroy(textPre);
    }
}
