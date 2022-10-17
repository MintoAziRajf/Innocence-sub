using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //�p�l���̃C���[�W�𑀍삷��̂ɕK�v
 
public class FadeController : MonoBehaviour
{

	float fadeSpeed = 0.02f;        //�����x���ς��X�s�[�h���Ǘ�
	float red, green, blue, alfa;   //�p�l���̐F�A�s�����x���Ǘ�

	public bool isFadeOut = false;  //�t�F�[�h�A�E�g�����̊J�n�A�������Ǘ�����t���O
	public bool isFadeIn = false;   //�t�F�[�h�C�������̊J�n�A�������Ǘ�����t���O
    [SerializeField] Image fadeImage;                //�����x��ύX����p�l���̃C���[�W

	public float Alfa => alfa;
	void Start()
	{
		red = fadeImage.color.r;
		green = fadeImage.color.g;
		blue = fadeImage.color.b;
		alfa = fadeImage.color.a;

		fadeSpeed = 0.02f;
	}

	void Update()
	{
		red = fadeImage.color.r;
		green = fadeImage.color.g;
		blue = fadeImage.color.b;
		alfa = fadeImage.color.a;

		if (isFadeOut)
		{
			StartFadeOut();
		}

		if (isFadeIn)
		{
			StartFadeIn();
		}
	}

	void StartFadeOut()
	{
		alfa -= fadeSpeed;                //�s�����x�����X�ɉ�����
		SetAlpha();                      //�ύX�����s�����x�p�l���ɔ��f����
		if (alfa <= 0)
		{                    //c)���S�ɓ����ɂȂ����珈���𔲂���
			isFadeOut = false;
			fadeImage.enabled = false;    //d)�p�l���̕\�����I�t�ɂ���
		}
	}

	void StartFadeIn()
	{
		fadeImage.enabled = true;  // �p�l���̕\�����I���ɂ���
		alfa += fadeSpeed;         // �s�����x�����X�ɂ�����
		SetAlpha();               // �ύX���������x���p�l���ɔ��f����
		if (alfa >= 1)
		{             // ���S�ɕs�����ɂȂ����珈���𔲂���
			isFadeIn = false;
		}
	}

	void SetAlpha()
	{
		fadeImage.color = new Color(red, green, blue, alfa);
	}
}