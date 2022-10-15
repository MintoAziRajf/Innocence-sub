using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace StageSelectScene
{


    public enum Score { SCORE0, SCORE1, SCORE2, SCORE3, };

    interface IClearEvaluation
    {
        
      //  void ClearEvaluations(string StageName, Score ClearAchievements);
      
    }

    [RequireComponent(typeof(AudioSource))]
    public class StageSelectionScreen : MonoBehaviour, IClearEvaluation
    {

        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip audioClip;

        [SerializeField] GameObject obj, ClearEvaluation00, ClearEvaluation01, ClearEvaluation02;

        [SerializeField] GameObject[] stagesRed = new GameObject[13];

        [SerializeField] Text[] stages = new Text[13];
        [SerializeField] Text[] titles = new Text[13];
        [SerializeField] Text stageNumber;
        [SerializeField] Text title;
        
        [SerializeField] Text levelTextRank;
        [SerializeField] Image levelImages;
        [SerializeField] Text stamina;

        bool checkUp = false;
        bool checkDown = false;
        bool checkMovingRangeUp;
        bool checkMovingRangeDown;
        static string names;
        static Score clearAchievements;
        Color green, yellow, red,purple;
        Vector3 posDown,posUp;


        // Start is called before the first frame update
        void Start()
        {
            posUp = new Vector3(-27.3f, 100.6f, 90f);
            posDown = new Vector3(-27.3f, -88.2f, 90f);

            green = new Color(0f, 1f, 0f, 0.5f);
            yellow = new Color(1f, 00.92f, 0.016f, 0.5f);
            red = new Color(1f, 0f, 0f, 0.5f);
            purple = new Color(0.5f, 0f, 0.5f, 0.5f);
         
        }

        // Update is called once per frame
        void Update()
        {
            bool keyUp = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
            bool keyDown = (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));

            
            /*ClearEvaluations("Stage0", Score.SCORE0);
            ClearEvaluations("Stage1", Score.SCORE1);
            ClearEvaluations("Stage2", Score.SCORE2);
            ClearEvaluations("Stage3", Score.SCORE3);
            ClearEvaluations("Stage4", Score.SCORE1);
            ClearEvaluations("Stage5", Score.SCORE3);
            ClearEvaluations("Stage6", Score.SCORE2);
            ClearEvaluations("Stage7", Score.SCORE1);
            ClearEvaluations("Stage8", Score.SCORE0);
            ClearEvaluations("Stage9", Score.SCORE1);
            ClearEvaluations("Stage10", Score.SCORE2);
            ClearEvaluations("Stage11", Score.SCORE3);
            ClearEvaluations("Stage12", Score.SCORE2);*/

            //Debug.Log(checkMovingRangeUp);
                Vector3 target = obj.transform.position;

            //移動
                obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, 1.0f * Time.deltaTime);


          

            

            if (obj.transform.position == target)
            {
                if (keyUp && checkMovingRangeUp || checkUp)
                {
                    checkUp = true;

                    //target.y += -150;
                    obj.transform.Translate(0f, -60f * Time.deltaTime, 0f);
                    Invoke("wait", 0.28f);
                }

                if (keyDown && checkMovingRangeDown || checkDown)
                {
                    checkDown = true;
                   //target.y += 150;
                    obj.transform.Translate(0f, 60f * Time.deltaTime, 0f);
                    Invoke("wait", 0.28f);
                }


                //Up
                //target.y += 150f;
                //Down
                //target.y -= 150f;


            }
            
                
           
            Evaluation();

        }

        void waitScUp() => checkMovingRangeUp = true;
        void waitScDown() => checkMovingRangeDown = true;
        

        void OnTriggerEnter2D(Collider2D other)
        {
            audioSource.PlayOneShot(audioClip);

            if (other.name == "Stage0")
            {
                checkMovingRangeUp = false;
           
                            
            }
            else if (other.name == "Stage12")
            {
                checkMovingRangeDown = false;
              
            }
            else
            {
                checkMovingRangeUp = true;
                checkMovingRangeDown = true;
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {

            names = other.name;


            for (int i = 0; i < stages.Length; i++)
            {
                if (other.name == "Stage" + i)
                {
                    title.text = titles[i].text;



                    if (i < 3) { stamina.text = "- 25 -"; levelTextRank.text = "- EAGY -"; levelImages.color = green; }
                    if (i > 3 && i < 6) { stamina.text = "- 20 -"; levelTextRank.text = "- NOMAL -"; levelImages.color = yellow; }
                    if (i > 6 && i < 9) { stamina.text = "- 15 -"; levelTextRank.text = "- HARD -"; levelImages.color = red; }
                    if (i > 9 && i < 12) { stamina.text = "- 10 -"; levelTextRank.text = "- NIGHTMEA -"; levelImages.color = purple; }

                    if (i < 10) stageNumber.text = "STAGE - 0" + i;
                    else stageNumber.text = "STAGE - " + i;

                    //stages[i].color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

                    stagesRed[i].SetActive(true);


                }
                else
                {
                    stagesRed[i].SetActive(false);
                }
            }

        }
        void wait()
        {
            checkUp = false;
            checkDown = false;
        }

        
        //引数の内容（ステージ名：例 "stage01"など クリア実績数：SCORE1,2,3）
        public static void ClearEvaluations(string StageName, Score ClearAchievements)
        {
          if (StageName == names) clearAchievements = ClearAchievements;            
        }

        
        private void Evaluation()
        {
            switch (clearAchievements)
            {
                case Score.SCORE1:

                    ClearEvaluation00.SetActive(true);
                    ClearEvaluation01.SetActive(false);
                    ClearEvaluation02.SetActive(false);
                    break;

                case Score.SCORE2:

                    ClearEvaluation00.SetActive(true);
                    ClearEvaluation01.SetActive(true);
                    ClearEvaluation02.SetActive(false);
                    break;

                case Score.SCORE3:

                    ClearEvaluation00.SetActive(true);
                    ClearEvaluation01.SetActive(true);
                    ClearEvaluation02.SetActive(true);
                    break;

                default:

                    ClearEvaluation00.SetActive(false);
                    ClearEvaluation01.SetActive(false);
                    ClearEvaluation02.SetActive(false);
                    break;
            }
        }
    }
}
