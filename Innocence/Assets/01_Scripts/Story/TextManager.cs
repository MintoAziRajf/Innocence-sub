using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.IO;
using UnityEngine.SceneManagement;


namespace NTextManager
{
    

    interface ITextManager
    {
        //�e�L�X�g�E�B���h�E�\��
        void TextWindow();
        //�e�L�X�g�E�B���h�E��\��
        void ReTextWindow();

        int CurrentLineNum { get; }

        bool CheckIfTheStoryIsOver { get; }
    }


    [RequireComponent(typeof(AudioSource))]

    public class TextManager : MonoBehaviour, ITextManager
    {

        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioClip audioClip;

        [SerializeField] Image chara_leftImage, chara_rightImage;
        //�l�̖��O
        [SerializeField] Text personName;
        //�e�L�X�g�E�B���h�E�̈ړ��p
        [SerializeField] RectTransform rectTransform;


        //�ŏ��ƍŌ�̃t�F�[�h����
        [SerializeField]protected Fade fade = null;

        //�e�L�X�g�\�����I����Ă��邩�̃`�F�b�N
        bool checkIfTheStoryIsOver = false;

        //�e�L�X�g�E�B���h�E�ɕ\�������e�L�X�g
        [SerializeField] Text mainText;

        //�e�L�X�g�t�@�C���ǂݍ��ݗp
        protected List<string> texts = new List<string>();
        //csv�t�@�C���ǂݍ��ݗp
        List<string[]> csvDatas = new List<string[]>();

        //�L�����N�^�[�̉摜���t�F�[�h�������邽��
        [SerializeField]
        FadeController chara_leftFade,chara_rightFade;

        //�t�@�C���̃p�X
        [SerializeField] string textPath, csvPath;


   
        protected int currentCharNum = 0;
        protected int currentLineNum = 0;
        int WhoDisplayed1 = 1;
        int WhoDisplayed2 = 0;
        float textInterval = 0;
        float time = 0f;
        float time2 = 0f;
        float posY = -150f;

        bool check;
        bool reCheck;
        bool whenInvokeBegins;
        bool blackoutCheck;

        Vector2 textWindowMove;
        public bool CheckIfTheStoryIsOver => checkIfTheStoryIsOver;

        public int CurrentLineNum => currentLineNum;

        //  public Image Chara_leftImage => chara_leftImage;
        // public Image Chara_rightImage => chara_rightImage;

       
        protected virtual void Start()
        {
            //chara_leftFade.isFadeIn = true;
            //chara_rightFade.isFadeIn = true;
            check = true;
            reCheck = true;
            whenInvokeBegins = false;
            blackoutCheck = true;
            Vector2 textWindowMove = new Vector2(0, posY);
            StartCoroutine(LoadText());

        }

        IEnumerator LoadText()
        {
            enabled = false;


            Addressables.LoadAssetAsync<TextAsset>(textPath).Completed += novelData =>
            {
                StringReader reader = new StringReader(novelData.Result.text);

                while (reader.Peek() != -1)
                {

                    string line = reader.ReadLine();
                    texts.Add(line);
                }
                enabled = true;
            };


            Addressables.LoadAssetAsync<TextAsset>(csvPath).Completed += charaData =>
            {
                StringReader reader = new StringReader(charaData.Result.text);

                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();
                    csvDatas.Add(line.Split(','));
                }
                enabled = true;
            };

            //�ǂݍ��ׂ݂̈̎���
            yield return new WaitForSeconds(1);
        }

       
        protected virtual void Update()
        {
            Invoke("TextWindow", 3f);

        
        }

        public void TextWindow()
        {
            if (Input.GetKey(KeyCode.Space) && checkIfTheStoryIsOver || whenInvokeBegins)
            {
                whenInvokeBegins = true;
                ReTextWindow();
            }

            time += Time.deltaTime;
            if (time >= 0.005f && check)
            {
                posY++;
                rectTransform.anchoredPosition = textWindowMove;
                time = 0;
            }

            if (posY >= 30)
            {
                check = false;
                TextController();
            }
        }

        public void ReTextWindow()
        {
            time2 += Time.deltaTime;

            if (time2 >= 0.005f && reCheck)
            {
                posY--;
                rectTransform.anchoredPosition = textWindowMove;
                time2 = 0;
            }

            if (posY <= -150)
            {
                reCheck = false;
            }

            if (!reCheck && blackoutCheck)
            {
                blackoutCheck = false;
                Invoke("Blackout", 1f);
            }
        }


        protected virtual void Blackout()//�Ó]
        {
            fade.FadeOut(0, () => fade.FadeIn(3));
         //   SceneManager.LoadScene(""); �ǂ̃V�[���ɑJ�ڂ��邩�͕ۗ�
        }

