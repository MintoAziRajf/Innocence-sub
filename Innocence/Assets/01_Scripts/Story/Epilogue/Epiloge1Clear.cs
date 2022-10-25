using NTextManager;

public class Epiloge1Clear : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        CSVManager.instance.LoadGame();
    }

    protected override int lineIsTheEnd()
    {
        int lineIsTheEnd = 4;
        return lineIsTheEnd;
    }
}
