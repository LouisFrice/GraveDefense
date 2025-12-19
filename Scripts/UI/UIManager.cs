using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UIManager 
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    private Transform canvasTrans;

    private UIManager()
    {
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);
        //Debug.Log("已调用");
    }
    public T ShowPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        
            
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }

        GameObject panelObject = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObject.transform.SetParent(canvasTrans, false);
        T panel = panelObject.GetComponent<T>();
        panelDic.Add(panelName, panel);
        panel.ShowMe();

        return panel;
    }
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //淡出后销毁
                panelDic[panelName].HideMe(() =>
                {
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //直接销毁
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }
        }

    }
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;


        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        return null;
    }
}
