using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StoneController : MonoBehaviour
{
    //移動距離
    private Vector3[] moveDirection =
    {
         new Vector3(0f, 1f, 0f),
         new Vector3(1f, 0f, 0f),
         new Vector3(0f, -1f, 0f),
         new Vector3(-1f, 0f, 0f)
    };

    [SerializeField] private float step = 2f;　//移動速度
    Vector3 target; //移動先の座標

    //各方向の衝突情報
    private bool[] isDirection = new bool[4] { false, false, false, false };
    private enum DIRECTION
    {
        TOP,
        RIGHT,
        UNDER,
        LEFT
    }

    //stoneのアクセスフラグ
    private bool isAccess = false;
    public bool IsAccess
    {
        set { isAccess = value; }
    }
    private SpriteRenderer circuit;

    //蹴られた時のアニメーション
    private Animator stoneAnim;
    private GameObject kickEffect = null;
    private Animator kickAnim;
    private GameObject dustEffect = null;
    private Animator dustAnim;

    //playerController
    private GameObject player;
    PlayerController playerController;

    //Minigame
    //ミニゲームの種類 0:シューティング 1:イライラ棒
    private enum GAMETYPE
    {
        SHOOTING,
        TIMING,
        SKILLCHECK
    }
    private GAMETYPE gameType;
    //ゲームの難易度 10通り
    [Range(0, 9)] public int gameDifficulty = 0;

    //GameManager
    MainGameManager mainGameManager; 

    private void Start()
    {
        target = transform.position;
        circuit = this.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        mainGameManager = GameObject.Find("GameManager").GetComponent<MainGameManager>();

        //Effect
        stoneAnim = GetComponent<Animator>();
        dustEffect = GameObject.Find("DustEffect02");
        dustAnim = dustEffect.GetComponent<Animator>();
        kickEffect = GameObject.Find("KickEffect");
        kickAnim = kickEffect.GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        AccessDisplay();

        void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, step * Time.deltaTime);
        }
        //アクセス権限の可視化
        void AccessDisplay()
        {
            if (isAccess)
            {
                circuit.color = Color.green;
                return;
            }
            circuit.color = Color.red;
        }
    }
    public void SetStoneInfo(int type, int difficulty)
    {
        gameType = (GAMETYPE)Enum.ToObject(typeof(GAMETYPE), type);
        gameDifficulty = difficulty;
    }
    public void PushStonePosition(int direction)
    {
        //アクセス権限がない場合指定されているミニゲームを開始する
        if (!isAccess)
        {
            StartMiniGame();
            return;
        }

        StartCoroutine(KickAnimation());
        if (isDirection[direction])
        {
            //Stoneが動かなかったときのアニメーション
            stoneAnim.SetTrigger("KickCantMove");
            return;
        }

        target = transform.position + moveDirection[direction];
    }

    private IEnumerator KickAnimation()
    {
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Kick);
        //Effect
        dustEffect.transform.position = this.transform.position;
        kickEffect.transform.position = this.transform.position;
        kickAnim.SetTrigger("Kick");

        yield return new WaitForSeconds(0.2f);
        dustAnim.SetTrigger("Move");
    }

    public void CollisionInfomation(int direction, bool isHit)
    {
        isDirection[direction] = isHit;
    }

    private void StartMiniGame()
    {
        playerController.IsStart = false;
        mainGameManager.SetStone(GetComponent<StoneController>());
        switch (gameType)
        {
            case GAMETYPE.SHOOTING:
                SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Shooting);
                mainGameManager.Difficulty = gameDifficulty;
                mainGameManager.StartCoroutine("SceneAdd", "03_Shooting");
                break;
            case GAMETYPE.TIMING:
                SoundManager.instance.PlayBGM(SoundManager.BGM_Type.Timing);
                mainGameManager.Difficulty = gameDifficulty;
                mainGameManager.StartCoroutine("SceneAdd", "02_Timing");
                break;
            case GAMETYPE.SKILLCHECK:
                SoundManager.instance.PlayBGM(SoundManager.BGM_Type.SkillCheck);
                mainGameManager.Difficulty = gameDifficulty;
                mainGameManager.StartCoroutine("SceneAdd", "04_SkillCheck");
                break;
            default:
                Debug.Log("不正なゲームIDです。");
                break;
        }
    }
}
