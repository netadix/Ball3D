using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnAnimationEnd(string name)
    {
        GameObject scoreText = this.gameObject;
        Destroy(scoreText);
    }
}

