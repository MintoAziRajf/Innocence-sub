using NTextManager;

public class Epiloge1 : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        //   SceneManager.LoadScene(""); ‚Ç‚ÌƒV[ƒ“‚É‘JˆÚ‚·‚é‚©‚Í•Û—¯
    }

    protected override int lineIsTheEnd()
    {
        int lineIsTheEnd = 6;
        return lineIsTheEnd;
    }
}
