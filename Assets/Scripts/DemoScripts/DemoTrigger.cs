using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoTrigger : MonoBehaviour
{
	public GameObject Obj;

	void Start()
	{
		OnAnimationTrigger();
	}

	private void OnAnimationTrigger()
	{
		for (int i = 0; i < this.transform.childCount; i++)
		{
			this.transform.GetChild(i).SendMessage("Msg_OnTriggerEnter", Obj, SendMessageOptions.DontRequireReceiver);
		}
	}
}
