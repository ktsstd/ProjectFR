using System.Collections;
using UnityEngine;
using Photon.Pun;

public class Solborn : MonsterAI
{
    [SerializeField] ParticleSystem SolbornEffect;
    [SerializeField] Attackboundary atkboundary;
    public override void Awake()
    {
        base.Awake();
        currentState = States.Stop;
        animator.SetTrigger("Spawn");
        StartCoroutine(StartMove());
    }
    private IEnumerator StartMove()
    {
        yield return new WaitForSeconds(1.1f);
        currentState = States.Idle;
    }
    public override void AttackEvent() 
    {
        atkboundary.EnterPlayer();
        SolbornEffect.Play();
        SoundManager.Instance.PlayMonsterSfx(1, transform.position);
    }
    public override void ShowAttackBoundary()
    {
        atkboundary.ShowBoundary();
    }

    public override void DestroyMonster()
    {
        PlayerController playerCtrl = GameManager.Instance.localPlayerCharacter.GetComponent<PlayerController>();
        PhotonNetwork.Destroy(gameObject);
        if (SceneManagerHelper.ActiveSceneName == "Tutorial")
        {
            TutorialManagement.Instance.CheckMonster();
        }
        else
        {
            GameManager.Instance.CheckMonster();
        }
    }
}
