using NTextManager;

public class Epiloge1Clear : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        //   SceneManager.LoadScene(""); ‚Ç‚ÌƒV[ƒ“‚É‘JˆÚ‚·‚é‚©‚Í•Û—¯
    }

    protected override int lineIsTheEnd()
    {
        int lineIsTheEnd = 4;
        return lineIsTheEnd;
    }
}
