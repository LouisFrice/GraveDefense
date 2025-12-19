using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle toggleMusic;
    public Toggle toggleSound;
    public Slider sliderMusic;
    public Slider sliderSound;

    protected override void Init()
    {
        //初始化面板内容，每次开关面板都会调用
        toggleMusic.isOn = GameDataMgr.Instance.musicData.isMusicOn;
        toggleSound.isOn = GameDataMgr.Instance.musicData.isSoundOn;
        sliderMusic.value = GameDataMgr.Instance.musicData.musicVolume;
        sliderSound.value = GameDataMgr.Instance.musicData.soundVolume;


        btnClose.onClick.AddListener(() => 
        {
            UIManager.Instance.HidePanel<SettingPanel>();
            //关闭面板时保存音乐数据
            GameDataMgr.Instance.SaveMusicData();
        });

        toggleMusic.onValueChanged.AddListener((isOn) =>
        {
            BKMusic.Instance.SetIsOpen(isOn);
            GameDataMgr.Instance.musicData.isMusicOn = isOn;
            
        });
        toggleSound.onValueChanged.AddListener((isOn) =>
        {
            GameDataMgr.Instance.musicData.isSoundOn = isOn;
        });
        sliderMusic.onValueChanged.AddListener((value) =>
        {
            BKMusic.Instance.SetVolume(value);
            GameDataMgr.Instance.musicData.musicVolume = value;
        });
        sliderSound.onValueChanged.AddListener((value) =>
        {
            GameDataMgr.Instance.musicData.soundVolume = value;
        });
    }

}
