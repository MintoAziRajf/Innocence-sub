using NTextManager;

public class Prologue : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        CSVManager.instance.Stages = -1;
        CSVManager.instance.LoadGame();
    }
}
