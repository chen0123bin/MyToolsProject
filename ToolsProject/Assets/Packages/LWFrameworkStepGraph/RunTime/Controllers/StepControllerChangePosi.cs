﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// 步骤控制器，处理位移
/// </summary>
public class StepControllerChangePosi:BaseStepController
{
    [LabelText("移动时间"), LabelWidth(70)]
    public float m_MoveTime;
    [LabelText("移动位置"), LabelWidth(70)]
    public Vector3[] m_PosiArray;
    private Transform m_Target;
    public override void ControllerBegin()
    {
        m_Target = StepRuntimeData.Instance.FindGameObject(m_ObjName).transform;
        if (m_PosiArray.Length < 2) {
            LWDebug.LogError("当前节点的Controller的移动参数少于2个");
        }
        m_Target.position = m_PosiArray[0];
    }

    public override void ControllerEnd()
    {
        m_Target.position = m_PosiArray[m_PosiArray.Length-1];
    }

    public override void ControllerExecute()
    {
        m_Target.DOPath(m_PosiArray, m_MoveTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            m_ControllerCompleted?.Invoke();
        });     
    }

   
}