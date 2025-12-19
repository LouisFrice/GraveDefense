using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : BasePanel
{
    public Button btnClose;
    public Text txtTips;
    protected override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            //隐藏提示面板
            UIManager.Instance.HidePanel<TipsPanel>();
        });
    }
    public void ChangeTips(string tips)
    {
        //设置提示内容
        txtTips.text = tips;
    }
}
