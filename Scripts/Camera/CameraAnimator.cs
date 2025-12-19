using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimator : MonoBehaviour
{
    private Animator animator;
    private UnityAction UnityAction;//动画完成后调用的函数

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //动画播放完成会调用的事件
    public void PlayOver()
    {
        UnityAction?.Invoke();
        //执行完置空
        UnityAction = null;
    }

    //左转
    public void TurnLeft(UnityAction callBack)
    {
        //播放左转动画
        animator.SetTrigger("TurnLeft");
        UnityAction = callBack;
    }

    //右转
    public void TurnRight(UnityAction callBack)
    {
        animator.SetTrigger("TurnRight");
        UnityAction = callBack;
    }
}
