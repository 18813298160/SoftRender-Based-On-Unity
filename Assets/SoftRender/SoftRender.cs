using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 软渲染器（无优化与封装），可代替MeshRenderer组件进行渲染
/// </summary>
public partial class SoftRender
{
    public const int width = 1280;
    public const int height = 720;
    VertexArrayObject VAO;
    List<Vertex> SrVertices;
    List<Triangle> SrTriangles;
    BaseShader srShader;

    FrameBuffer frameBuffer;
	float[,] zBuffer = new float[1280, 720];

    Matrix4x4 L2WMat;
    Matrix4x4 MVPMat;

    bool blend = true;

	/// <summary>
	/// 这里仅仅是使用从filter获取网格对象，可以将一个mesh保存在Assets中，然后使用
    /// 或者一个GameObject只挂载MeshFilter组件，不挂载MeshRenderer组件，采用此softRenderer进行渲染
	/// </summary>
	public SoftRender(MeshFilter filter, Texture2D tex, Camera mainCamera)
    {
        frameBuffer = new FrameBuffer(tex);
        VAO = new VertexArrayObject(filter);
        SrVertices = new List<Vertex>(1024 * 4);
        SrTriangles = new List<Triangle>(1024 * 4);
        L2WMat = filter.transform.localToWorldMatrix;
		//矩阵乘法不满足交换律，顺序不可变
		MVPMat = mainCamera.projectionMatrix * mainCamera.worldToCameraMatrix * L2WMat;
    }

    /// <summary>
    /// 此mesh要和这个Trans保持一致（仅供测试）
    /// </summary>

    public SoftRender(Mesh meshAsset, Texture2D tex, Camera mainCamera, Transform targetTrans)
    {
		frameBuffer = new FrameBuffer(tex);
        VAO = new VertexArrayObject(meshAsset);
		SrVertices = new List<Vertex>(1024 * 4);
		SrTriangles = new List<Triangle>(1024 * 4);
		L2WMat = targetTrans.localToWorldMatrix;
		//矩阵乘法不满足交换律，顺序不可变
		MVPMat = mainCamera.projectionMatrix * mainCamera.worldToCameraMatrix * L2WMat;
    }

    public void Draw(bool save = true)
    {
        frameBuffer.SetPixels(0.2f, 0.3f, 0.4f);
        InitShader();
        RunVertShader();
        SetupTriangles();
        Rasterization();
        if (save)
        {
            frameBuffer.Save();
        }
    }

    /// <summary>
    /// 初始化shader
    /// </summary>
    private void InitShader()
    {
        srShader = new BaseShader(L2WMat, MVPMat);
    }

    /// <summary>
    /// 顶点处理
    /// </summary>
    private void RunVertShader()
    {
        for (int i = 0; i < VAO.VBO.Length; i++)
        {
            a2v a = VAO.VBO[i];
            Vertex vert = srShader.VertShader(a);
            SrVertices.Add(vert);
        }
    }

    /// <summary>
    /// 设置三角形
    /// </summary>
    private void SetupTriangles()
    {
        for (int i = 0; i < VAO.EBO.Length; i += 3)
        {
            Triangle tri = new Triangle(
                SrVertices[VAO.EBO[i]],
                SrVertices[VAO.EBO[i + 1]],
                SrVertices[VAO.EBO[i + 2]]
            );
            SrTriangles.Add(tri);
        }
    }

    /// <summary>
    /// 光栅化
    /// </summary>
    public void Rasterization()
    {
        for (int i = 0; i < SrTriangles.Count; i++)
        {
            var fl = Rast(SrTriangles[i]);
            foreach (var frag in fl)
            {
                if (frag.z > zBuffer[frag.x, frag.y] && zBuffer[frag.x, frag.y] != 0) continue;
                Color col = srShader.FragShader(frag.data);
                if (blend)
                {
                    Color t = frameBuffer.GetPixel(frag.x, frag.y);
                    float r = t.r * (1 - col.a) + col.r * col.a;
                    float g = t.g * (1 - col.a) + col.g * col.a;
                    float b = t.b * (1 - col.a) + col.b * col.a;
                    frameBuffer.SetPixel(frag.x, frag.y, new Color(r, g, b)); 
                }
                else
                {
                    frameBuffer.SetPixel(frag.x, frag.y, col);
                }
                zBuffer[frag.x, frag.y] = frag.z;
            }
        }
    }
}
