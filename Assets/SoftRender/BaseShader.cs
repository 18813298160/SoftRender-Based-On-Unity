using UnityEngine;
using System.Collections;

public class BaseShader
{
    private Matrix4x4 L2WMat;
    private Matrix4x4 MVPMat;

    public BaseShader(Matrix4x4 l2wMat, Matrix4x4 mvpMat)
    {
        L2WMat = l2wMat;
        MVPMat = mvpMat;
    }
    
    public virtual Vertex VertShader(a2v i)
    {
		v2f v = new v2f();
        v.postion = L2WMat.MultiplyPoint3x4(i.position);
		v.normal = L2WMat.MultiplyVector(i.normal);
		v.uv = i.uv;
		Vertex vert = new Vertex();
        Vector4 svp = i.position;
		svp.w = 1f;
		svp = MVPMat * svp;
		vert.data = v;
        vert.x = (svp.x / svp.w / 2 + 0.5f) * SoftRender.width;
        vert.y = (svp.y / svp.w / 2 + 0.5f) * SoftRender.height;
		vert.z = svp.z / svp.w / 2 + 0.5f;
		return vert;
    }

    public virtual Color FragShader(v2f f)
    {
        return Color.white;
    }

}
