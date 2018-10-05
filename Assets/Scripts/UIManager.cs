using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> {

	public HUD HUD;

	private Dictionary<Type, UIScreen> typeToScreen = new Dictionary<Type, UIScreen>();
	private Stack<UIScreen> screenStack = new Stack<UIScreen>();

	protected override void Awake()
	{
		base.Awake();
		//DontDestroyOnLoad(this.gameObject);

		UIScreen[] screens = GetComponentsInChildren<UIScreen>(true);
		foreach(UIScreen screen in screens)
		{
			screen.gameObject.SetActive(false);
			typeToScreen.Add(screen.GetType(), screen);
		}
		Push(typeof(HUD));
		
	}

	public T Push<T> () where T : UIScreen
	{
		UIScreen newScreen = Push(typeof(T));
		return (T)newScreen;
	}

	public UIScreen Push(Type screenType)
	{
		if(screenStack.Count > 0)
		{
			UIScreen oldScreen = screenStack.Peek();
			oldScreen.gameObject.SetActive(false);
		}

		UIScreen newScreen = typeToScreen[screenType];
		newScreen.gameObject.SetActive(true);
		screenStack.Push(newScreen);
		return newScreen;
	}

	public void Pop()
	{
		UIScreen topScreen = screenStack.Pop();
		topScreen.gameObject.SetActive(false);

		UIScreen newTopScreen = screenStack.Peek();
		newTopScreen.gameObject.SetActive(true);
	}

}
