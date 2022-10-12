using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    [SerializeField] RectTransform button;
   
    private int counter = 0;
    private float move = 0.005f;

    void Update()
    {
        button.position += new Vector3(0, move, 0);
        counter++;
        if (counter == 500)
        {
            counter = 0;
            move *= -1;
        }
    }
}
