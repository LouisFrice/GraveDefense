using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SelScenePanel : BasePanel
{
    public Button btnLast;
    public Button btnNext;
    public Button btnStart;
    public Button btnBack;
    
    public Text txtSceneDes;

    public Image imgScene;

    //场景信息
    private SceneInfo nowSceneInfo;
    private int nowIndex = 0;
    private List<SceneInfo> sceneInfoList;
    //场景图片图集
    private SpriteAtlas SpriteAtlas;

    protected override void Init()
    {
        //获取场景信息列表
        sceneInfoList = GameDataMgr.Instance.sceneInfoList;
        //获取场景图集
        SpriteAtlas = Resources.Load<SpriteAtlas>("SceneImg/SceneAtlas");
        //初始化场景信息列表
        ChangSceneInfo();
        btnStart.onClick.AddListener(() =>
        {
            //开始游戏
            //记录当前选择的场景
            //GameDataMgr.Instance.nowSelSceneInfo = nowSceneInfo;
            //隐藏场景面板
            UIManager.Instance.HidePanel<SelScenePanel>();
            //切换游戏界面
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            ao.completed += (e) =>
            {
                //加载完成后初始化游戏关卡
                GameLevelMgr.Instance.InitGameLevel(nowSceneInfo);
            };
            

        });
        btnBack.onClick.AddListener(() =>
        {
            //返回选角面板
            UIManager.Instance.HidePanel<SelScenePanel>();
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        btnNext.onClick.AddListener(() =>
        {
            //下一个场景
            ++nowIndex;
            if (nowIndex > sceneInfoList.Count - 1)
            {
                nowIndex = 0;
            }
            //更新场景面板
            ChangSceneInfo();
        });
        btnLast.onClick.AddListener(() =>
        {
            //上一个场景
            --nowIndex;
            if (nowIndex < 0)
            {
                nowIndex = sceneInfoList.Count - 1;
            }
            //更新场景面板
            ChangSceneInfo();
        });
    }
    private void ChangSceneInfo()
    {
        
        //当前选择的场景面板信息
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];

        //更新场景图片
        imgScene.sprite = SpriteAtlas.GetSprite(nowSceneInfo.imgRes);
        //更新场景描述文本
        txtSceneDes.text = nowSceneInfo.des;


    }
}
