using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    //最大波数
    public int maxWave;
    //每波的怪物数量
    public int monsterCountPerWave;
    //怪物ID列表
    public List<int> monsterIDs;
    //单只怪物生成间隔时间
    public float monsterSpawnInterval;
    //波的间隔时间
    public float waveInterval;
    //第一波的延迟时间
    public float firstWaveDelay;

    //当前波要创建的怪物ID
    private int nowMonsterID;
    //当前波的怪物数量
    private int nowMonsterNum;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave", firstWaveDelay);
        GameLevelMgr.Instance.AddMonsterSpawnPoint(this);
        GameLevelMgr.Instance.UpdateMaxWaveNum(maxWave);
    }

    private void CreateWave()
    {
        //随机得到当前波的怪物ID,0~数组长度-1
        nowMonsterID = monsterIDs[Random.Range(0, monsterIDs.Count)];
        //当前波的怪物数量
        nowMonsterNum = monsterCountPerWave;
        //波数-1
        maxWave--;
        //开始生成怪物
        CreateMonster();

        
        //总波数-1
        GameLevelMgr.Instance.UpdateNowWaveNum(1);
    }
    private void CreateMonster()
    {
        //取出怪物数据
        MonsterInfo monsterInfo = GameDataMgr.Instance.monsterInfoList[nowMonsterID - 1];
        //创建怪物
        GameObject monsterObj = Instantiate(Resources.Load<GameObject>(monsterInfo.monterRes),this.transform);
        //给怪物添加MonsterObject组件
        MonsterObject monsterObject = monsterObj.AddComponent<MonsterObject>();
        //初始化怪物信息
        monsterObject.InitMonsterInfo(monsterInfo);

        //记录怪物数量+1
        GameLevelMgr.Instance.ChangeMonsterNum(true, monsterObject);

        //怪物数量-1
        nowMonsterNum--;
        //如果当前波的怪物数量为0
        if (nowMonsterNum == 0)
        {
            if(maxWave > 0)
            {
                //如果还有波数,延迟一段时间后创建下一波
                Invoke("CreateWave", waveInterval);
            }
            //else没有怪物需要创建了
        }
        else
        {
            Invoke("CreateMonster", monsterSpawnInterval);
        }

    }
    /// <summary>
    /// 外部调用,检查是否创建完所有波数的怪物
    /// </summary>
    /// <returns></returns>
    public bool CheckOver() 
    {
        //如果所有怪出完了是True,否则False
        return maxWave == 0 && nowMonsterNum == 0;
    }
    
}
