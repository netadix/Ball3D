using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    public GameObject RollerBall;
    public GameObject ScoreText;
    public GameObject MainCamera;
    private GameObject CanvasUI;

    private new Camera camera;

    // Use this for initialization
    void Start () {
        /// ---------------------
        // Register the singleton
        /// ---------------------
        if (Instance != null)
        {
            //Debug.LogError("Multiple instances of InstanceExample script!");
        }

        Instance = this;

        Rigidbody rigid = GetComponent<Rigidbody>();
        rigid.AddForce(new Vector3(0f, 5f, 0f), ForceMode.Impulse);

        camera = Camera.main.GetComponent<Camera>();
        CanvasUI = GameObject.Find("Canvas");
    }

// Update is called once per frame
void Update() {
        Vector3 currentBallPosition = this.gameObject.transform.position;
        currentBallPosition.z = 8.0f;
        this.gameObject.transform.position = currentBallPosition;

     }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject != null)
        {
            if (col.gameObject.name.Length >= 7)
            {
                if (col.gameObject.name.Substring(0, 7) == "missile")     // missile
                {
                    // 連チャン処理
                    VariableCollection.ContinuousShoot++;   // 連チャンカウンタ

                    if (VariableCollection.ContinuousShoot > 2) {
                        GameObject scoreText = GameObject.Instantiate(ScoreText) as GameObject;
                        scoreText.transform.SetParent(CanvasUI.transform);
                        Vector3 screenPoint = camera.WorldToScreenPoint(col.gameObject.transform.position);
                        scoreText.transform.position = new Vector3(screenPoint.x, screenPoint.y + gameObject.transform.localScale.y * 10, 0);
                        scoreText.GetComponent<Text>().text = VariableCollection.ContinuousShoot + " Combo!";
                        scoreText.GetComponent<Outline>().effectColor = new Color32((byte)(VariableCollection.ContinuousShoot * 10),
                            (byte)(VariableCollection.ContinuousShoot * 10),
                            255, 255);  // アニメーションに色が支配されているっぽいので効かない
                    }

                    Destroy(col.gameObject);    // missile

                    Vector3 ballPosition = this.gameObject.transform.position;
                    ExplosionA(ballPosition);    // 爆発

                    if (this.gameObject.transform.localScale.x <= VariableCollection.finalBallSize)
                    {
                        //Debug.Log("Destroy Total ball:" + VariableCollection.totalBallNumber + " Start ball:" + VariableCollection.startBallNumber);
                        ExplosionB(this.gameObject.transform.position);    // 爆発

                        GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
                        int ExistBallNumber = 0;
                        foreach(GameObject obs in objs)
                        {
                            if (obs.name.Substring(0, 3) == "Rol")
                            {
                                ExistBallNumber++;
                            }
                        }
                        if (ExistBallNumber == 1)   // Only my ball because GameObject itself is not destroyed untill the code is finished.
                        {
                            VariableCollection.StageClear = true;

                        }
                        VariableCollection.totalBallNumber--;
                        //if (VariableCollection.totalBallNumber <= 0)
                        //{
                        //    VariableCollection.StageClear = true;
                        //}
                    }
                    else   // 分裂
                    {
                        //Debug.Log("Part Total ball:" + VariableCollection.totalBallNumber + " Start ball:" + VariableCollection.startBallNumber);
                        GameObject ball1 = GameObject.Instantiate(RollerBall) as GameObject;
                        GameObject ball2 = GameObject.Instantiate(RollerBall) as GameObject;

                        ball1.transform.localScale *= VariableCollection.ballSizeFactor;
                        ball2.transform.localScale *= VariableCollection.ballSizeFactor;

                        Vector3 currentBall1Position = ballPosition;
                        currentBall1Position.x += VariableCollection.ballSizeFactor * ball1.transform.localScale.x;
                        ball1.transform.position = currentBall1Position;
                        Vector3 currentBall2Position = ballPosition;
                        currentBall2Position.x -= VariableCollection.ballSizeFactor * ball1.transform.localScale.x;
                        ball2.transform.position = currentBall2Position;

                        Rigidbody rigid = ball1.GetComponent<Rigidbody>();
                        rigid.AddForce(new Vector3(1.2f, 2.5f, 0f), ForceMode.Impulse);
                        rigid = ball2.GetComponent<Rigidbody>();
                        rigid.AddForce(new Vector3(-1.2f, 2.5f, 0f), ForceMode.Impulse);
                        Behaviour halo = (Behaviour)ball1.GetComponent("Halo"); // 何故かHaloがオフになってる
                        halo.enabled = true;
                        halo = (Behaviour)ball2.GetComponent("Halo");
                        halo.enabled = true;
                    }

                    VariableCollection.audioClip = Resources.Load<AudioClip>("Mortar1915982213");
                    VariableCollection.AudioPlayFlag = true;
                    Destroy(this.gameObject);


                }
            }
            else
            {
                VariableCollection.audioClip = Resources.Load<AudioClip>("boing_spring");
                VariableCollection.AudioPlayFlag = true;
            }
        }
    }
    /// ------------------------------
    /// Creating instance of particles
    /// ------------------------------
    /// ------------------------------
    /// Singleton
    /// ------------------------------
    public static Ball Instance;

    public ParticleSystem effectA;
    public ParticleSystem effectB;

    /// -----------------------------------------
    /// Create an explosion at the given location
    /// -----------------------------------------
    public void ExplosionA(Vector3 position)
    {
        instantiate(effectA, position);
    }
    public void ExplosionB(Vector3 position)
    {
        instantiate(effectB, position);
    }

    /// -----------------------------------------
    /// Instantiate a Particle system from prefab
    /// -----------------------------------------
    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newParticleSystem = Instantiate(prefab, position, Quaternion.identity) as ParticleSystem;

        /// -----------------------------
        // Make sure it will be destroyed
        /// -----------------------------
        Destroy(
            newParticleSystem.gameObject,
            newParticleSystem.startLifetime
        );

        return newParticleSystem;
    }
}
