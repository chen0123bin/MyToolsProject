﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 步骤控制器，主要用于处理各种步骤中的变化效果
/// </summary>
public class SC_Active : BaseStepController
{
    [LabelText("控制对象"), LabelWidth(70), ValueDropdown("GetSceneObjectList"), HorizontalGroup]
    public string m_ObjName;
    [LabelText("开始Active"), LabelWidth(90)]
    public bool m_BeginActive;
    [LabelText("结束Active"), LabelWidth(90)]
    public bool m_EndActive;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        m_Target.gameObject.SetActive(m_BeginActive);
    }
    public override void ControllerEnd()
    {
        m_Target.gameObject.SetActive(m_EndActive);
    }
    public override void ControllerExecute()
    {
        m_ControllerExecuteCompleted?.Invoke();
    }

#if UNITY_EDITOR
    [Button("选中"), HorizontalGroup(30)]
    public void ChooseObj()
    {
        UnityEditor.Selection.activeObject = StepRuntimeData.Instance.FindGameObject(m_ObjName);
    }
#endif
}