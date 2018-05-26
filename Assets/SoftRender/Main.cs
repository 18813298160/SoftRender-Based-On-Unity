using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public Transform targetTrans;
	private MeshFilter filter;
    private Camera mainCamera;
    private Light[] lights;
    private SoftRender sr;

	void Awake () 
    {
        mainCamera = GetComponentInChildren<Camera>();
        lights = GetComponentsInChildren<Light>();
        filter = targetTrans.GetComponent<MeshFilter>();
        var unityMeshRenderer = targetTrans.GetComponent<MeshRenderer>();
        if (unityMeshRenderer)
        {
			DestroyImmediate(unityMeshRenderer);
            Debug.Log("测试SoftRender， 不需要Unity自带的渲染组件，已除去。");
        }
        sr = new SoftRender(filter, null, mainCamera);
        sr.Draw();
	}
    /// <summary>
    /// projectionMatrix的求法, 只是写出具体过程，并没有使用，
    /// 因为结果和mainCamera.projectionMatrix是一样的
    /// </summary>
    private Matrix4x4 TryGetMVPMat()
    {
        Matrix4x4 GetMVPMat = new Matrix4x4();
		float near = mainCamera.nearClipPlane;
        float far = mainCamera.farClipPlane;
        float aspect = mainCamera.aspect;
        float fov = mainCamera.fieldOfView * Mathf.PI / 360.0f;
		GetMVPMat.m00 = 1f / (Mathf.Tan(fov) * aspect);
		GetMVPMat.m11 = 1f / Mathf.Tan(fov);
        GetMVPMat.m22 = (near + far)/(near - far);
        GetMVPMat.m23 = 2 * far * near / (near - far);
        GetMVPMat.m32 = -1f;
        return GetMVPMat;
    }

}
