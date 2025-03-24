using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class MonsterAI : MonoBehaviourPunCallbacks, IPunObservable
{
    public MonsterInfo monsterInfoScript;
    public MonsterInfo monsterInfo;
    private Transform playerController;
    public float monsterSlowCurTime;
    [SerializeField] private GameObject sloweffect;


    public Transform target;
    private Rigidbody rigidbody;
    private List<(float slowtime, float slowmoveSpeed)> slowEffects = new List<(float, float)>();

    public NavMeshAgent agent;
    public bool canMove = true;
    public Animator animator;

    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = 50;
        monsterInfo = Instantiate(monsterInfoScript);
        agent.speed = monsterInfo.speed;
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        playerController = GameObject.FindGameObjectWithTag("Player").transform;
        monsterSlowCurTime = 0;
    }
    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            OnMonsterKnockBack(playerController);
        if (Input.GetKeyDown(KeyCode.V))
            OnMonsterSpeedDown(3f, 3f);
        if (sloweffect != null)
        {
            ParticleSystem ps = sloweffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.startRotation = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
            }
        }

        target = GetClosestTarget();

        if (target != null && canMove && agent.enabled)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (animator != null)
                animator.SetBool("Run", true);

            if (distance <= monsterInfo.attackRange)
            {
                agent.velocity = Vector3.zero;
                if (monsterInfo.attackTimer <= 0)
                {
                    Attack();
                    canMove = false;
                }
                else
                {
                    agent.ResetPath(); // todo -> Idle animation
                }
                if (animator != null)
                    animator.SetBool("Run", false);
            }
            else
            {
                agent.SetDestination(target.position); // todo -> moving animation
                if (animator != null)
                    animator.SetBool("Run", true);
            }

            if (monsterInfo.attackTimer > 0)
            {
                monsterInfo.attackTimer -= Time.deltaTime;
            }
            Vector3 directionToTarget = target.position - transform.position;
            if (directionToTarget != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
        }
        else if (!canMove && agent.enabled)
        {
            agent.velocity = Vector3.zero;
            agent.ResetPath(); // todo -> Idle animation
            if (animator != null)
                animator.SetBool("Run", false);
            target = GetClosestTarget();
        }
    }
    protected Transform GetClosestTarget()
    {
        Transform closestTarget = null;
        float closestDistance = monsterInfo.redistance;

        foreach (string targetTag in monsterInfo.priTarget)
        {
            GameObject[] possibleTargets = GameObject.FindGameObjectsWithTag(targetTag);

            if (possibleTargets == null || possibleTargets.Length == 0)
                continue;

            foreach (GameObject possibleTarget in possibleTargets)
            {
                if (possibleTarget.CompareTag("Player"))
                {
                    PlayerController playerCtrl = possibleTarget.GetComponent<PlayerController>();
                    if (playerCtrl.playerHp <= 0)
                    {
                        continue;
                    }
                       
                }

                float distance = Vector3.Distance(transform.position, possibleTarget.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = possibleTarget.transform;
                }
                else
                {
                    if (!monsterInfo.isBoss)
                    {
                        GameObject objectTarget = GameObject.FindGameObjectWithTag("Object");
                        return objectTarget.transform;
                    }
                    else
                    {
                        GameObject objectTarget = GameObject.FindGameObjectWithTag("Player");
                        PlayerController playerCont = objectTarget.GetComponent<PlayerController>();
                        if (playerCont.playerInfo.hp <= 0)
                        {
                            return null;
                        }
                        else
                        {
                            return objectTarget.transform;
                        }
                    }
                }
            }
        }

        return closestTarget;
    }

    public virtual void Attack() // todo -> attacking animation
    {
        string attackBoundary = "MonsterAdd/" + monsterInfo.attackboundary[0].name;
        Vector3 attackFowardPos = new Vector3(transform.position.x, 0.1f, transform.position.z) + transform.forward * 1.5f;
        if (animator != null)
            animator.SetTrigger("StartAttack");
        Vector3 currentEulerAngles = transform.eulerAngles;
        GameObject AttackObj = PhotonNetwork.Instantiate(attackBoundary, attackFowardPos, Quaternion.Euler(currentEulerAngles.x, currentEulerAngles.y, currentEulerAngles.z));
        AttackObj.transform.SetParent(this.transform);
        Vector3 AttackObjlocal = AttackObj.transform.localPosition;
        AttackObjlocal.y = -0.9f;
        AttackObj.transform.localPosition = AttackObjlocal;
    }

    public virtual void AttackSuccess(GameObject Obj, int damage)
    {
        Debug.Log("attack success: " + Obj + ":" + damage);
        monsterInfo.attackTimer = monsterInfo.attackCooldown;
        canMove = true;
    }

    public virtual void MonsterDmged(float damage)
    {
        if (!photonView.IsMine) return;

        monsterInfo.health -= damage;
        Debug.Log("health: " + monsterInfo.health);
        if (monsterInfo.health <= 0)
        {
            if (animator != null)
                animator.SetTrigger("Die");
            gameObject.SetActive(false);
            GameManager.Instance.CheckMonster();
        }
    }

    public void OnMonsterKnockBack(Transform _transform)
    {
        if (monsterInfo.isBoss) return;
        canMove = false;
        agent.enabled = false;

        Vector3 vec = transform.position - _transform.position;
        vec.x = Mathf.Sign(vec.x) * 2;
        vec.y = 0;
        vec.z = Mathf.Sign(vec.z) * 2;
        rigidbody.AddForce(vec, ForceMode.Impulse);

        OnMonsterStun(0.5f);
    }

    private Coroutine stunCoroutine;
    public void OnMonsterStun(float _time)
    {
        if (stunCoroutine != null)
            StopCoroutine(stunCoroutine);

        stunCoroutine = StartCoroutine(MonsterStun(_time));
    }
    IEnumerator MonsterStun(float _time)
    {
        yield return new WaitForSeconds(_time);
        rigidbody.velocity = Vector3.zero;
        agent.enabled = true;
        canMove = true;
    }

    public void OnMonsterSpeedDown(float _time, float _moveSpeed)
    {
        if (monsterSlowCurTime > 0 && !monsterInfo.isBoss)
        {
            slowEffects.Add((_time, _moveSpeed));
            slowEffects.Sort((a, b) => b.slowmoveSpeed.CompareTo(a.slowmoveSpeed));
        }
        else
        {
            OnMonsterSpeedDownStart(_time, _moveSpeed);
        }
    }

    private Coroutine speedCoroutine;
    public void OnMonsterSpeedDownStart(float _time, float _moveSpeed)
    {
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);

        speedCoroutine = StartCoroutine(MonsterSpeedDowning(_time, _moveSpeed));
    }

    IEnumerator MonsterSpeedDowning(float _time, float _moveSpeed)
    {
        agent.speed -= _moveSpeed;
        sloweffect.SetActive(true);
        monsterSlowCurTime = _time;
        while (monsterSlowCurTime > 0)
        {
            yield return null;
            monsterSlowCurTime -= Time.deltaTime;

            for (int i = 0; i < slowEffects.Count; i++)
            {
                var remaineffect = slowEffects[i];
                slowEffects[i] = (remaineffect.slowtime - Time.deltaTime, remaineffect.slowmoveSpeed);
            }
        }
        // yield return new WaitForSeconds(_time);
        if (slowEffects.Count > 0)
        {
            var nextSlowEffect = slowEffects[0];
            slowEffects.RemoveAt(0);
            agent.speed += _moveSpeed;
            OnMonsterSpeedDownStart(nextSlowEffect.slowtime, nextSlowEffect.slowmoveSpeed);
        }
        else
        {
            speedCoroutine = null;
            agent.speed += _moveSpeed;
            sloweffect.SetActive(false);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(monsterInfo.health);
            stream.SendNext(monsterInfo.attackTimer);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            monsterInfo.health = (float)stream.ReceiveNext();
            monsterInfo.attackTimer = (float)stream.ReceiveNext();
        }
    }
}