        protected virtual void TextController()
        {
            if (currentCharNum < texts[currentLineNum].Length) DisplayText();
            else NextLineWhenSpaceButton();

            PersonTalking();
            StartCoroutine(wait());
            ImageState();
        }

        //�e�L�X�g��\��
        protected void DisplayText()
        {

            Debug.Log(texts[currentLineNum][currentCharNum]);
            if (textInterval <= 0)
            {
                mainText.text += texts[currentLineNum][currentCharNum];
                currentCharNum++;

                if (Input.GetKey(KeyCode.Space))
                {

                    //�ꕶ���ǂݍ��ݑ���
                    textInterval = 5;

                }
                else
                {
                    textInterval = 50;
                }

            }
            else textInterval--;
        }


        protected void NextLineWhenSpaceButton()//space�L�[�Ŏ��̍s�Ɉړ�
        {
            if (Input.GetKey(KeyCode.Space) && !checkIfTheStoryIsOver)
            {
                audioSource.PlayOneShot(audioClip);

                currentLineNum++;

                if (currentLineNum >= lineIsTheEnd()) { checkIfTheStoryIsOver = true; Debug.Log(checkIfTheStoryIsOver); };
                currentCharNum = 0;
                mainText.text = "";

            }
        }

     

        protected void ImageLoading(bool check, int i, string imageName)
        {
            if (check)
            {
                Addressables.LoadAssetAsync<Sprite>(imageName).Completed += sprite =>
                {
                    if(i == 1)chara_leftImage.sprite = Instantiate(sprite.Result);
                    else chara_rightImage.sprite = Instantiate(sprite.Result);
                };
            }
        }

        protected void PersonTalking()//�b���Ă���l���N���̔���
        {
            //�i���[�V����
            if (csvDatas[currentLineNum][0] == "0")
            {
                personName.text = " ";
            }
            //��l��
            if (csvDatas[currentLineNum][0] == "1")
            {
                personName.text = "��l��";
                chara_leftImage.color = new Color(1f, 1f, 1f, chara_leftFade.Alfa);
                chara_rightImage.color = new Color(0.5f, 0.5f, 0.5f, chara_rightFade.Alfa);

            }

            //�Ŏ�
            if (csvDatas[currentLineNum][0] == "2")
            {
                personName.text = "�Ŏ�";

                chara_rightImage.color = new Color(1f, 1f, 1f, chara_rightFade.Alfa);
                chara_leftImage.color = new Color(0.5f, 0.5f, 0.5f, chara_leftFade.Alfa);
            }

            //�H�H�H
            if (csvDatas[currentLineNum][0] == "3")
            {
                personName.text = "�H�H�H";
            }
        }

        IEnumerator wait()//�摜�̓ǂݍ��ݑ҂��̈�
        {
            yield return new WaitForSeconds(0.1f);
            FadeIn();
            FadeOut();           
        }

        void FadeIn()
        {

            if (csvDatas[currentLineNum][1] == "1")
            {
                chara_leftFade.isFadeIn = true;
                WhoDisplayed1 = 1;
            }

            if (csvDatas[currentLineNum][1] == "2")
            {
                chara_leftFade.isFadeIn = true;
                WhoDisplayed2 = 1;
            }

            if (csvDatas[currentLineNum][2] == "1")
            {
                chara_rightFade.isFadeIn = true;
                WhoDisplayed1 = 0;
            }

            if (csvDatas[currentLineNum][2] == "2")
            {
                chara_rightFade.isFadeIn = true;
                WhoDisplayed2 = 0;
            }

        }

        void FadeOut()
        {
            if (csvDatas[currentLineNum][1] == "0")
            {
                chara_leftFade.isFadeOut = true;
            }

            if (csvDatas[currentLineNum][2] == "0")
            {
                chara_rightFade.isFadeOut = true;
            }

        }

        void ImageState()
        {
            //��l���̉摜  
            ImageLoading(csvDatas[currentLineNum][3] == "0",WhoDisplayed1, "Assets/Image/Smile.png");                     
            ImageLoading(csvDatas[currentLineNum][3] == "1",WhoDisplayed1, "Assets/Image/SilentExpression.png");
            ImageLoading(csvDatas[currentLineNum][3] == "2",WhoDisplayed1, "Assets/Image/ScaredExpression.png");

            //�Ŏ�̉摜
            ImageLoading(csvDatas[currentLineNum][4] == "0",WhoDisplayed2, "Assets/Image/jailer_Stirup.png");
            ImageLoading(csvDatas[currentLineNum][4] == "1",WhoDisplayed2, "Assets/Image/jailer_Normal.png");
            ImageLoading(csvDatas[currentLineNum][4] == "2",WhoDisplayed2, "Assets/Image/jailer_Impatience.png");

        }

        protected virtual int lineIsTheEnd()
        {
            int lineIsTheEnd = 15;
            return lineIsTheEnd;
        }
    }
}