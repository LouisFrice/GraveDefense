using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawnPoint : MonoBehaviour
{
    private GameObject towerObject;
    public TowerInfo nowTowerInfo;
    //可选的炮台ID列表
    public List<int> chooseIDs;

    //粒子系统控制
    private ParticleSystem particle;
    
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //如果已经有炮台并且没有下一级了，直接返回
        if (nowTowerInfo != null && nowTowerInfo.nextLevel == 0)
        {
            return;
        }
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }

    private void OnTriggerExit(Collider other)
    {
        //传空代表隐藏造塔点
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }

    public void CreateTower(int id)
    {
        TowerInfo towerInfo = GameDataMgr.Instance.towerInfoList[id - 1];
        //钱不够直接返回
        if (GameLevelMgr.Instance.player.money < towerInfo.money) { return; }

        //建造后隐藏粒子特效
        if (particle.isPlaying) { particle.Stop(); }


        //扣金币建造
        GameLevelMgr.Instance.player.AddMoney(-towerInfo.money);
        //如果有了就直接摧毁，升级炮台
        if (towerObject != null)
        {
            Destroy(towerObject);
            towerObject = null;
        }
        //创建新的炮台
        towerObject = GameObject.Instantiate(Resources.Load<GameObject>(towerInfo.res), this.transform.position,Quaternion.identity);
        //设置炮台信息
        towerObject.GetComponent<TowerObject>().InitInfo(towerInfo);
        //记录当前炮台信息
        nowTowerInfo = towerInfo;

        //造完塔，如果还能升级，显示升级UI
        if (nowTowerInfo.nextLevel != 0) 
        { 
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this); 
        }
        else
        {
            //如果没有下一级了，隐藏造塔点
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }


    }
}
