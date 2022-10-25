using NTextManager;

public class Epiloge2 : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        CSVManager.instance.LoadGame(true);
    }

    protected override int lineIsTheEnd()
    {
        int lineIsTheEnd = 5;
        return lineIsTheEnd;
    }
}
