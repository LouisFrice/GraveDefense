using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    public Button btnLast;
    public Button btnNext;
    public Button btnStart;
    public Button btnBack;
    public Button btnUnlock;
    public Text txtUnlock;
    public Text txtHeroDes;
    public Text txtMoney;

    //场景上英雄位置和对象
    private Transform heroPos;
    private GameObject heroObj;
    //当前选择的英雄
    private RoleInfo nowRoleInfo;
    private int nowIndex = 0;
    //获取英雄列表
    private List<RoleInfo> roleInfoList;

    protected override void Init()
    {
        //英雄列表
        roleInfoList = GameDataMgr.Instance.roleInfoList;
        //英雄放置的位置
        heroPos = GameObject.Find("HeroPos").transform;
        //更新玩家目前的金币数量
        txtMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();
        //实例化默认英雄
        ChangHero();
        btnStart.onClick.AddListener(() => 
        {
            //开始游戏
            //记录当前选择的角色
            GameDataMgr.Instance.nowSelRoleInfo = nowRoleInfo;
            //隐藏选角面板
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            //切换选择场景面板
            UIManager.Instance.ShowPanel<SelScenePanel>();

        });
        btnBack.onClick.AddListener(() =>
        {
            //返回主界面
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            //播放左转动画
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                //动画播放完成后显示开始面板
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });
        btnNext.onClick.AddListener(() =>
        {
            //下一个角色
            ++nowIndex;
            if(nowIndex > roleInfoList.Count - 1)
            {
                nowIndex = 0;
            }
            //更新模型
            ChangHero();
        });
        btnLast.onClick.AddListener(() =>
        {
            //上一个角色
            --nowIndex;
            if (nowIndex < 0)
            {
                nowIndex = roleInfoList.Count - 1;
            }
            //更新模型
            ChangHero();
        });
        btnUnlock.onClick.AddListener(() =>
        {
            //解锁角色
            PlayerData playerData = GameDataMgr.Instance.playerData;
            if (playerData.haveMoney >= nowRoleInfo.money)
            {
                playerData.haveMoney -= nowRoleInfo.money;
                //更新剩余的金币数量
                txtMoney.text = playerData.haveMoney.ToString();
                //添加到已购买角色列表
                playerData.buyHero.Add(nowIndex);
                //更新解锁按钮
                UpdateLockBtn();
                //保存玩家数据
                GameDataMgr.Instance.SavePlayerData();
                //提示面板显示购买成功
                UIManager.Instance.ShowPanel<TipsPanel>().ChangeTips("购买成功！\n已解锁角色：" + nowRoleInfo.des);
               
            }
            else
            {
                //提示面板显示金币不足
                UIManager.Instance.ShowPanel<TipsPanel>().ChangeTips("金钱不足\n还差$" + (nowRoleInfo.money - playerData.haveMoney));
            }
        });

    }

    private void ChangHero()
    {
        //每次切换之前先销毁
        if (heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
        //当前选择的角色信息
        nowRoleInfo = GameDataMgr.Instance.roleInfoList[nowIndex];
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleInfo.res),heroPos);
        
        
        //更新英雄描述文本
        txtHeroDes.text = nowRoleInfo.des;

        //移除玩家的PlayerObject组件
        Destroy(heroObj.GetComponent<PlayerObject>());

        //更新解锁按钮
        UpdateLockBtn();
    }
    private void UpdateLockBtn()
    {
        //如果当前角色需要购买且玩家没有买过
        if(nowRoleInfo.money > 0 && !GameDataMgr.Instance.playerData.buyHero.Contains(nowRoleInfo.id))
        {
            btnUnlock.gameObject.SetActive(true);
            //更新解锁价格
            txtUnlock.text =  "$ "+ nowRoleInfo.money;
            //没解锁过角色，隐藏开始按钮
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            //已经解锁该角色
            btnUnlock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }
    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        //销毁当前英雄模型
        if (heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
    }
}
