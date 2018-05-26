using UnityEngine;
using System.IO;

public class FrameBuffer
{
	public readonly int width;
	public readonly int height;
    private Texture2D tex2D;
	private Color[] cols;

    public FrameBuffer(Texture2D tex)
    {
        tex2D = tex ? tex : new Texture2D(SoftRender.width, SoftRender.height, TextureFormat.RGBA32, false);
        cols = tex2D.GetPixels();
        width = tex2D.width;
        height = tex2D.height;
    }

    public FrameBuffer(int w, int h)
    {
        width = w;
        height = h;
        cols = new Color[w * h];
    }

    public void Clear()
    {
        for (int i = 0; i < cols.Length; i++)
        {
			cols[i].r = 0;
			cols[i].g = 0;
			cols[i].b = 0;
        }
    }

    public void SetPixels(float r, float g, float b)
    {
		for (int i = 0; i < cols.Length; i++)
		{
			cols[i].r = r;
			cols[i].g = g;
			cols[i].b = b;
		}
    }

    public Color[] GetPixels()
    {
        return cols;
    }

    public Color GetPixel(int x, int y)
    {
        x = MathUtil.CMID(x, 0, width - 1);
        y = MathUtil.CMID(y, 0, height - 1);
        return cols[y * width + x];
    }

    public Color GetPixel(float u, float v)
    {
		int x = (int)(u * width + 0.49f);
		int y = (int)(v * height + 0.49f);
        return GetPixel(x, y);
    }

    public void SetPixel(int x, int y, Color c)
	{
        cols[y * width + x] = c;
	}

    public void Save()
    {
		Texture2D t = new Texture2D(width, height, TextureFormat.RGBA32, false);
		t.SetPixels(cols);
		byte[] bytes = t.EncodeToPNG();
		FileStream file = File.Open("Assets/tmp.png", FileMode.Create);
		BinaryWriter writer = new BinaryWriter(file);
		writer.Write(bytes);
		file.Close();
		Texture2D.DestroyImmediate(t);
		t = null;
    }
}
