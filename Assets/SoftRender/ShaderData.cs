using UnityEngine;

public struct a2v
{
	public Vector3 position;
	public Vector3 normal;
	public Vector2 uv;

	public a2v(Vector3 pos, Vector3 nor, Vector2 uv)
	{
		position = pos;
		normal = nor;
		this.uv = uv;
	}
}

public struct v2f
{
	public Vector3 postion;
	public Vector3 normal;
	public Vector2 uv;

	public float this[int i]
    {
        get
        {
            switch (i)
            {
                case 0:
                    return postion.x;
                case 1:
                    return postion.y;
                case 2:
                    return postion.z;
                case 3:
                    return normal.x;
                case 4:
                    return normal.y;
                case 5:
                    return normal.z;
                case 6:
                    return uv.x;
                case 7:
                    return uv.y;
            }
            return 0f;
        }
        set
        {
            switch (i)
            {
                case 0:
                    postion.x = value;
                    break;
                case 1:
                    postion.y = value;
                    break;
                case 2:
                    postion.z = value;
                    break;
                case 3:
                    normal.x = value;
                    break;
                case 4:
                    normal.y = value;
                    break;
                case 5:
                    normal.z = value;
                    break;
                case 6:
                    uv.x = value;
                    break;
                case 7:
                    uv.y = value;
                    break;
            }
        }
    }
}
