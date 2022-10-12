using NTextManager;

public class Prologue : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        //   SceneManager.LoadScene(""); どのシーンに遷移するかは不明
    }
}
