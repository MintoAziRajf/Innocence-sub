using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float speed = 5f;　//移動速度
    private float coolTime = 0f; //移動のクールタイム
    Vector3 target; //移動先の座標

    //各方向の衝突情報
    private bool[] isDirection = new bool[4] { false, false, false, false };

    //方向
    private enum DIRECTION
    {
        TOP = 0,
        RIGHT = 1,
        UNDER = 2,
        LEFT = 3
    }
    //移動距離
    private Vector3[] moveDirection =
    {
         new Vector3(0f, 1f, 0f),
         new Vector3(1f, 0f, 0f),
         new Vector3(0f, -1f, 0f),
         new Vector3(-1f, 0f, 0f)
    };

    private GameObject[] directionObj = new GameObject[4];
    StoneController stoneController = null;

    //animator
    Animator animator;
    SpriteRenderer sp;

    //移動砂埃Animator
    private GameObject dustEffect;
    Animator dustAnim;
    SpriteRenderer dustSp;

    //Gameスタートしてるかどうか
    private bool isStart = false;
    public bool IsStart
    {
        set { isStart = value; }
    }

    //移動制限
    private int maxSteps;
    private int steps = 0;
    public int Steps
    {
        set { steps = value; }
    }
    //歩数表示用:UI
    [SerializeField] private Text stepText = null;
    private GameObject[] stepDisplay = new GameObject[3];

    //GameManager
    MainGameManager mgm;
    MapCreater mapCreater;

    //Battleフラグ
    [SerializeField] private bool isBattle = false;

    void Start()
    {
        mgm = GameObject.Find("GameManager").GetComponent<MainGameManager>();
        //歩数表示
        //歩数を受け取るまで待機
        maxSteps = steps;
        stepText.text = steps.ToString("00");
        stepDisplay[0] = GameObject.Find("StepsDisplay_0");
        stepDisplay[1] = GameObject.Find("StepsDisplay_1");
        stepDisplay[2] = GameObject.Find("StepsDisplay_2");
        //playerSprite
        sp = this.GetComponent<SpriteRenderer>();

        //目的地初期化
        target = transform.position;

        //Animator関連
        dustEffect = GameObject.Find("DustEffect");
        dustSp = dustEffect.GetComponent<SpriteRenderer>();
        dustAnim = dustEffect.GetComponent<Animator>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //移動
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!isStart) return;
        //行動クールタイムを計算
        coolTime -= Time.deltaTime;
        if (coolTime > 0f) return;
        //移動中かどうかの判定。移動中でなければ入力を受付
        if (this.transform.position == target)
        {
            InputDirection();
        }
    }

    /// <summary>
    /// 入力に応じて移動後の位置を算出
    /// </summary>
    private void InputDirection()
    {
        if (Input.GetButton("Up"))
        {
            DirectionMethod((int)DIRECTION.TOP);
        }

        else if (Input.GetButton("Right"))
        {
            sp.flipX = false;
            DirectionMethod((int)DIRECTION.RIGHT);
        }

        else if (Input.GetButton("Down"))
        {
            DirectionMethod((int)DIRECTION.UNDER);
        }

        else if (Input.GetButton("Left"))
        {
            sp.flipX = true;
            DirectionMethod((int)DIRECTION.LEFT);
        }
    }

    /// <summary>
    /// 行動処理
    /// </summary>
    /// <param name="direction"> 方向 </param>
    private void DirectionMethod(int direction)
    {
        //移動回数制限を使い切ったらゲームオーバー
        if (steps == 0)
        {
            isStart = false; //プレイヤーの停止
            mgm.GameOver();
            return;
        }

        //指定方向にオブジェクトが無かった場合移動
        if (!isDirection[direction])
        {
            coolTime = 0.2f;
            target = transform.position + moveDirection[direction];
            StartCoroutine(MoveAnimation());
            return;
        }
        //操作できるオブジェクトが無ければ何もしない
        if (directionObj[direction] == null) return;
        //指定方向のオブジェクトがスイッチの場合　スイッチのメソッドを呼び出す
        if (directionObj[direction].tag == "Switch")
        {
            directionObj[direction].GetComponent<TrapController>().TrapOff();
            return;
        }
        //stoneを押す
        StartCoroutine(KickStone(direction));
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Moved()
    {
        steps--;
        //残り歩数の可視化
        if(steps == 0)
        {
            stepDisplay[0].SetActive(false);
        }
        else if(steps <= maxSteps / 3)
        {
            stepDisplay[1].SetActive(false);
        }
        else if(steps <= (maxSteps * 2) / 3)
        {
            stepDisplay[2].SetActive(false);
        }
        stepText.text = steps.ToString("00");
    }

    /// <summary>
    /// 移動エフェクト
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveAnimation()
    {
        Moved();
        //SE
        SoundManager.instance.PlaySE(SoundManager.SE_Type.Move);
        //Effect
        dustEffect.transform.position = this.transform.position;
        dustSp.flipX = sp.flipX;
        
        //Animation
        animator.SetBool("Move", true);
        yield return new WaitForSeconds(0.1f);
        dustAnim.SetTrigger("Move");
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("Move", false);
    }

    /// <summary>
    /// キックエフェクト
    /// </summary>
    /// <param name="direction"> 方向 </param>
    /// <returns></returns>
    private IEnumerator KickStone(int direction)
    {
        stoneController = directionObj[direction].GetComponent<StoneController>();
        stoneController.PushStonePosition(direction);
        Moved();
        coolTime = 0.5f;
        animator.SetBool("Kick", true);
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("Kick", false);
    }

    /// <summary>
    /// 子オブジェクトから衝突情報を受け取る
    /// </summary>
    /// <param name="direction"> 方向 </param>
    /// <param name="isHit"> 当たっているかどうか </param>
    /// <param name="obj"> 今衝突しているオブジェクト </param>
    public void CollisionInfomation(int direction, bool isHit, GameObject obj)
    {
        isDirection[direction] = isHit;
        directionObj[direction] = obj;
    }

    //衝突処理
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Goal"))
        {
            if (!isBattle)
            {
                //anim
                collider.GetComponent<Animator>().SetTrigger("Goal");
            }
            //playerの動き停止
            isStart = false;
            //次のステージへ進む
            mgm.StartCoroutine("NextStage");
        }
        if (collider.CompareTag("Trap"))
        {
            mgm.GameOver();
        }
    }
}