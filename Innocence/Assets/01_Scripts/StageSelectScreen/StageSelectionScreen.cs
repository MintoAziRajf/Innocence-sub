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

        [SerializeField] AudioSource audioSource = null;
        [SerializeField] AudioClip audioClip = null;

        [SerializeField] GameObject ClearEvaluation00 = null, ClearEvaluation01 = null, ClearEvaluation02 = null;

        [SerializeField] GameObject[] stagesRed = new GameObject[13];

        [SerializeField] Text[] stages = new Text[13];
        [SerializeField] Text[] titles = new Text[13];
        [SerializeField] Text stageNumber = null;
        [SerializeField] Text title = null;
        
        [SerializeField] Text levelTextRank = null;
        [SerializeField] Image levelImages = null;
        

        [SerializeField] RoadingCSV csv = null;
        string names;
        Score clearAchievements;
        Color green, yellow, red,purple;

        int nameNumber = 0;
        bool keyDown = false;
        bool isFirst = false;
        public string Names => names;


        // Start is called before the first frame update
        void Start()
        {
            green = new Color(0f, 1f, 0f, 0.5f);
            yellow = new Color(1f, 00.92f, 0.016f, 0.5f);
            red = new Color(1f, 0f, 0f, 0.5f);
            purple = new Color(0.5f, 0f, 0.5f, 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            if (isFirst) return;
            keyDown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
            for (int i = 0; i < 13;++i)
            {
                ClearEvaluations("Stage" + i, csv.ClearEvaluations[i]);
            }

            if (keyDown && names != "Stage0")
            {
                isFirst = true;
                CSVManager.instance.Stages = nameNumber - 1;
                CSVManager.instance.LoadGame();
            }
            else if(keyDown && names == "Stage0")
            {
                isFirst = true;
                CSVManager.instance.Stages = -1;
                CSVManager.instance.LoadGame();
            }



            Evaluation();           
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            audioSource.PlayOneShot(audioClip);
        }
        void OnTriggerStay2D(Collider2D other)
        {

            names = other.name;

            for (int i = 0; i < stages.Length; i++)
            {
                if (other.name == "Stage" + i)
                {
                    title.text = titles[i].text;

                    nameNumber = i;

                    if(i == 0) { levelTextRank.text = ""; }
                    if (i >= 1 && i < 3) { levelTextRank.text = "- EAGY -"; levelImages.color = green; }
                    if (i > 3 && i < 6) { levelTextRank.text = "- NOMAL -"; levelImages.color = yellow; }
                    if (i > 6 && i < 9) { levelTextRank.text = "- HARD -"; levelImages.color = red; }
                    if (i > 9 && i < 12) { levelTextRank.text = "- NIGHTMEA -"; levelImages.color = purple; }

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
        
        //引数の内容（ステージ名：例 "stage01"など クリア実績数：SCORE1,2,3）
        void ClearEvaluations(string StageName, Score ClearAchievements)
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
