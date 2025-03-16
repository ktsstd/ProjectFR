using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using System.ComponentModel;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject playerRespawnZone;
    public PlayerInfo playerInfo;

    public Rigidbody rigidbody;
    public BoxCollider collider;
    public PhotonView pv;
    private CinemachineVirtualCamera virtualCamera;

    private Vector3 receivePos;
    private Quaternion receiveRot;

    public float playerHp;
    public float playerMaxHp;
    public float playerAtk;
    public float playerSpeed;
    public float currentDashCoolTime;
    public float dashCoolTime;
    public float[] currentSkillsCoolTime;
    public float[] skillsCoolTime;

    void StartStatSet()
    {
        playerHp = playerInfo.hp;
        playerMaxHp = playerInfo.hp;
        playerAtk = playerInfo.atk;
        playerSpeed = playerInfo.speed;
        currentDashCoolTime = 0f;
        dashCoolTime = playerInfo.dashCoolTime;
        currentSkillsCoolTime = playerInfo.skillsCoolTime;
        for (int i = 0; i < currentSkillsCoolTime.Length; i++)
            currentSkillsCoolTime[i] = 0;
        skillsCoolTime = playerInfo.skillsCoolTime;
    }

    public enum States // Idle, Attack, Dash, Die
    {
        Idle, // run, idle
        Attack,
        Dash,
        Die
    }
    public States currentStates;

    public virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        pv = GetComponent<PhotonView>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();

        currentStates = States.Idle;
        StartStatSet();

        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
    }

    public virtual void Update()
    {
        if (currentDashCoolTime > 0)
            currentDashCoolTime -= Time.deltaTime;
        if (damageDelayTime > 0)
            damageDelayTime -= Time.deltaTime;

        if (pv.IsMine)
        {
            CameraMove();

            switch (currentStates)
            {
                case States.Idle:
                    Move();
                    Dash();
                    break;
                case States.Attack:
                    // 조작 불가능, 모션 끝날때 Idle로 전환
                    break;
                case States.Dash:
                    Dash();
                    // 캐릭터마다 이동기능
                    break;
                case States.Die:
                    OnPlayerRespawn(); // 부활기능 넣기
                    // 카메라 조작(관전기능)
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                pv.RPC("OnPlayerHit", RpcTarget.All, 10f, false);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * 10);
        }
    }

    Ray ray;
    Vector3 targetPos;
    private bool isMoving = false;
    void Move()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("ground")))
            {
                targetPos = hit.point;
                isMoving = true;
            }
        }
        if (isMoving)
        {
            targetPos.y = transform.position.y;
            Vector3 direction = (targetPos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.1f);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, playerSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    Vector3 dashPos;
    public virtual void Dash()
    {
        if (currentStates == States.Dash)
        {
            if (dashPos != null)
            {
                dashPos.y = transform.position.y;
                Vector3 direction = (dashPos - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.1f);
                transform.position = Vector3.MoveTowards(transform.position, dashPos, 30 * Time.deltaTime);

                if (Vector3.Distance(transform.position, dashPos) < 0.1f)
                {
                    collider.isTrigger = false;
                    currentStates = States.Idle;
                }
            }
        }
        else
        {
            if(currentDashCoolTime <= 0)
            {
                if (Input.GetKeyDown(KeyCode.Space)) // dash start
                {
                    currentDashCoolTime = dashCoolTime;
                    targetPos = transform.position;
                    isMoving = false;
                    collider.isTrigger = true;
                    dashPos = GetSkillRange(5);
                    currentStates = States.Dash;
                }
            }
        }
    }

    float damageDelayTime;
    [PunRPC]
    public void OnPlayerHit(float _damage, bool _isNoDelay)
    {
        if (currentStates != States.Die)
        {
            if (_isNoDelay)
            {
                playerHp -= _damage;
            }
            else
            {
                if (damageDelayTime <= 0)
                {
                    playerHp -= _damage;
                    damageDelayTime = 0.2f;
                }
            }

            

            if (playerHp <= 0) // 플레이어 죽음 애니메이션 실행 등
            {
                currentStates = States.Die;
                playerRespawnZone.SetActive(true);
                playerHp = 0;
            }
        }
    }

    [PunRPC]
    public void OnPlayerHeal(float _heal)
    {
        if (currentStates != States.Die)
        {
            playerHp += _heal;
            if (playerHp > playerMaxHp)
                playerHp = playerMaxHp;
        }
    }

    private List<GameObject> playerInRange = new List<GameObject>();
    private float respawnCoolTime;
    public void OnPlayerRespawn()
    {
        if (playerInRange.Count != 0)
        {
            if (respawnCoolTime > 0)
            {
                respawnCoolTime -= Time.deltaTime;
                Debug.Log(respawnCoolTime);
            }
            if (respawnCoolTime <= 0)
            {
                // 부활 애니메이션 넣기
                pv.RPC("PlayerRespawn", RpcTarget.All, null);
            }
        }
        else
        {
            respawnCoolTime = 10;
        }
    }

    [PunRPC]
    private void PlayerRespawn()
    {
        PlayerController targetPlayer = playerInRange[0].GetComponent<PlayerController>();
        playerHp = targetPlayer.playerHp * 0.5f;
        targetPlayer.OnPlayerHit(targetPlayer.playerHp * 0.5f, true);
        playerInRange.Clear();
        currentStates = States.Idle;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (currentStates == States.Die)
        {
            if (other.tag == "Player")
            {
                playerInRange.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentStates == States.Die)
        {
            if (other.tag == "Player")
            {
                playerInRange.Remove(other.gameObject);
            }
        }
    }

    bool cameraMoving = false;
    public void CameraMove()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            cameraMoving = !cameraMoving;
            if (cameraMoving)
            {
                virtualCamera.Follow = null;
                virtualCamera.LookAt = null;
            }
            else
            {
                virtualCamera.Follow = transform;
                virtualCamera.LookAt = transform;
            }
        }

        if (cameraMoving)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                virtualCamera.transform.Translate(Vector3.forward * 20f * Time.deltaTime, Space.World);
            if (Input.GetKey(KeyCode.LeftArrow))
                virtualCamera.transform.Translate(Vector3.left * 20f * Time.deltaTime, Space.World);
            if (Input.GetKey(KeyCode.RightArrow))
                virtualCamera.transform.Translate(Vector3.right * 20f * Time.deltaTime, Space.World);
            if (Input.GetKey(KeyCode.DownArrow))
                virtualCamera.transform.Translate(Vector3.back * 20f * Time.deltaTime, Space.World);
        }
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }
        else { return Vector3.zero; }
    }

    public Vector3 GetSkillRange(float _range)
    {
        Vector3 direction = GetMousePosition() - transform.position;
        float distance = direction.magnitude;

        if (distance > _range)
        {
            direction = direction.normalized;
            return transform.position + direction * _range;
        }
        else
            return GetMousePosition();
    }
}