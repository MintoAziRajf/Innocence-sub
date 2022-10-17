using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //パネルのイメージを操作するのに必要
 
public class FadeController : MonoBehaviour
{

	float fadeSpeed = 0.02f;        //透明度が変わるスピードを管理
	float red, green, blue, alfa;   //パネルの色、不透明度を管理

	public bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
	public bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ
    [SerializeField] Image fadeImage;                //透明度を変更するパネルのイメージ

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
		alfa -= fadeSpeed;                //不透明度を徐々に下げる
		SetAlpha();                      //変更した不透明度パネルに反映する
		if (alfa <= 0)
		{                    //c)完全に透明になったら処理を抜ける
			isFadeOut = false;
			fadeImage.enabled = false;    //d)パネルの表示をオフにする
		}
	}

	void StartFadeIn()
	{
		fadeImage.enabled = true;  // パネルの表示をオンにする
		alfa += fadeSpeed;         // 不透明度を徐々にあげる
		SetAlpha();               // 変更した透明度をパネルに反映する
		if (alfa >= 1)
		{             // 完全に不透明になったら処理を抜ける
			isFadeIn = false;
		}
	}

	void SetAlpha()
	{
		fadeImage.color = new Color(red, green, blue, alfa);
	}
}