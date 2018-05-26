public struct Vertex
{
	public float x;
	public float y;
	public float z;
    public v2f data;
}

public struct Triangle
{
    private Vertex[] verts;
    public Triangle(Vertex a, Vertex b, Vertex c)
    {
        verts = new Vertex[3];
		verts[0] = a;
		verts[1] = b;
		verts[2] = c;
    }

    public Vertex this[int idx]
    {
        get
        {
            return verts[idx];
        }
    }
}

public struct Fragment
{
	public int x;
	public int y;
	public float z;
	public v2f data;
	public float fx
	{
		get
		{
			return x + 0.5f;
		}
	}
	public float fy
	{
		get
		{
			return y + 0.5f;
		}
	}
}


