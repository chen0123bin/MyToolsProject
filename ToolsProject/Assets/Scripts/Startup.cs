﻿using LWFramework.Core;
using LWFramework.FMS;
using LWFramework.Message;
using LWFramework.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class Startup : MonoBehaviour
{
    public static Action OnStart { get; set; }
    public static Action OnUpdate { get; set; }
    void Start()
    {
        string path = "Assets/@Resources/Scenes/TestScene.unity";
        string aFirstName = path.Substring(path.LastIndexOf("/") + 1, (path.LastIndexOf(".") - path.LastIndexOf("/") - 1));
        LWDebug.Log(aFirstName);

        DontDestroyOnLoad(gameObject);      
        MainManager.Instance.Init();
        //添加各种管理器
        MainManager.Instance.AddManager(typeof(UIManager).ToString(), new UIManager());    
        MainManager.Instance.AddManager(typeof(FSMManager).ToString(), new FSMManager());
        MainManager.Instance.AddManager(typeof(HotfixManager).ToString(), new HotfixManager());
        MainManager.Instance.AddManager(typeof(GlobalMessageManager).ToString(), new GlobalMessageManager());
        if (LWUtility.GlobalConfig.assetMode == AssetMode.AssetBundle || LWUtility.GlobalConfig.assetMode == AssetMode.AssetBundleDev)
        {
            ABAssetsManger abAssetManger = new ABAssetsManger();
            abAssetManger.ABInitUpdate = new ABInitUpdate();
            MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), abAssetManger);
        }
        else if (LWUtility.GlobalConfig.assetMode == AssetMode.Resources)
        {
            MainManager.Instance.AddManager(typeof(IAssetsManager).ToString(), new ResAssetsManger());
        }

        MainManager.Instance.GetManager<IAssetsManager>().OnUpdateCallback = OnUpdateCallback;
    }
    /// <summary>
    /// 默认资源更新完成
    /// </summary>
    /// <param name="obj"></param>
    private void OnUpdateCallback(bool obj)
    {
        StartCoroutine(MainManager.Instance.GetManager<HotfixManager>().IE_LoadScript(LWUtility.GlobalConfig.hotfixCodeRunMode));
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
