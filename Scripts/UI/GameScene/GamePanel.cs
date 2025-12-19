using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel
{
    public Button btnQuit;
    public Image imgHp;
    public Text txtHp;
    public Text txtWave;
    public Text txtMoney;

    //造塔点UI的父物体
    public GameObject towerCollection;
    //towerBtn的列表
    public List<TowerBtn> towerBtnList = new List<TowerBtn>();

    //当前进入的造塔点
    private TowerSpawnPoint nowSelTowerPoint;

    //是否可以造塔
    private bool canCreateTower;

    private bool isCursorHidden; // 记录当前鼠标状态
    public bool isAllowCursorHidden; // 允许隐藏鼠标

    protected override void Init()
    {
        //隐藏造塔UI
        towerCollection.SetActive(false);

        btnQuit.onClick.AddListener(() =>
        {
            //返回主界面，显示鼠标
            ShowCursor();
            isAllowCursorHidden = false;

            UIManager.Instance.HidePanel<GamePanel>();
            SceneManager.LoadScene("BeginScene");
        });

        //隐藏并锁定鼠标在窗口内
        HideCursor();
        isAllowCursorHidden = true;
    }
    /// <summary>
    /// 更新血量
    /// </summary>
    /// <param name="hp">当前血量</param>
    /// <param name="MaxHp">最大血量</param>
    public void UpdateHp(int hp , int MaxHp)
    {
        txtHp.text = hp + "/" + MaxHp;
        //sizeDelta表示受到锚点影响的宽高，500是初始宽度
        imgHp.rectTransform.sizeDelta = new Vector2( (float)hp/MaxHp * 500 , imgHp.rectTransform.sizeDelta.y );
    }
    /// <summary>
    /// 更新剩余波数
    /// </summary>
    /// <param name="nowNum">当前波数</param>
    /// <param name="MaxNum">最大波数</param>
    public void UpdateWave(int nowNum, int MaxNum)
    {
        txtWave.text = nowNum + "/" + MaxNum;
    }
    /// <summary>
    /// 更新金币数量
    /// </summary>
    /// <param name="money">获得的金币</param>
    public void UpdateMoney(int money)
    {
        txtMoney.text = money.ToString();
    }

    /// <summary>
    /// 更新当前进入的造塔点的界面信息
    /// </summary>
    /// <param name="point"></param>
    public void UpdateSelTower(TowerSpawnPoint point)
    {
        nowSelTowerPoint = point;

        //如果传入的点为空，代表隐藏造塔点
        if (point == null)
        {
            //拒绝按键输入造塔
            canCreateTower = false;
            towerCollection.SetActive(false);
            return;
        }
        //允许按键输入造塔
        canCreateTower = true;
        //显示造塔点UI
        towerCollection.SetActive(true);
        //如果没有造过塔，就显示并更新3个造塔点
        if (nowSelTowerPoint.nowTowerInfo == null)
        {
            for (int i = 0; i < towerBtnList.Count; i++)
            {
                towerBtnList[i].gameObject.SetActive(true);
                towerBtnList[i].InitInfo(point.chooseIDs[i], i + 1);
            }
        }
        else
        {
            //如果造过塔了，就隐藏造塔点
            for (int i = 0; i < towerBtnList.Count; i++)
            {
                towerBtnList[i].gameObject.SetActive(false);
            }
            //显示中间造塔点，升级造塔点
            towerBtnList[1].gameObject.SetActive(true);
            towerBtnList[1].InitInfo(nowSelTowerPoint.nowTowerInfo.nextLevel, 1);

        }
    }

    protected override void Update()
    {
        //鼠标相关
        // 按ESC键呼出鼠标
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowCursor();
        }
        // 按左键隐藏鼠标（仅在鼠标显示时有效）
        else if (Input.GetMouseButtonDown(0) && !isCursorHidden && isAllowCursorHidden
                 && !EventSystem.current.IsPointerOverGameObject())
        {
                // 检查鼠标是否在UI上,不在UI上才隐藏鼠标
                HideCursor();
        }

        base.Update();
        //如果不允许造塔，直接返回
        if (!canCreateTower) { return; }
        //按键造塔
        //如果没有造过塔就按123键来选择塔的类型
        if (nowSelTowerPoint.nowTowerInfo == null)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.chooseIDs[2]);
            }

        }
        //如果造过塔就显示1键来升级
        else
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                nowSelTowerPoint.CreateTower(nowSelTowerPoint.nowTowerInfo.nextLevel);
            }
        }
        
    }

    // 隐藏鼠标
    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标到屏幕中心（射击游戏常用）
        Cursor.visible = false;
        isCursorHidden = true;
    }

    // 显示鼠标
    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None; // 解锁鼠标
        Cursor.visible = true;
        isCursorHidden = false;
    }

}
