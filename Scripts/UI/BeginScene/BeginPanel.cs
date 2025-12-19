using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnAbout;
    public Button btnQuit;
    protected override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //隐藏当前面板
            UIManager.Instance.HidePanel<BeginPanel>();
            //播放左转动画
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() =>
            {
                //动画播放完成后显示选角面板
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });
        });
        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        btnAbout.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<AboutPanel>();
        });
        btnQuit.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
