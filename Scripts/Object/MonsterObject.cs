using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;
    private MonsterInfo monsterInfo;

    private int nowHp;
    public bool isDead = false;

    //上一次攻击时间
    private float lastAtkTime;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) { return; }
        //根据速度设置动画
        //magnitude是Vector3的模长
        animator.SetBool("Run", agent.velocity.magnitude > 0.1f);
        //范围内攻击
        if (Vector3.Distance(this.transform.position , MainTowerObject.Instance.transform.position) < 5 &&
            Time.time - lastAtkTime >= monsterInfo.atkDelay)
        {
            lastAtkTime = Time.time;
            //播放攻击动画
            animator.SetBool("Atk", true);
        }
    }

    public void InitMonsterInfo(MonsterInfo info)
    {
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animatorRes);

        monsterInfo = info;
        nowHp = info.hp;
        agent.speed = info.moveSpeed;
        agent.acceleration = info.moveSpeed * 2;
        agent.angularSpeed = info.roundSpeed;
    }

    public void Wound(int dmg)
    {
        //防止多次死亡
        if (isDead) { return; }

        nowHp -= dmg;
        //播放受伤动画
        animator.SetTrigger("Wound");

        if(nowHp <= 0)
        {
            //死亡
            Dead();
        }
        else
        {
            //播放受伤音效
            GameDataMgr.Instance.PlaySound("Wound");
        }
    }

    public void Dead()
    {
        //播放音效
        GameDataMgr.Instance.PlaySound("dead");

        isDead = true;
        animator.SetBool("Dead",true);
        agent.isStopped = true;
        

        //加钱
        GameLevelMgr.Instance.player.AddMoney(20);

    }

    /// <summary>
    /// 动画死亡事件
    /// </summary>
    public void DeadEvent()
    {
        

        //记录减少怪物数量
        GameLevelMgr.Instance.ChangeMonsterNum(false,this);
        //销毁
        Destroy(this.gameObject, 3f);

        //检测游戏是否胜利
        if (GameLevelMgr.Instance.CheckGameOver())
        {
            //游戏胜利，显示结束面板并存储当前金币
            UIManager.Instance.ShowPanel<GameOverPanel>().ChangeInfo(true);
        }
        
    }

    /// <summary>
    /// 动画出生结束事件
    /// </summary>
    public void BornOverEvent()
    {
        //出生结束后朝塔移动
        agent.SetDestination(MainTowerObject.Instance.transform.position);
        //播放移动动画
        animator.SetBool("Run", true);
    }
    /// <summary>
    /// 动画攻击事件
    /// </summary>
    public void AtkEvent()
    {
        //播放音效
        GameDataMgr.Instance.PlaySound("Eat");

        //攻击事件
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward, 2, LayerMask.GetMask("MainTower"));
        for (int i = 0; i < colliders.Length; i++)
        {
            //如果碰撞的是主塔
            if (colliders[i].gameObject == MainTowerObject.Instance.gameObject)
            {
                //主塔受伤
                MainTowerObject.Instance.Wound(monsterInfo.atk);
            }

        }
    }
}
