﻿using LWFramework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LWFramework.UI {
    /// <summary>
    /// 所有的UI管理器
    /// </summary>
    //[ManagerClass(ManagerType.Normal)]
    public class UIManager : IManager, IUIManager
    {
        /// <summary>
        /// 所有的view字典
        /// </summary>
        protected Dictionary<string, IUIView> m_UIViewDic;

        /// <summary>
        /// 所有的绑定数据
        /// </summary>
        protected Dictionary<string, string> m_UIBindViewPath;

        #region 获取Canvas编辑节点

        private Transform _editTransform;
        private Transform EditTransform
        {
            get
            {
                if (_editTransform == null)
                {
                    _editTransform = GameObject.Find("LWFramework/Canvas/Edit").transform;
                }
                return _editTransform;
            }
        }

        #endregion
        public virtual void Init()
        {
            m_UIViewDic = new Dictionary<string, IUIView>();
            m_UIBindViewPath = new Dictionary<string, string>();
            //启动之后隐藏编辑层
            EditTransform.gameObject.SetActive(false);
        }
        /// <summary>
        /// 更新所有的View
        /// </summary>
        public void Update()
        {
            foreach (var item in m_UIViewDic.Values)
            {
                if (item.IsOpen)
                {
                    item.UpdateView();
                }
            }
        }
      
        public void BindView(string viewName, string uiGameObjectPath)
        {
            if (!m_UIBindViewPath.ContainsKey(viewName)) {
                m_UIBindViewPath.Add(viewName, uiGameObjectPath);
            }
        }
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">view的控制类</typeparam>
        /// <param name="isLastSibling">是否放置在最前面</param>
        public void OpenView<T>(bool isLastSibling = false)
        {
            IUIView uiViewBase;
            if (!m_UIViewDic.TryGetValue(typeof(T).ToString(), out uiViewBase))
            {
                uiViewBase = CreateView<T>();
                m_UIViewDic.Add(typeof(T).ToString(), uiViewBase);
            }
            uiViewBase.OpenView();

            uiViewBase.SetViewLastSibling(isLastSibling);
        }
        /// <summary>
        /// 打开View
        /// </summary>
        /// <typeparam name="T">view的控制类</typeparam>
        /// <param name="viewName">view的名字，用于一个多个页面共用一个类</param>
        /// <param name="uiGameObject">view的对象，提前创建，优先级高于自己创建</param>
        /// <param name="isLastSibling">是否放置在最前面</param>
        public void OpenView<T>(string viewName, GameObject uiGameObject = null , bool isLastSibling = false)
        {
            IUIView uiViewBase;
            if (!m_UIViewDic.TryGetValue(viewName, out uiViewBase))
            {
                if (m_UIBindViewPath.ContainsKey(viewName))
                {
                    uiViewBase = CreateView<T>(uiGameObject, m_UIBindViewPath[viewName]);
                }
                else {
                    uiViewBase = CreateView<T>(uiGameObject);
                }               
                m_UIViewDic.Add(viewName, uiViewBase);
            }
            uiViewBase.OpenView();
            uiViewBase.SetViewLastSibling(isLastSibling);
        }
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseView<T>()
        {
            CloseView(typeof(T).ToString());
        }
        /// <summary>
        /// 关闭View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseView(string viewName)
        {
            IUIView uiViewBase;
            if (m_UIViewDic.TryGetValue(viewName, out uiViewBase))
            {
                uiViewBase.CloseView();
            }
        }
        /// <summary>
        /// 获取VIEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetView<T>(string viewName = null)
        {
            if (viewName == null)
                return (T)m_UIViewDic[typeof(T).ToString()];
            else
                return (T)m_UIViewDic[viewName];
        }
        /// <summary>
        /// 获取所有的VIEW
        /// </summary>
        /// <returns></returns>
        public IUIView[] GetAllView() {
            return m_UIViewDic.Values.ToArray<IUIView>();
        }
        /// <summary>
        /// 关闭其他所有的View
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseOtherView<T>()
        {
            CloseOtherView(typeof(T).ToString());
        }
        public void CloseOtherView(string viewName) {
            foreach (var item in m_UIViewDic.Keys)
            {
                if (item != viewName)
                {
                    m_UIViewDic[item].CloseView();
                }
            }
        }
        /// <summary>
        /// 关闭所有的view
        /// </summary>
        public void CloseAllView()
        {
            foreach (var item in m_UIViewDic.Values)
            {
                item.CloseView();
            }
        }
        /// <summary>
        /// 清理所有的view
        /// </summary>
        public void ClearAllView()
        {
            foreach (var item in m_UIViewDic.Values)
            {
                item.ClearView();
            }
            m_UIViewDic.Clear();
        }

        /// <summary>
        /// 创建一个VIEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private BaseUIView CreateView<T>(GameObject uiGameObject = null,string loadPathParam = null)
        {
            BaseUIView uiView = Activator.CreateInstance(typeof(T)) as BaseUIView;

            //获取UIViewDataAttribute特性
            var attr = (UIViewDataAttribute)typeof(T).GetCustomAttributes(typeof(UIViewDataAttribute), true).FirstOrDefault();
            if (attr != null)
            {
                if (uiGameObject == null) {
                    string loadPath = attr.m_LoadPath;
                    //创建UI对象
                    if (loadPathParam != null)
                    {
                        loadPath = loadPathParam;
                    }
                    uiGameObject = UIUtility.Instance.CreateViewEntity(attr.m_LoadPath);
                }
              
                Transform parent = UIUtility.Instance.GetParent(attr.m_FindType, attr.m_Param);
                if (uiGameObject == null)
                {
                    LWDebug.LogError("没有找到这个UI对象" + attr.m_LoadPath);
                }
                if (parent == null)
                {
                    LWDebug.LogError("没有找到这个UI父节点" + attr.m_Param);
                }
                if (parent != null)
                {
                    uiGameObject.transform.SetParent(parent, false);
                }
                //初始化UI
                uiView.CreateView(uiGameObject);
            }
            else
            {
                LWDebug.Log("没有找到UIViewDataAttribute这个特性");
            }
            LWDebug.Log("UIManager：" + typeof(T).ToString());
            return uiView;
        }
        /// <summary>
        /// 创建一个VIEW
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [Obsolete("废弃的写法不用了")]
        private BaseUIView CreateView<T>(string viewName)
        {
            GameObject uiGameObject = UIUtility.Instance.CreateViewEntity(m_UIBindViewPath[viewName]);
            return CreateView<T>(uiGameObject);
        }

        public virtual Cysharp.Threading.Tasks.UniTask<T> OpenViewAsync<T>(bool isFirstSibling = false)
        {
            LWDebug.LogError("不支持异步打开View");
            throw new NotImplementedException();
        }
    }
}

