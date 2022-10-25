using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    //成功:s 失敗:f
    [SerializeField] private GameObject s_Effect = null;
    [SerializeField] private Material f_Material = null;
    private MeshRenderer myRenderer = null;
    private bool isSuccess = false;

    private void OnEnable()
    {
        myRenderer = this.GetComponent<MeshRenderer>();
    }
    public void NodeSuccess()
    {
        myRenderer.enabled = false;
        GameObject obj = Instantiate(s_Effect, this.transform.position, this.transform.rotation, this.transform); //エフェクトを生成
        Destroy(obj, 2f);
        isSuccess = true;
    }
    public void NodeExit()
    {
        if (isSuccess) return;
        SoundManager.instance.PlaySE(SoundManager.SE_Type.SC_Miss);
        myRenderer.material = f_Material;
    }
}
