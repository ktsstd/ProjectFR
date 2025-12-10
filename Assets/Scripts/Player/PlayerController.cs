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
using Photon.Pun.Demo.PunBasics;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject playerRespawnZone;
    public ParticleSystem respawnEF;
    public GameObject recoveryShileObject;
    public GameObject recoveryShileDestroy;
    public GameObject recoveryEF;
    public GameObject eatEffeck;
    public GameObject dashEF;
    public GameObject stunEF;
    public GameObject hitEF;
    public ParticleSystem poisonEF;
    public Image respawnImgae;
    public PlayerInfo playerInfo;

    public PhotonView pv;
    public Animator animator;
    public Rigidbody rigidbody;
    public CapsuleCollider collider;
    private CinemachineVirtualCamera virtualCamera;
    private PlayerUi playerUi;

    private Vector3 receivePos;
    private Quaternion receiveRot;

    public int money = 0;
    public float playerHp;
    public float playerMaxHp;
    public float playerAtk;
    public float playerSpeed;
    public float currentDashCoolTime;
    public float dashCoolTime;
    public float[] currentSkillsCoolTime;
    public float[] skillsCoolTime;
    public int elementalCode = 0;
    public bool elementalkey = true;

    public float recoveryShield;

    public GameObject[] skillRanges;
    public Vector3[] skillsPos = new Vector3[3];
    public FusionSkill fusion;
    public ItemUi item;

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
        Die,
        Stun,
        Fusion
    }
    public States currentStates;

    public virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        fusion = FindAnyObjectByType<FusionSkill>();
        playerUi = GameObject.Find("PlayerUi").GetComponent<PlayerUi>();
        item = playerUi.transform.GetChild(0).gameObject.GetComponent<ItemUi>();

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        currentStates = States.Idle;
        StartStatSet();

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedCharacter", out object character);
        elementalCode = (int)character;


        if (GameManager.Instance.selectedMode == 1)
        {
            elementalkey = false;
        }

        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
        else
        {
            playerUi.OtherPlayerData(gameObject);
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
            currentSkillsCoolTime[2] -= Time.deltaTime;
        if (currentStates == States.Dash)
            dashEF.SetActive(true);
        else
            dashEF.SetActive(false);
        if (currentStates == States.Die)
            respawnImgae.rectTransform.sizeDelta = new Vector2((1 - (respawnCoolTime / 6)) * 8.5f,(1 - (respawnCoolTime / 6)) * 8.5f);

        if (playerHp <= 0 && currentStates != States.Die)
        {
            pv.RPC("DieRPC", RpcTarget.All, null);
        }


        if (pv.IsMine)
        {
            RunAnimation();
            CameraMove();
            playerUi.InputDashData(currentDashCoolTime, dashCoolTime);
            playerUi.InputSkillData(currentSkillsCoolTime, skillsCoolTime);
            playerUi.InputMoneyData(money);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);

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
                    break;
                case States.Dash:
                    Dash();
                    break;
                case States.Die:
                    OnPlayerRespawn();
                    break;
                case States.Stun:
                    break;
                case States.Fusion:
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                pv.RPC("OnPlayerHit", RpcTarget.All, 100f);
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                pv.RPC("OnPlayerStun", RpcTarget.All, 5f);
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                pv.RPC("OnPlayerPoison", RpcTarget.All, 5);
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                money += 5000;
            }

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * 20);
        }
    }

    [PunRPC]
    public void DieRPC()
    {
        playerHp = 0;
        currentStates = States.Die;
        pv.RPC("OnPlayerDie", RpcTarget.All, null);
        pv.RPC("PlayTriggerAnimation", RpcTarget.All, "die");
        GameManager.Instance.CheckPlayer();
        OffSkills();
    }

    public virtual void RunAnimation()
    {
        animator.SetBool("isRun", isMoving);
    }

    Ray ray;
    Vector3 targetPos;
    public bool isMoving = false;
    void Move()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButton(1))
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

            if (Input.GetKeyDown(KeyCode.S))
            {
                targetPos += transform.position;
                isMoving = false;
            }

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    Vector3 dashPos;
    public void Dash()
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

                RaycastHit dashRayHit;
                if (Physics.Raycast(transform.position + Vector3.up - transform.forward * 0.5f, transform.forward, out dashRayHit, 1.5f))
                {
                    if (dashRayHit.transform.tag == "BackGroundObj" || dashRayHit.transform.tag == "Object")
                    {
                        dashPos = transform.position;
                        animator.SetBool("isDash", false);
                        currentStates = States.Idle;
                    }
                }

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
                    SoundManager.Instance.PlayPlayerSfx(7, transform.position);
                }
            }
        }
    }

    public virtual void Attack() { }

    public void UseItem()
    {
        if (GameManager.Instance.selectedMode == 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                item.UseItem(elementalCode, 0, GetMousePosition());
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                item.UseItem(elementalCode, 1, GetMousePosition());
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                item.UseItem(elementalCode, 2, GetMousePosition());
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                item.UseItem(elementalCode, 3, GetMousePosition());
            }
        }
    }

    float fusionHoldTime = 2f;
    public void ElementalSetting()
    {
        if (elementalkey)
        {
            if (!skillRanges[0].activeSelf && !skillRanges[1].activeSelf && !skillRanges[2].activeSelf)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    if (fusion.RetrunFusionSkillReady(elementalCode) != 10)
                    {
                        fusionHoldTime -= Time.deltaTime;
                        playerUi.FusionHoldTime = fusionHoldTime;
                        if (fusionHoldTime <= 0)
                        {
                            fusion.FusionSkillSetting(GetSkillRange(10f));
                        }
                    }
                }
                if (Input.GetKeyUp(KeyCode.R))
                {
                    fusion.FusionSkillRangeOff();
                    if (fusion.RetrunFusionSkillReady(elementalCode) != 10)
                    {
                        if (fusionHoldTime <= 0)
                        {
                            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                            foreach (GameObject player in players)
                            {
                                if (player.GetComponent<PlayerController>().elementalCode == fusion.RetrunFusionSkillReady(elementalCode))
                                {
                                    if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 3f && player.GetComponent<PlayerController>().currentStates != States.Die)
                                    {
                                        fusion.UseFusionSkill(GetSkillRange(10f));
                                    }
                                }
                            }
                        }
                        fusionHoldTime = 2f;
                        playerUi.FusionHoldTime = fusionHoldTime;
                    }
                    else
                    {
                        if (fusion.FusionSkillCoolTimeCheck())
                            fusion.pv.RPC("ElementalSettingMaster", RpcTarget.MasterClient, elementalCode);
                    }
                }
            }
        }
    }

    [PunRPC]
    public void UsingFusionSkill(bool _bool)
    {
        if (_bool)
            currentStates = States.Fusion;
        else
            currentStates = States.Idle;
    }

    public virtual float OtherShield(float _damage) { return _damage; }

    float damageDelayTime;
    [PunRPC]
    public void OnPlayerHit(float _damage)
    {
        if (currentStates != States.Die)
            Instantiate(hitEF, transform);
        if (pv.IsMine)
        {
            if (currentStates != States.Die && currentStates != States.Fusion)
            {
                float currentDamage = _damage;
                if (damageDelayTime <= 0)
                {
                    currentDamage = OtherShield(currentDamage);
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
                    pv.RPC("PlayTriggerAnimation", RpcTarget.All, "die");
                    GameManager.Instance.CheckPlayer();
                }
            }

            playerUi.InputHpData(playerHp, playerMaxHp);
        }
    }

    [PunRPC]
    public void OnPlayerTrueDamage(float _damage)
    {
        if (pv.IsMine)
        {
            if (currentStates != States.Die && currentStates != States.Fusion)
            {
                playerHp -= _damage;

                if (playerHp <= 0)
                {
                    playerHp = 0;
                    currentStates = States.Die;
                    pv.RPC("OnPlayerDie", RpcTarget.All, null);
                    pv.RPC("PlayTriggerAnimation", RpcTarget.All, "die");
                }
            }

            playerUi.InputHpData(playerHp, playerMaxHp);
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
        collider.enabled = false;

        PlayerDieEvent();
    }

    public virtual void PlayerDieEvent() { }

    [PunRPC]
    public void OnPlayerRecovery(float _heal)
    {
        if (currentStates != States.Die)
        {
            recoveryEF.SetActive(false);
            recoveryEF.SetActive(true);
            SoundManager.Instance.PlayPlayerSfx(2, transform.position);
            if (pv.IsMine)
            {
                playerHp += _heal;
                if (playerHp > playerMaxHp)
                    playerHp = playerMaxHp;

                playerUi.InputHpData(playerHp, playerMaxHp);
            }
        }
    }

    private List<GameObject> playerInRange = new List<GameObject>();
    public float respawnCoolTime = 6;
    public void OnPlayerRespawn()
    {
        if (playerInRange.Count != 0)
        {
            if (playerInRange[0].GetComponent<PlayerController>().currentStates == States.Die)
                playerInRange.RemoveAt(0);

            if (respawnCoolTime > 0)
            {
                respawnCoolTime -= Time.deltaTime;
            }
            if (respawnCoolTime <= 0)
            {
                respawnCoolTime = 6;
                GameObject targetPlayer = playerInRange[0];
                pv.RPC("PlayerRespawn", RpcTarget.All, null);
            }
        }
        else
        {
            respawnCoolTime = 6;
        }
    }

    [PunRPC]
    public void PlayerRespawn()
    {
        SoundManager.Instance.PlayPlayerSfx(17, transform.position);
        collider.enabled = true;
        respawnEF.Play();
        playerRespawnZone.SetActive(false);
        PlayTriggerAnimation("reset");
        if (pv.IsMine)
        {
            PlayerController playerController = playerInRange[0].GetComponent<PlayerController>();
            playerHp = playerController.playerHp * 0.5f;
            playerController.pv.RPC("OnPlayerTrueDamage", RpcTarget.All, playerController.playerHp * 0.5f);
            currentStates = States.Idle;
            playerUi.InputHpData(playerHp, playerMaxHp);
        }
        playerInRange.Clear();
    }

    public IEnumerator PlayerAttackBuff(float _time, float _atk)
    {
        float atk = (playerAtk * _atk) - playerAtk;
        playerAtk += atk;
        yield return new WaitForSeconds(_time);
        playerAtk -= atk;
    }

    private Coroutine stunCoroutine;
    [PunRPC]
    public void OnPlayerStun(float _time)
    {
        if (pv.IsMine)
        {
            if (stunCoroutine != null)
                StopCoroutine(stunCoroutine);

            stunCoroutine = StartCoroutine("PlayerStun", _time);
        }
        stunEF.SetActive(true);
        PlayTriggerAnimation("reset");
    }

    IEnumerator PlayerStun(float _time)
    {
        currentStates = States.Stun;
        animator.SetBool("isDash", false);
        yield return new WaitForSeconds(_time);
        pv.RPC("StunEffect", RpcTarget.All, false);
        if (currentStates != States.Die)
            currentStates = States.Idle;
    }

    [PunRPC]
    public void StunEffect(bool _bool)
    {
        stunEF.SetActive(_bool);
    }

    private Coroutine suppressedCoroutine;
    [PunRPC]
    public void OnPlayerSuppressed(float _time)
    {
        if (pv.IsMine)
        {
            if (suppressedCoroutine != null)
                StopCoroutine(suppressedCoroutine);

            suppressedCoroutine = StartCoroutine("PlayerSuppressed", _time);
        }
        OffSkills();
        PlayTriggerAnimation("reset");

    }

    public virtual void OffSkills() { }

    IEnumerator PlayerSuppressed(float _time)
    {
        currentStates = States.Stun;
        animator.SetBool("isDash", false);
        collider.isTrigger = true;
        rigidbody.useGravity = false;
        eatEffeck.SetActive(true);
        yield return new WaitForSeconds(_time);
        collider.isTrigger = false;
        rigidbody.useGravity = true;
        eatEffeck.SetActive(false);
        if (currentStates != States.Die)
            currentStates = States.Idle;
    }

    [PunRPC]
    public void PlayerStunClear()
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
            stunEF.SetActive(false);

            if (currentStates != States.Die)
                currentStates = States.Idle;
        }

        if (suppressedCoroutine != null)
        {
            StopCoroutine(suppressedCoroutine);
            
            rigidbody.useGravity = true;
            collider.isTrigger = false;
            eatEffeck.SetActive(false);

            if (currentStates != States.Die)
                currentStates = States.Idle;
        }
    }

    Coroutine poisonCoroutine;
    float[] poisontime = new float[2] {0, 0};
    [PunRPC]
    public void OnPlayerPoison(int _time)
    {
        if (poisonCoroutine != null)
        {
            if ((Time.time - poisontime[0]) - poisontime[1] < _time)
            {
                StopCoroutine(poisonCoroutine);
            }
        }
        if (!poisonEF.isPlaying)
            poisonEF.Play();

        poisontime[0] = Time.time;
        poisontime[1] = (float)_time;
        poisonCoroutine = StartCoroutine("PlayerPoison", _time);
    }

    IEnumerator PlayerPoison(int _time)
    {
        int count = 0;
        while (count != _time)
        {
            if (currentStates == States.Die)
                break;
            yield return new WaitForSeconds(0.5f);
            OnPlayerTrueDamage(playerHp * 0.05f);
            count += 1;
            if (pv.IsMine)
                playerUi.InputHpData(playerHp, playerMaxHp);
            yield return new WaitForSeconds(0.5f);
        }
        poisonEF.Stop();
    }


    public virtual void OnTriggerEnter(Collider collision)
    {
        if (currentStates == States.Die)
        {
            if (collision.gameObject.tag == "Player")
            {
                playerInRange.Add(collision.gameObject);
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
        recoveryShileObject.SetActive(false);
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
    float scrollSpeed = 3f;
    float minY = 10f, maxY = 14f;
    float minZ = -10f, maxZ = -6f;
    float screenWidth;
    float screenHeight;
    public void CameraMove()
    {
        if (currentStates != States.Fusion)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            Vector3 mousePosition = Input.mousePosition;

            if (scroll != 0)
            {
                CinemachineTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
                Vector3 offset = transposer.m_FollowOffset;

                offset.y -= scroll * scrollSpeed;
                offset.z += scroll * scrollSpeed;

                offset.y = Mathf.Clamp(offset.y, minY, maxY);
                offset.z = Mathf.Clamp(offset.z, minZ, maxZ);

                transposer.m_FollowOffset = offset;
            }

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
                if (mousePosition.y >= 0 && mousePosition.y >= screenHeight)
                    virtualCamera.transform.Translate(Vector3.forward * 25f * Time.deltaTime, Space.World);
                if (mousePosition.y <= 0 && mousePosition.y <= screenHeight)
                    virtualCamera.transform.Translate(Vector3.back * 25f * Time.deltaTime, Space.World);
                if (mousePosition.x <= 0 && mousePosition.x <= screenWidth)
                    virtualCamera.transform.Translate(Vector3.left * 25f * Time.deltaTime, Space.World);
                if (mousePosition.x >= 0 && mousePosition.x >= screenWidth)
                    virtualCamera.transform.Translate(Vector3.right * 25f * Time.deltaTime, Space.World);
            }
        }
    }

    [PunRPC]
    public IEnumerator LookAtTarget(string _gameObject, float _time)
    {
        if (pv.IsMine)
        {
            virtualCamera.LookAt = GameObject.Find(_gameObject).transform;
            yield return new WaitForSeconds(_time);
            virtualCamera.LookAt = transform;
        }
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(playerHp);
            stream.SendNext(currentStates);
            stream.SendNext(recoveryShield);
            stream.SendNext(skillsPos);
            stream.SendNext(elementalCode);
            stream.SendNext(respawnCoolTime);
            stream.SendNext(playerAtk);
            stream.SendNext(playerMaxHp);
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            playerHp = (float)stream.ReceiveNext();
            currentStates = (States)stream.ReceiveNext();
            recoveryShield = (float)stream.ReceiveNext();
            skillsPos = (Vector3[])stream.ReceiveNext();
            elementalCode = (int)stream.ReceiveNext();
            respawnCoolTime = (float)stream.ReceiveNext();
            playerAtk = (float)stream.ReceiveNext();
            playerMaxHp = (float)stream.ReceiveNext();
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

    [PunRPC]
    public void PlayTriggerAnimation(string _name)
    {
        animator.SetTrigger(_name);
    }

    public void MoveAniSound()
    {
        int randomNum = Random.Range(0, 2);
        switch (randomNum)
        {
            case 0:
                SoundManager.Instance.PlayPlayerSfx(8, transform.position);
                break;
            case 1:
                SoundManager.Instance.PlayPlayerSfx(9, transform.position);
                break;
            case 2:
                SoundManager.Instance.PlayPlayerSfx(10, transform.position);
                break;
        }
    }
}