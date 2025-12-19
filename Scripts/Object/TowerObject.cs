using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Xml.Schema;
using UnityEngine;

public class TowerObject : MonoBehaviour
{
    //炮台头部，旋转攻击
    public Transform head;
    //炮台开火点
    public Transform firePoint;
    //炮台头部旋转速度
    private float roundSpeed = 20;

    //当前炮台的数据
    private TowerInfo towerInfo;
    //攻击目标
    private MonsterObject targetObj;
    //多个攻击目标
    private List<MonsterObject> targetObjs = new List<MonsterObject>();
    //攻击目标位置
    private Vector3 targetPos;
    //攻击间隔
    private float nowTime;

    
    void Update()
    {
        //单体攻击逻辑
        if (towerInfo.atkType == 1)
        {
            //目标为空，或者目标死亡，或者目标超出攻击范围，则重新寻找目标
            if (targetObj == null || targetObj.isDead ||  Vector3.Distance(this.transform.position,targetObj.transform.position) > towerInfo.atkRange )
            {
                targetObj = GameLevelMgr.Instance.GetMonsterObjcet(this.transform.position, towerInfo.atkRange);
            }

            //没有目标直接返回
            if (targetObj == null) { return; }

            //炮台头部朝向目标
            //防止炮台往怪物脚下瞄准
            targetPos = targetObj.transform.position;
            targetPos.y = head.transform.position.y;
            //怪物位置- 炮台头部位置
            //看向怪物同等高度的位置
            Quaternion targetRotation = Quaternion.LookRotation(targetPos - head.position);
            head.rotation = Quaternion.Slerp(head.rotation, targetRotation, roundSpeed * Time.deltaTime);

            //炮台对准目标开始攻击
            if (Vector3.Angle(head.forward , targetPos - head.position) < 5 && (Time.time - nowTime) >= towerInfo.offsetTime)
            {
                nowTime = Time.time;
                //攻击目标受伤
                targetObj.Wound(towerInfo.atk);
                //播放攻击音效
                GameDataMgr.Instance.PlaySound("Tower");
                //播放攻击特效
                GameObject eff = Instantiate(Resources.Load<GameObject>(towerInfo.eff),firePoint.position, firePoint.rotation);
                
                Destroy(eff, 0.2f);
            }
        }
        //群体攻击逻辑
        else
        {
            targetObjs = GameLevelMgr.Instance.GetMonsterObjects(this.transform.position, towerInfo.atkRange);
            if(targetObjs.Count > 0 && Time.time >= towerInfo.offsetTime)
            {
                //怪物受伤
                for (int i = 0; i < targetObjs.Count; i++)
                {
                    targetObjs[i].Wound(towerInfo.atk);
                }

                nowTime = Time.time;
                //播放攻击音效
                GameDataMgr.Instance.PlaySound("Tower");
                //播放攻击特效
                GameObject eff = Instantiate(Resources.Load<GameObject>(towerInfo.eff), firePoint.position, firePoint.rotation);
                Destroy(eff, 1f);
            }
        }
    }

    public void InitInfo(TowerInfo Info)
    {
        this.towerInfo = Info;
    }
}
