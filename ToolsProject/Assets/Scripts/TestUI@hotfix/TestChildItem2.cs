﻿using LWFramework.UI;
using UnityEngine.UI;
using UnityEngine;

[UIViewData("TestChildItem2", LWFramework.UI.UILayer.local)]
public class TestChildItem2 : LWFramework.UI.BaseUIView  
{

	[UIElement("HeadImg", "Assets/Res/Runtime/Sprites/log3.png")]
	public Image _HeadImg;
	[UIElement("NameText")]
	public Text _NameText;
	public override  void CreateView(GameObject gameObject)
	{
		base.CreateView(gameObject);
	}
}