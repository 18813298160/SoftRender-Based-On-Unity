using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// VAO(vertex array object), 指定了读取VBO的方式
/// </summary>
public class VertexArrayObject
{
    /// <summary>
    /// VBO:(vertex buffer object),用于存放渲染所需要的数据
    /// </summary>
    public a2v[] VBO;
	/// <summary>
	/// EBO:索引缓冲对象（Element Buffer Object）, 是vbo的索引，
    /// 每3个索引表示一个三角面。相当于Unity中mesh的triangles
	/// </summary>
	public int[] EBO;

    public VertexArrayObject(MeshFilter filter)
    {
        Mesh mesh = filter.sharedMesh;
        FillVAO(mesh);
    }

    public VertexArrayObject(Mesh mesh)
    {
		FillVAO(mesh);
	}

    public void FillVAO(Mesh mesh)
    {
		int cnt = mesh.vertexCount;
		VBO = new a2v[cnt];
		for (int i = 0; i < cnt; i++)
		{
			VBO[i] = new a2v(mesh.vertices[i],
							 mesh.normals[i],
							 mesh.uv[i]);

		}

		EBO = mesh.triangles;
    }
}
