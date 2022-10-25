using NTextManager;

public class Epiloge4 : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        CSVManager.instance.LoadGame(true);
    }

    protected override int lineIsTheEnd()
    {
        int lineIsTheEnd = 9;
        return lineIsTheEnd;
    }
}
