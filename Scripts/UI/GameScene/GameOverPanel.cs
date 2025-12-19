using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Button btnComfirm;
    public Text txtWinOrLose;
    public Text txtMoney;


    protected override void Init()
    {
        btnComfirm.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();

            GameLevelMgr.Instance.ClearData();//清空本局数据
            SceneManager.LoadScene("BeginScene");

            
        });
    }

    public void ChangeInfo(bool isWin)
    {
        //通关拿场景的金币奖励，失败拿一半
        int money = isWin ? GameLevelMgr.Instance.player.money : GameLevelMgr.Instance.player.money/2;
        txtWinOrLose.text = isWin? "通关奖励" : "失败奖励";
        txtMoney.text = "$ " + money;

        //保存当前获得的金币
        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }

    public override void ShowMe()
    {
        base.ShowMe();
        //显示结束面板时，取消锁定鼠标
        UIManager.Instance.GetPanel<GamePanel>().ShowCursor();
        UIManager.Instance.GetPanel<GamePanel>().isAllowCursorHidden = false;
    }
}
