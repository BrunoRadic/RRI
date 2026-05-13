#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class SceneBuilder
{
    [MenuItem("GameObject/Lab2/Build Test Scene", false, 0)]
    static void Build()
    {
        // Ground
        CreateBox("Ground",
            new Vector3(0f, -0.25f, 0f),
            new Vector3(40f, 0.5f, 40f),
            new Color(0.45f, 0.65f, 0.35f));

        // Walls
        CreateBox("Wall_North", new Vector3(  0f, 1.5f,  20f), new Vector3(40f, 3f, 0.5f), Color.gray);
        CreateBox("Wall_South", new Vector3(  0f, 1.5f, -20f), new Vector3(40f, 3f, 0.5f), Color.gray);
        CreateBox("Wall_East",  new Vector3( 20f, 1.5f,   0f), new Vector3(0.5f, 3f, 40f), Color.gray);
        CreateBox("Wall_West",  new Vector3(-20f, 1.5f,   0f), new Vector3(0.5f, 3f, 40f), Color.gray);

        // Staircase
        for (int i = 0; i < 5; i++)
        {
            float h = (i + 1) * 0.3f;
            CreateBox($"Step_{i}",
                new Vector3(-8f, h * 0.5f, -6f + i * 0.65f),
                new Vector3(3f, h, 0.65f),
                new Color(0.72f, 0.56f, 0.36f));
        }

        // Tunnel
        float clearH = 1.15f;
        float slabT  = 0.3f;
        float tW     = 4f;
        float tL     = 7f;
        float tX     = 6f;
        float tZ     = 4f;

        CreateBox("Tunnel_Ceiling",
            new Vector3(tX, clearH + slabT * 0.5f, tZ),
            new Vector3(tW, slabT, tL),
            new Color(0.5f, 0.5f, 0.5f));

        CreateBox("Tunnel_WallLeft",
            new Vector3(tX - tW * 0.5f - slabT * 0.5f, clearH * 0.5f, tZ),
            new Vector3(slabT, clearH, tL),
            new Color(0.5f, 0.5f, 0.5f));

        CreateBox("Tunnel_WallRight",
            new Vector3(tX + tW * 0.5f + slabT * 0.5f, clearH * 0.5f, tZ),
            new Vector3(slabT, clearH, tL),
            new Color(0.5f, 0.5f, 0.5f));

        // Platform
        float platTop = 2.0f;
        CreateBox("Platform",
            new Vector3(8f, platTop - slabT * 0.5f, -4f),
            new Vector3(6f, slabT, 6f),
            new Color(0.4f, 0.4f, 0.8f));

        // Ramp
        float rampLen   = 6f;
        float rampAngle = Mathf.Atan2(platTop, rampLen) * Mathf.Rad2Deg;
        var ramp = CreateBox("Platform_Ramp",
            new Vector3(8f, platTop * 0.5f, -7f - rampLen * 0.5f + rampLen),
            new Vector3(3f, 0.3f, rampLen + 0.5f),
            new Color(0.35f, 0.35f, 0.7f));
        ramp.transform.rotation = Quaternion.Euler(-rampAngle, 0f, 0f);

        // Cover boxes
        CreateBox("Cover_A", new Vector3(-3f,  0.5f, -5f), new Vector3(1f, 1f, 1f), new Color(0.8f, 0.3f, 0.3f));
        CreateBox("Cover_B", new Vector3( 3f,  1.0f, -9f), new Vector3(1f, 2f, 1f), new Color(0.8f, 0.3f, 0.3f));
        CreateBox("Cover_C", new Vector3(-6f,  0.5f, -2f), new Vector3(2f, 1f, 2f), new Color(0.8f, 0.3f, 0.3f));

        Debug.Log("[Lab2] Scene built. Set Player position to X=0, Y=1.0, Z=-15 before pressing Play.");
    }

    static GameObject CreateBox(string name, Vector3 pos, Vector3 scale, Color color)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name              = name;
        go.transform.position  = pos;
        go.transform.localScale = scale;

        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
        {
            Debug.LogWarning($"[Lab2] URP shader not found for '{name}'. Object will be pink. Check Graphics Settings.");
            return go;
        }

        var mat = new Material(shader);
        mat.SetColor("_BaseColor", color);
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }
}
#endif
