using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    //成功:s 失敗:f
    [SerializeField] private Material s_Material = null;
    [SerializeField] private Material f_Material = null;
    private MeshRenderer myRenderer = null;
    private bool isSuccess = false;

    private void OnEnable()
    {
        myRenderer = this.GetComponent<MeshRenderer>();
    }
    public void NodeSuccess()
    {
        myRenderer.material = s_Material;
        isSuccess = true;
    }
    public void NodeExit()
    {
        if (isSuccess) return;
        SoundManager.instance.PlaySE(SoundManager.SE_Type.SC_Miss);
        myRenderer.material = f_Material;
    }
}
