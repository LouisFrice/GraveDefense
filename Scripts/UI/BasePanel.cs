using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private float alphaSpeed = 10;
    private bool isShow;
    private UnityAction hideMeCallBack;//隐藏结束后回调
    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) 
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

    }
    protected virtual void Start()
    {
        Init();
    }

    protected abstract void Init();

    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }

    public virtual void HideMe(UnityAction callBack)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hideMeCallBack = callBack;
    }
    protected virtual void Update()
    {
        //淡入
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha >= 1) { canvasGroup.alpha = 1; }

        }
        //淡出 
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaSpeed;
            if (canvasGroup.alpha <= 0)
            { 
                canvasGroup.alpha = 0; 
                hideMeCallBack?.Invoke();
            };

        }
    }
}
