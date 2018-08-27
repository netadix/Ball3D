using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {

    public GameObject RollerBall;
    public GameObject TitleText;
    public GameObject Player;
    public GameObject Obstacles;
    public Button SwitchButton;
    public GameObject Camera;
    public GameObject Floor;
	public GameObject TransparentWall;
    public GameObject CameraAnimation;

    private bool View2D;

    // Use this for initialization
    void Start () {
        View2D = true;
        StartCoroutine("TextAnimationStart");

        SetTotalBallNumber();
        //int i;
        //float z;
        //VariableCollection.originBallSize = RollerBall.transform.localScale.x;
        //z = VariableCollection.originBallSize;
        //for (i = 0; ; i++)
        //{
        //    if (z < VariableCollection.finalBallSize) break;
        //    z = z * 0.8f;
        //}
        //VariableCollection.totalBallNumber = (int)(VariableCollection.startBallNumber * Math.Pow(2, i));
        Button btn = SwitchButton.GetComponent<Button>();
        btn.onClick.AddListener(SwitchView);
    }

    void SetTotalBallNumber()
    {
        int i;
        float z;
        VariableCollection.originBallSize = RollerBall.transform.localScale.x;
        z = VariableCollection.originBallSize;
        for (i = 0; ; i++)
        {
            if (z < VariableCollection.finalBallSize) break;
            z = z * VariableCollection.ballSizeFactor;
        }
        VariableCollection.totalBallNumber = (int)(VariableCollection.startBallNumber * Math.Pow(2, i));

    }

    IEnumerator TextAnimationStart()
    {
        //float currentTimeScale = Time.timeScale;
        //Time.timeScale = 0;     // すべての処理を止める
        Text target = TitleText.GetComponent<Text>();
        target.text = "STAGE" + VariableCollection.Stage + " START";
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(false);
        //Time.timeScale = currentTimeScale;

    }
    IEnumerator TextAnimationGameOver()
    {
        Text target = TitleText.GetComponent<Text>();
        target.text = "GAME OVER";
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        TitleText.SetActive(false);
        VariableCollection.TheEnd = false;
        Application.Quit();
        //  EditorApplication.Exit(0);      これ危険！！！！
    }

    IEnumerator TextAnimationStageClear()
    {
        //Debug.Log("Stage clear");
        Text target = TitleText.GetComponent<Text>();
        target.text = "STAGE" + VariableCollection.Stage + " CLEAR";
        //String str = "STAGE" + VariableCollection.Stage + " CLEAR";
        //TextAnimation(str, 4, 0.5f);
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        TitleText.SetActive(false);
        VariableCollection.StageClear = false;
        VariableCollection.Stage++;
        //StartCoroutine("TextAnimationStart");
        target.text = "STAGE" + VariableCollection.Stage + " START";
        //String str = "STAGE" + VariableCollection.Stage + " CLEAR";
        //TextAnimation(str, 4, 0.5f);
        yield return new WaitForSeconds(0.3f);
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        TitleText.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        TitleText.SetActive(false);

        VariableCollection.startBallNumber++;
        SetTotalBallNumber();
        int maxColumn = 4;

        // 面クリア時のボールの生成
        int row = VariableCollection.startBallNumber / maxColumn;
        for (int j = 0; j < row + 1; j++)
        {
            for (int i = 0; i < maxColumn; i++)
            {
                if (VariableCollection.startBallNumber <= j * maxColumn + i) break; 
                GameObject ball1 = GameObject.Instantiate(RollerBall) as GameObject;

                ball1.transform.localScale = new Vector3(3f, 3f, 3f);

                Vector3 currentBall1Position = new Vector3(-17f + (float)i * 3.5f, 6f + (float)j * 3f, 8f);
                currentBall1Position.x += VariableCollection.ballSizeFactor * ball1.transform.localScale.x;
                ball1.transform.position = currentBall1Position;
                Rigidbody rigid = ball1.GetComponent<Rigidbody>();
                rigid.AddForce(new Vector3(0f, 2.5f, 0f), ForceMode.Impulse);

            }
        }

        GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obs in objs)
        {
            if (obs.name.Substring(0, 3) == "Obs")
            {
                Destroy(obs);
            }
        }

        for (int i = 0; i < row; i++)
        {
            GameObject wall = GameObject.Instantiate(Obstacles) as GameObject;

            wall.transform.localScale = new Vector3(2f, 2f, 2f);

            Vector3 WallPosition = new Vector3(-17f + (float)i * 3.5f, -2f, 8f);
            wall.transform.position = WallPosition;
        }

        yield break;

    }

    //IEnumerator TextAnimation(string text, int count, float interval = 0.2f)
    //{
    //    Text target = TitleText.GetComponent<Text>();
    //    target.text = "STAGE" + VariableCollection.Stage + " CLEAR";
    //    for (int i = 0; i < count; i++)
    //    {
    //        TitleText.SetActive(true);
    //        yield return new WaitForSeconds(interval);
    //        TitleText.SetActive(false);
    //    }
    //}

    // Update is called once per frame
    void Update () {

        //　何故かBall.csのスクリプトがDisabledになってしまうので無理やりEnabledにしている。
        // Ballの生成と消滅のタイミングが何か関係があるのかもしれない
        GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obs in objs)
        {
            if (obs.name.Substring(0, 3) == "Rol")
            {
                if (obs.GetComponent<Ball>().enabled == false)
                {
                    obs.GetComponent<Ball>().enabled = true;
                }
            }
        }


        if (VariableCollection.TheEnd == true)
        {
            StartCoroutine("TextAnimationGameOver");
            VariableCollection.TheEnd = false;
        }
        if (VariableCollection.StageClear == true)
        {
            StartCoroutine("TextAnimationStageClear");
            VariableCollection.StageClear = false;
        }
#if true
        if (VariableCollection.AudioPlayFlag == true)
        {
            VariableCollection.audioSource = gameObject.GetComponent<AudioSource>();
            VariableCollection.audioSource.PlayOneShot(VariableCollection.audioClip);
            VariableCollection.AudioPlayFlag = false;
        }
#endif

    }

    private void SwitchView()
    {
        if (View2D == true)
        {
            View2D = false;
            //Camera.transform.position = new Vector3(-10f, -22f, 2f);
            //Camera.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
            Animator anime = CameraAnimation.GetComponent<Animator>();
            anime.Play("Rotation90Anime");
            //Floor.GetComponent<MeshFilter>().;
            //SwitchButton.enabled = false;
			TransparentWall.transform.position = new Vector3(-10f, -5f, -7.5f);
        }
        else
        {
            View2D = true;
            Camera.transform.position = new Vector3(-10f, 3.7f, -19.8f);
            Camera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
			TransparentWall.transform.position = new Vector3(-10f, -5f, 6.5f);

        }
    }
}
