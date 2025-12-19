using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerObject : MonoBehaviour
{
    //1.玩家属性
    //玩家攻击力
    private int atk;
    //目前金额
    public int money;
    //旋转速度
    public float rotateSpeed = 70;
    //开火点
    public Transform firePoint;

    //2.玩家移动动画
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        //animator.speed = 1.2f;
    }


    void Update()
    {
        //左shift跑步
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            animator.speed = 1.6f;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            animator.speed = 1.2f;
        }

        //玩家移动
        //1-动画变量，2-替换的动画变量
        animator.SetFloat("VerticalSpeed", Input.GetAxis("Vertical"));
        animator.SetFloat("HorizontalSpeed", Input.GetAxis("Horizontal"));
        //玩家旋转
        this.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") *rotateSpeed*Time.deltaTime );

        if (this.gameObject.name == "Heavy(Clone)" || this.gameObject.name == "Rifle(Clone)")
        {
            //鼠标左键
            if (Input.GetMouseButton(0))
            {
                //重武器开火
                animator.SetTrigger("Fire");//重武器开火
            }
        }
        else
        {
            //鼠标左键
            if (Input.GetMouseButtonDown(0))
            {
                //轻武器开火
                animator.SetTrigger("Fire");//轻武器开火
            }
            //animator.SetTrigger("Fire");//鼠标左键开枪
        }
        //空格翻滚
        if (Input.GetKeyDown(KeyCode.Space)) 
        { 
            animator.SetTrigger("Roll");//空格翻滚
        }
        //切换蹲伏，1是蹲伏的层级
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Crouch Layer"), 1);
        }
        //抬起取消蹲伏
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(animator.GetLayerIndex("Crouch Layer"), 0);
        }
    }

    public void InitPlayerInfo(int atk, int money)
    {
        this.atk = atk;
        this.money = money;
        //更新UI金钱
        UpdateMoney();
    }

    public void KnifeEvent()
    {
        //播放音效
        GameDataMgr.Instance.PlaySound("Knife");

        //伤害检测
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up, 2, LayerMask.GetMask("Monster"));
        for(int i = 0; i < colliders.Length; i++)
        {
            //碰撞到怪物，怪物受伤
            MonsterObject monster = colliders[i].GetComponent<MonsterObject>();
            //如果碰撞到的怪物不为空并且没有死亡,防止重复打死同一个怪物
            if (monster != null && !monster.isDead)
            {
                monster.Wound(this.atk);
                //只打中第一个怪物
                //break;
            }

        }
    }

    public void ShootEvent()
    {
        if (this.gameObject.name == "Rocket(Clone)")
        {
            //播放RPG音效
            GameDataMgr.Instance.PlaySound("RPG");
        }
        else
        {
            //播放枪声音效
            GameDataMgr.Instance.PlaySound("Gun");
        }

        RaycastHit[] hits = Physics.RaycastAll(new Ray(firePoint.position,firePoint.forward), 1000, LayerMask.GetMask("Monster"));
        for (int i = 0; i < hits.Length; i++)
        {
            //碰撞到怪物，怪物受伤
            MonsterObject monster = hits[i].collider.GetComponent<MonsterObject>();
            //如果碰撞到的怪物不为空并且没有死亡,防止重复打死同一个怪物
            if (monster != null && !monster.isDead)
            {
                //播放特效
                GameObject hitEff = GameObject.Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRoleInfo.hitEff),
                                    hits[i].point, Quaternion.LookRotation(hits[i].normal));
                Destroy(hitEff, 1);

                monster.Wound(this.atk);
                //只打中第一个怪物
                break;
            }
                
        }
    }

    public void UpdateMoney()
    {
        //更新UI金钱
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }
    public void AddMoney(int money)
    {
        //加钱顺便更新UI
        this.money += money;
        UpdateMoney();
    }

}
