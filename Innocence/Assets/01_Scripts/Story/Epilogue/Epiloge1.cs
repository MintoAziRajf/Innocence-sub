using NTextManager;

public class Epiloge1 : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        //   SceneManager.LoadScene(""); どのシーンに遷移するかは保留
    }

    protected override int lineIsTheEnd()
    {
        int lineIsTheEnd = 6;
        return lineIsTheEnd;
    }
}
