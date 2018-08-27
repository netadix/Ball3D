using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomFloor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnCollisionEnter(Collision col)
    {
        string objectName = col.gameObject.name;
        if (col.gameObject.name.Length >= 7)
        {
            if (objectName.Substring(0, 7) == "missile")      // missile
            {
                Destroy(col.gameObject);
                VariableCollection.ContinuousShoot = 0;     // 連チャンクリア
            }
        }
    }
}
