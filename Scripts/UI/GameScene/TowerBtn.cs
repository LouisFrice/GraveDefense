using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    public Image imgPic;
    public Text txtMoney;
    public Text txtNum;

    /// <summary>
    /// 初始化炮台按钮信息
    /// </summary>
    /// <param name="id">炮台ID</param>
    /// <param name="inputNum">按键数字</param>
    public void InitInfo(int id , int inputNum)
    {
        TowerInfo towerinfo = GameDataMgr.Instance.towerInfoList[id - 1];
        imgPic.sprite = Resources.Load<Sprite>(towerinfo.imgRes);
        txtNum.text = "数字键" + inputNum;
        //升级金币大于玩家拥有的金币，显示金钱不足
        txtMoney.text = towerinfo.money > GameLevelMgr.Instance.player.money ? "金钱不足" : "$" + towerinfo.money;
        
    }

    
}
