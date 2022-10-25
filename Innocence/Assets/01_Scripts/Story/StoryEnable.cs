using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEnable : MonoBehaviour
{
    [SerializeField] private GameObject storyObj = null;

    void Awake()
    {
        StartCoroutine(ActivateStory());
    }
    IEnumerator ActivateStory()
    {
        yield return new WaitForSeconds(1.0f);
        storyObj.SetActive(true);
    }
    public void InactivateStory()
    {
        storyObj.SetActive(false);
    }
}
