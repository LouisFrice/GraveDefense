using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTowerObject : MonoBehaviour
{
    private static MainTowerObject instance;
    public static MainTowerObject Instance => instance;

    private int hp;
    private int maxHp;
    private bool isDead;


    private void Awake()
    {
        instance = this;
    }


    public void UpdateHp(int hp, int maxHp)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        UIManager.Instance.GetPanel<GamePanel>().UpdateHp(hp, maxHp);
    }

    public void Wound(int dmg)
    {
        if (isDead)
        {
            return;
        }
         
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            //游戏失败，显示结束面板并存储当前金币，失败金币/2
            UIManager.Instance.ShowPanel<GameOverPanel>().ChangeInfo(false);
        }
        UpdateHp(hp, maxHp);
    }

    private void OnDestroy()
    {
        instance = null;//切换场景主动销毁
    }
}
