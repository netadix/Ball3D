using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class VariableCollection {

    static public bool TheEnd = false;
    static public bool StageClear = false;
    static public int Stage = 1;

    static public float originBallSize;
    static public float ballSizeFactor = 0.8f;
    static public float finalBallSize = 2.2f;
    static public int totalBallNumber;
    static public int startBallNumber = 1;

    static public AudioClip audioClip;
    static public AudioSource audioSource;
    static public bool AudioPlayFlag = false;
    static public int ContinuousShoot = 0;

}
