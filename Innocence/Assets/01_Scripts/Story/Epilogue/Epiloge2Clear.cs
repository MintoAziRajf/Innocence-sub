using NTextManager;

public class Epiloge2Clear : TextManager
{
    protected override void Blackout()
    {
        fade.FadeOut(0, () => fade.FadeIn(3));
        //   SceneManager.LoadScene(""); �ǂ̃V�[���ɑJ�ڂ��邩�͕ۗ�
    }

    protected override int lineIsTheEnd()
    {
        int lineIsTheEnd = 6;
        return lineIsTheEnd;
    }
}
