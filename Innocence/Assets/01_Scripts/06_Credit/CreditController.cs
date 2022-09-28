using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour
{
    [SerializeField] private GameObject loadPrefab = null;
    private void Start()
    {
        StartCoroutine(WaitEsc());
    }

    private IEnumerator WaitEsc()
    {
        yield return new WaitForSeconds(1.0f);
        yield return new WaitWhile(() => !Input.GetKeyDown(KeyCode.Space));
        GameObject SceneLoader = Instantiate(loadPrefab);
        Loading loading = SceneLoader.GetComponent<Loading>();
        loading.StartCoroutine("SceneLoading", "00_Title");
    }
}
