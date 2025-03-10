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
    public PlayerInfo playerInfo;

    public Rigidbody rigidbody;

    public BoxCollider collider;

    public PhotonView pv;
    private CinemachineVirtualCamera virtualCamera;

    private Vector3 receivePos;
    private Quaternion receiveRot;

    public bool isMoving = false;

    public enum States
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

        currentStates = States.Idle;
    }

    public virtual void Update()
    {
        if (pv.IsMine)
        {
            switch (currentStates)
            {
                case States.Idle:
                    Move();

                    if (Input.GetKeyDown(KeyCode.Space)) // dash start
                    {
                        targetPos = transform.position;
                        isMoving = false;
                        collider.isTrigger = true;
                        dashPos = GetSkillRange(5);
                        currentStates = States.Dash;
                    }
                    break;
                case States.Attack:
                    // ���� �Ұ���, ��� ������ Idle�� ��ȯ
                    break;
                case States.Dash:
                    Dash();
                    // ĳ���͸��� �̵����
                    break;
                case States.Die:
                    // ī�޶� ����
                    break;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, Time.deltaTime * 10);
            transform.rotation = Quaternion.Slerp(transform.rotation, receiveRot, Time.deltaTime * 10);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            currentStates = States.Idle;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            currentStates = States.Attack;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            currentStates = States.Dash;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            currentStates = States.Die;
        }
    }

    Ray ray;
    Vector3 targetPos;
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
            transform.position = Vector3.MoveTowards(transform.position, targetPos, playerInfo.speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    Vector3 dashPos;
    void Dash()
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