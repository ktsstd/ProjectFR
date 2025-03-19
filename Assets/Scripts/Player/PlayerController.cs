using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using System.ComponentModel;
using UnityEngine.PlayerLoop;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject playerRespawnZone;
    public GameObject recoveryShileObject;
    public GameObject recoveryShileDestroy;
    public PlayerInfo playerInfo;

    public PhotonView pv;
    public Animator animator;
    private Rigidbody rigidbody;
    private CapsuleCollider collider;
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

    public float recoveryShield;

    public GameObject[] skillRanges;

    public virtual void StartStatSet()
    {
        playerHp = playerInfo.hp;
        playerMaxHp = playerInfo.hp;
        playerAtk = playerInfo.atk;
        playerSpeed = playerInfo.speed;
        currentDashCoolTime = 0f;
        dashCoolTime = playerInfo.dashCoolTime;
        currentSkillsCoolTime[0] = 0f;
        currentSkillsCoolTime[1] = 0f;
        currentSkillsCoolTime[2] = 0f;
        skillsCoolTime[0] = playerInfo.skillsCoolTime[0];
        skillsCoolTime[1] = playerInfo.skillsCoolTime[1];
        skillsCoolTime[2] = playerInfo.skillsCoolTime[2];
        recoveryShield = 0;
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
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
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
        if (currentSkillsCoolTime[0] > 0)
            currentSkillsCoolTime[0] -= Time.deltaTime;
        if (currentSkillsCoolTime[1] > 0)
            currentSkillsCoolTime[1] -= Time.deltaTime;
        if (currentSkillsCoolTime[2] > 0)
            currentSkillsCoolTime[3] -= Time.deltaTime;

        if (pv.IsMine)
        {
            animator.SetBool("isRun", isMoving);
            CameraMove();
            if (currentStates != States.Idle)
            {
                skillRanges[0].SetActive(false);
                skillRanges[1].SetActive(false);
                skillRanges[2].SetActive(false);
            }

            switch (currentStates)
            {
                case States.Idle:
                    Move();
                    Dash();
                    Attack();
                    break;
                case States.Attack:
                    isMoving = false;
                    targetPos = transform.position;
                    // ���� �Ұ���, ��� ������ Idle�� ��ȯ
                    break;
                case States.Dash:
                    Dash();
                    // ĳ���͸��� �̵����
                    break;
                case States.Die:
                    OnPlayerRespawn(); // ��Ȱ��� �ֱ�
                    // ī�޶� ����(�������)
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                pv.RPC("OnPlayerHit", RpcTarget.All, 100f);
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * 20);
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

                transform.rotation = lookRotation;
                transform.position = Vector3.MoveTowards(transform.position, dashPos, 30 * Time.deltaTime);

                if (Vector3.Distance(transform.position, dashPos) < 0.1f)
                {
                    animator.SetBool("isDash", false);
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
                    animator.SetBool("isDash", true);
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

    public virtual void Attack() { }

    float damageDelayTime;
    [PunRPC]
    public virtual void OnPlayerHit(float _damage)
    {
        if (pv.IsMine)
        {
            if (currentStates != States.Die)
            {
                float currentDamage = _damage;
                if (damageDelayTime <= 0)
                {
                    if (recoveryShield != 0)
                    {
                        if (recoveryShield < currentDamage)
                        {
                            currentDamage = currentDamage - recoveryShield;
                            pv.RPC("OnShieldBreak", RpcTarget.All, null);
                            recoveryShield = 0;
                        }
                        else
                        {
                            recoveryShield -= currentDamage;
                            currentDamage = 0;
                        }
                    }

                    playerHp -= currentDamage;
                    damageDelayTime = 0.2f;
                }

                if (playerHp <= 0)
                {
                    playerHp = 0;
                    currentStates = States.Die;
                    pv.RPC("OnPlayerDie", RpcTarget.All, null);
                }
            }
        }
    }

    [PunRPC]
    public void OnPlayerTrueDamage(float _damage)
    {
        if (pv.IsMine)
        {
            playerHp -= _damage;

            if (playerHp <= 0)
            {
                playerHp = 0;
                currentStates = States.Die;
                pv.RPC("OnPlayerDie", RpcTarget.All, null);
            }
        }
    }

    [PunRPC]
    public void OnShieldBreak()
    {
        recoveryShileObject.SetActive(false);
        recoveryShileDestroy.SetActive(true);
    }

    [PunRPC]
    public void OnPlayerDie()
    {
        playerRespawnZone.SetActive(true);
        // �÷��̾� ���� �ִϸ��̼� ���� ��
    }

    [PunRPC]
    public void OnPlayerRecovery(float _heal)
    {
        if (pv.IsMine)
        {
            if (currentStates != States.Die)
            {
                playerHp += _heal;
                if (playerHp > playerMaxHp)
                    playerHp = playerMaxHp;
            }
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
            }
            if (respawnCoolTime <= 0)
            {
                // ��Ȱ �ִϸ��̼� �ֱ�
                GameObject targetPlayer = playerInRange[0];
                pv.RPC("PlayerRespawn", RpcTarget.All, null);
            }
        }
        else
        {
            respawnCoolTime = 10;
        }
    }

    [PunRPC]
    public void PlayerRespawn()
    {
        playerRespawnZone.SetActive(false);
        if (pv.IsMine)
        {
            PlayerController playerController = playerInRange[0].GetComponent<PlayerController>();
            playerHp = playerController.playerHp * 0.5f;
            playerController.pv.RPC("OnPlayerTrueDamage", RpcTarget.All, playerController.playerHp * 0.5f);
            currentStates = States.Idle;
        }
        playerInRange.Clear();
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

    [PunRPC]
    public IEnumerator RecoveryShield(float _shield)
    {
        recoveryShileObject.SetActive(true);
        recoveryShield = _shield;
        yield return new WaitForSeconds(10f);
        if (!(recoveryShield <= 0))
        {
            OnPlayerRecovery(recoveryShield);
            recoveryShield = 0;
            yield return new WaitForSeconds(2f);
            recoveryShileObject.SetActive(false);
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

    // bool previousIsMoving;
    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(playerHp);
            stream.SendNext(currentStates);
            stream.SendNext(recoveryShield);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            playerHp = (float)stream.ReceiveNext();
            currentStates = (States)stream.ReceiveNext();
            recoveryShield = (float)stream.ReceiveNext();
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