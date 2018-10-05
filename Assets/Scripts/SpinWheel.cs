using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheel : MonoBehaviour {

	public List<AnimationCurve> animationCurves; //动画曲线列表

	private bool spinning; //是否在旋转中
	private float anglePerItem;
	private int randomTime; //旋转时间
	private int itemNumber; //item个数

	private bool rotateCommand = false;
	private int targetItemIndex;
	private bool clockwise = true;
	private System.Action EndCallBack; //旋转结束回调

	// Use this for initialization
	void Start () {
		//避免没有预设曲线报错(这里建一条先慢再快再慢的动画曲线)
		if (animationCurves == null)
		{
			Keyframe[] ks = new Keyframe[3];
			ks[0] = new Keyframe(0, 0);
			ks[0].inTangent = 0;
			ks[0].outTangent = 0;
			ks[1] = new Keyframe(0.5f, 0.5f);
			ks[1].inTangent = 1;
			ks[1].outTangent = 1;
			ks[2] = new Keyframe(1, 1);
			ks[2].inTangent = 0;
			ks[2].outTangent = 0;
			AnimationCurve animationCurve = new AnimationCurve(ks);
			animationCurves.Add(animationCurve);
		}
	}

	/// <summary>
	/// 开启旋转调用（外部调用）
	/// </summary>
	/// <param name="itemNum"></param>
	/// <param name="itemIndex"></param>
	/// <param name="cw"></param>
	public void RotateUp(int itemNum,int itemIndex, bool cw, System.Action callback)
	{
		itemNumber = itemNum;
		anglePerItem = 360 / itemNumber;
		targetItemIndex = itemIndex;
		clockwise = cw;
		EndCallBack = callback;
		rotateCommand = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(rotateCommand && !spinning)
		{
			randomTime = Random.Range(4, 6); //随机获取旋转全角的次数

			float maxAngle = 360 * randomTime + (targetItemIndex * anglePerItem);
			rotateCommand = false;
			StartCoroutine(SpinTheWheel(randomTime, maxAngle));
		}
	}

	IEnumerator SpinTheWheel(float time, float maxAngle)
	{
		spinning = true;
		float timer = 0.0f;
		float startAngle = transform.eulerAngles.z;
		maxAngle = maxAngle - GetFitAngle(startAngle); //减去相对于0位置的偏移角度

		//根据顺时针逆时针不同，不同处理
		int cw_value = 1;
		if (clockwise)
		{
			cw_value = -1;
		}
		//获得随机旋转曲线
		int animationCurveNumber = Random.Range(0, animationCurves.Count);

		while (timer < time)
		{
			//计算旋转,动画曲线的Evaluate函数返回了给定时间下曲线上的值：从0到1逐渐变化，速度又每个位置的切线斜率决定。
			float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
			//得到的angle从0到最大角度逐渐变化 速度可变,让给加到旋转物角度上实现逐渐旋转 速度可变
			transform.eulerAngles = new Vector3(0.0f, 0.0f, cw_value * angle + startAngle);
			timer += Time.deltaTime;
			yield return 0;
		}

		//避免旋转有误，最终确保其在该在的位置
		transform.eulerAngles = new Vector3(0.0f, 0.0f, cw_value * maxAngle + startAngle);
		//执行回调 
		if (EndCallBack != null)
		{
			EndCallBack();
			EndCallBack = null;
        }
		//Invoke("AfterSpinning", 1);
		spinning = false;
	}

	private void AfterSpinning()
	{
		UIManager.instance.HUD.spinWheel.SetActive(false);
		UIManager.instance.HUD.openButton.interactable = true;
		GameManager.instance.gamePause = false;
	}

	private float GetFitAngle(float angle)
	{
		if (angle > 0)
		{
			if (angle - 360 > 0)
			{
				return GetFitAngle(angle - 360);
			}
			else
			{
				return angle;
			}
		}
		else
		{
			if (angle + 360 < 0)
			{
				return GetFitAngle(angle + 360);
			}
			else
			{
				return angle;
			}
		}
	}
}
