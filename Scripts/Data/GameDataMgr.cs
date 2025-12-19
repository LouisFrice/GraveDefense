using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;

    public MusicData musicData;
    //角色数据
    public List<RoleInfo> roleInfoList;
    //玩家数据
    public PlayerData playerData;
    //当前选择的角色
    public RoleInfo nowSelRoleInfo;
    //场景数据
    public List<SceneInfo> sceneInfoList;
    //怪物数据
    public List<MonsterInfo> monsterInfoList;
    //炮台数据
    public List<TowerInfo> towerInfoList;

    //当前选择的场景
    //public SceneInfo nowSelSceneInfo;
    private GameDataMgr()
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }

    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }

    public void PlaySound(string audioName)
    {
        GameObject audioObj = new GameObject("Sound");
        AudioSource audioSource = audioObj.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load<AudioClip>("Music/"+ audioName);
        audioSource.volume = musicData.soundVolume;
        audioSource.mute = !musicData.isSoundOn;
        audioSource.Play();

        //延迟一秒摧毁音效对象
        GameObject.Destroy(audioObj,1);
    }
}
