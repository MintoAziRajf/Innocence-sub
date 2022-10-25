using NTextManager;

public class Epiloge3Clear : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        CSVManager.instance.LoadGame();
    }

    protected override int lineIsTheEnd()
    {
        int lineIsTheEnd = 11;
        return lineIsTheEnd;
    }
}
