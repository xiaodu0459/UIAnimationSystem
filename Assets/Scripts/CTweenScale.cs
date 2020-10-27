using UnityEngine;
using System.Collections;

public class CTweenScale : MonoBehaviour {

    public Vector3 StartScale = Vector3.one;
    public Vector3 EndScale = Vector3.one;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ScaleTo(bool isOn)
    {
        if (isOn)
            this.transform.localScale = EndScale;
        else
            this.transform.localScale = StartScale;
    }
}
