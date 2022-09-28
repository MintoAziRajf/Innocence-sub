using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class TextTransition : MonoBehaviour
{
    [SerializeField] private GameObject messageObj = null;
    [SerializeField] private string messageString = null;
    private StringBuilder messageText = new StringBuilder("");

    private TextMesh textMesh = null;
    
    private int currentNum;
    private int randomNum;
    private const string characters = "abcdefghijklmnopqrstuvwxyz";

    void Start()
    {
        textMesh = messageObj.GetComponent<TextMesh>();
        StartCoroutine(TextAnimation());
    }

    private IEnumerator TextAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        for (int j = 0; j < messageString.Length; j++)
        {
            messageText.Append(RandomChar(messageString.Length - j));
            textMesh.text = messageText.ToString();

            for (int k = 0; k < 5; k++)
            {
                yield return null;
                messageText.Remove(j, messageString.Length - j);
                messageText.Append(RandomChar(messageString.Length - j));
                textMesh.text = messageText.ToString();
            }

            messageText.Remove(j, messageString.Length - j);
            messageText.Append(messageString[j]);
            textMesh.text = messageText.ToString();
        }
    }

    private string RandomChar(int value)
    {
        string str = "";
        for(int i = 0; i < value; i++)
        {
            randomNum = Random.Range(0, 25);
            str += characters[randomNum];
        }
        return str;
    }
}
