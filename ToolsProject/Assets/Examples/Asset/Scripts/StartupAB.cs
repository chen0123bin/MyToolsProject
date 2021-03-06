﻿using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class StartupAB : MonoBehaviour
{
    public static Action OnStart { get; set; }
    public static Action OnUpdate { get; set; }
    void Start()
    {

        DontDestroyOnLoad(gameObject);      
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(IUIManager).ToString(), new UIManager());    
        MainManager.Instance.AddManager(typeof(IFSMManager).ToString(), new FSMManager());
        MainManager.Instance.AddManager(typeof(HotfixManager).ToString(), new HotfixManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        ABAssetsManger abAssetManger = new ABAssetsManger();
        abAssetManger.ABInitUpdate = new ABInitUpdate();
        MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), abAssetManger);

        MainManager.Instance.GetManager<IAssetsManager>().OnInitUpdateComplete = OnUpdateCallback;
    }
   
    /// <summary>
    /// 默认资源更新完成
    /// </summary>
    /// <param name="obj"></param>
    private void OnUpdateCallback(bool obj)
    {
        StartCoroutine(MainManager.Instance.GetManager<HotfixManager>().IE_LoadScript((HotfixCodeRunMode)LWUtility.GlobalConfig.hotfixCodeRunMode));
    } 
    // Update is called once per frame
    void Update()
    {
        MainManager.Instance.Update();
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

   
    void OnDestroy()
    {
    }

    private void OnApplicationQuit()
    {

    }



}
