using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestResultPopup : UIScreen {

	public Text resultText;
	private Action onOkButton;

	public void Init(string resultMessage, Action onOkButton)
	{
		resultText.text = resultMessage;
		this.onOkButton = onOkButton;
	}

	public void OnOkButton()
	{
		UIManager.instance.Pop();
		if (onOkButton != null)
			onOkButton();
	}

}
