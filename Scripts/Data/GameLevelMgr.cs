using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameLevelMgr 
{
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;

    public PlayerObject player;

    //记录所有的出怪点
    public List<MonsterSpawnPoint> monsterSpawnPoints = new List<MonsterSpawnPoint>();
    //这里是各个出怪点加起来的总波数
    //记录当前还有多少波怪物
    public int nowWaveCount = 0;
    //记录最大波数
    public int maxWaveCount = 0;
    ////记录场景上的怪物数量
    //public int nowMonsterNum = 0;
    
    //记录所有怪物信息
    public List<MonsterObject> monsters = new List<MonsterObject>();

    private GameLevelMgr()
    {

    }

    public void InitGameLevel(SceneInfo sceneInfo)
    {
        UIManager.Instance.ShowPanel<GamePanel>();
        //实例化玩家角色
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRoleInfo;
        Transform playerSpawnPoint = GameObject.Find("PlayerSpawnPoint").transform;
        GameObject playerObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),playerSpawnPoint);
        //让摄像机跟随玩家
        Camera.main.GetComponent<CameraPlayer>().SetCameraTarget(playerObj.transform);
        //获取玩家组件
        player = playerObj.GetComponent<PlayerObject>();
        //初始化玩家信息
        player.InitPlayerInfo(roleInfo.atk, sceneInfo.money);
        //初始化塔血量
        MainTowerObject.Instance.UpdateHp(sceneInfo.towerHp, sceneInfo.towerHp);

    }

    /// <summary>
    /// 存储每个怪物出生点
    /// </summary>
    /// <param name="monsterSpawnPoint"></param>
    public void AddMonsterSpawnPoint(MonsterSpawnPoint monsterSpawnPoint)
    {
        monsterSpawnPoints.Add(monsterSpawnPoint);
    }

    public void UpdateMaxWaveNum(int num)
    {

        //各个出怪点加起来的总波数
        maxWaveCount += num;
        nowWaveCount = maxWaveCount;
        //更新UI
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(nowWaveCount, maxWaveCount);
    }

    public void UpdateNowWaveNum(int num)
    {
        //各个出怪点加起来的剩余波数-1
        nowWaveCount -= num;
        //更新UI
        UIManager.Instance.GetPanel<GamePanel>().UpdateWave(nowWaveCount, maxWaveCount);
    }

    /// <summary>
    /// 改变怪物数组数量
    /// </summary>
    /// <param name="isAdd">true是添加，false是移除</param>
    /// <param name="monsterObject">传入的怪物obj</param>
    public void ChangeMonsterNum(bool isAdd, MonsterObject monsterObject)
    {
        if (isAdd)
        {
            monsters.Add(monsterObject);
        }
        else
        {
            monsters.Remove(monsterObject);
        }
    }

    public bool CheckGameOver()
    {
        for (int i = 0; i < monsterSpawnPoints.Count; i++)
        {
            //检查每个怪物出生点是否还有怪物
            //如果所有怪出完了是True,否则False
            //如果怪还没出完，返回false
            if (!monsterSpawnPoints[i].CheckOver())
            {
                return false;
            }
        }

        //是否还有怪物存在
        if (monsters.Count > 0)
        {
            return false;
        }

        //出生点没有要出的怪且怪物都死完了
        //游戏胜利
        return true;
    }


    /// <summary>
    /// 清空数据，防止影响下一次游戏
    /// </summary>
    public void ClearData()
    {
        player = null;
        monsterSpawnPoints.Clear();
        nowWaveCount = maxWaveCount = 0;
        monsters.Clear();

    }

    /// <summary>
    /// 返回一个攻击范围里的怪物对象
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="Range"></param>
    /// <returns></returns>
    public MonsterObject GetMonsterObjcet(Vector3 TowerPos , int atkRange)
    {
        //在怪物列表中找到满足攻击距离的怪物
        for (int i = 0; i < monsters.Count; i++)
        {
            //如果怪物在攻击范围内
            if (Vector3.Distance(TowerPos, monsters[i].transform.position) <= atkRange && !monsters[i].isDead)
            {
                return monsters[i];
            }
        }
        //不在范围内，返回Null
        return null;
    }

    /// <summary>
    /// 返回范围内的所有对象
    /// </summary>
    /// <returns></returns>
    public List<MonsterObject> GetMonsterObjects(Vector3 TowerPos, int atkRange)
    {
        List<MonsterObject> monsterList = new List<MonsterObject>();
        //在怪物列表中找到满足攻击距离的怪物
        for (int i = 0; i < monsters.Count; i++)
        {
            //如果怪物在攻击范围内
            if (Vector3.Distance(TowerPos, monsters[i].transform.position) <= atkRange && !monsters[i].isDead)
            {
                monsterList.Add(monsters[i]);
            }
        }

        return monsterList;
    }
}
