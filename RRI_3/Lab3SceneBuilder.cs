#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class Lab3SceneBuilder
{
    [MenuItem("GameObject/Lab3/Build Full Scene", false, 0)]
    static void Build()
    {
        BuildGeometry();
        BuildPlayer();
        BuildLab3Objects();
        BuildScoreManager();
        Debug.Log("[Lab3] Scena izgradena.");
    }

    // ─── GEOMETRIJA (Lab 2) ────────────────────────────────────

    static void BuildGeometry()
    {
        CreateBox("Ground",
            new Vector3(0f, -0.25f, 0f),
            new Vector3(40f, 0.5f, 40f),
            new Color(0.45f, 0.65f, 0.35f));

        CreateBox("Wall_North", new Vector3(  0f, 1.5f,  20f), new Vector3(40f, 3f, 0.5f), Color.gray);
        CreateBox("Wall_South", new Vector3(  0f, 1.5f, -20f), new Vector3(40f, 3f, 0.5f), Color.gray);
        CreateBox("Wall_East",  new Vector3( 20f, 1.5f,   0f), new Vector3(0.5f, 3f, 40f), Color.gray);
        CreateBox("Wall_West",  new Vector3(-20f, 1.5f,   0f), new Vector3(0.5f, 3f, 40f), Color.gray);

        for (int i = 0; i < 5; i++)
        {
            float h = (i + 1) * 0.3f;
            CreateBox("Step_" + i,
                new Vector3(-8f, h * 0.5f, -6f + i * 0.65f),
                new Vector3(3f, h, 0.65f),
                new Color(0.72f, 0.56f, 0.36f));
        }

        float clearH = 1.15f, slabT = 0.3f, tW = 4f, tL = 7f, tX = 6f, tZ = 4f;
        CreateBox("Tunnel_Ceiling",
            new Vector3(tX, clearH + slabT * 0.5f, tZ),
            new Vector3(tW, slabT, tL), new Color(0.5f, 0.5f, 0.5f));
        CreateBox("Tunnel_WallLeft",
            new Vector3(tX - tW * 0.5f - slabT * 0.5f, clearH * 0.5f, tZ),
            new Vector3(slabT, clearH, tL), new Color(0.5f, 0.5f, 0.5f));
        CreateBox("Tunnel_WallRight",
            new Vector3(tX + tW * 0.5f + slabT * 0.5f, clearH * 0.5f, tZ),
            new Vector3(slabT, clearH, tL), new Color(0.5f, 0.5f, 0.5f));

        float platTop = 2.0f;
        CreateBox("Platform",
            new Vector3(8f, platTop - slabT * 0.5f, -4f),
            new Vector3(6f, slabT, 6f), new Color(0.4f, 0.4f, 0.8f));

        float rampLen = 6f;
        float rampAngle = Mathf.Atan2(platTop, rampLen) * Mathf.Rad2Deg;
        var ramp = CreateBox("Platform_Ramp",
            new Vector3(8f, platTop * 0.5f, -7f - rampLen * 0.5f + rampLen),
            new Vector3(3f, 0.3f, rampLen + 0.5f), new Color(0.35f, 0.35f, 0.7f));
        ramp.transform.rotation = Quaternion.Euler(-rampAngle, 0f, 0f);

        CreateBox("Cover_A", new Vector3(-3f, 0.5f, -5f), new Vector3(1f, 1f, 1f), new Color(0.8f, 0.3f, 0.3f));
        CreateBox("Cover_B", new Vector3( 3f, 1.0f, -9f), new Vector3(1f, 2f, 1f), new Color(0.8f, 0.3f, 0.3f));
        CreateBox("Cover_C", new Vector3(-6f, 0.5f, -2f), new Vector3(2f, 1f, 2f), new Color(0.8f, 0.3f, 0.3f));
    }

    // ─── PLAYER ───────────────────────────────────────────────

    static void BuildPlayer()
    {
        // Capsule kao vizualni mesh
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0f, 1f, 0f);
        player.tag = "Player";

        // Ukloni CapsuleCollider jer CharacterController ima vlastiti
        Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());

        // CharacterController
        CharacterController cc = player.AddComponent<CharacterController>();
        cc.height = 2.0f;
        cc.radius = 0.4f;
        cc.center = new Vector3(0f, 0f, 0f);

        // PlayerController skripta
        player.AddComponent<PlayerController>();

        SetURPColor(player, new Color(0.2f, 0.5f, 1f));

        // Kamera kao child
        GameObject camObj = new GameObject("PlayerCamera");
        camObj.transform.SetParent(player.transform);
        camObj.transform.localPosition = new Vector3(0f, 0.7f, 0f);
        camObj.transform.localRotation = Quaternion.identity;
        camObj.AddComponent<Camera>();

        Undo.RegisterCreatedObjectUndo(player, "Create Player");
        Debug.Log("[Lab3] Player kreiran.");
    }

    // ─── LAB 3 OBJEKTI ────────────────────────────────────────

    static void BuildLab3Objects()
    {
        CreateCollectible("Collectible_1", new Vector3( 3f, 1f,  3f));
        CreateCollectible("Collectible_2", new Vector3(-3f, 1f,  6f));
        CreateCollectible("Collectible_3", new Vector3( 0f, 1f, 10f));

        CreateHazard("Hazard_A", new Vector3( 5f, 0.5f,  5f), new Vector3(1f, 1f, 1f));
        CreateHazard("Hazard_B", new Vector3(-5f, 1.0f, -3f), new Vector3(1f, 2f, 1f));

        CreateTriggerZone("SpeedBoostZone", new Vector3(8f, 1f, 8f),
            new Vector3(5f, 2f, 5f), typeof(SpeedBoostZone), new Color(0f, 1f, 0.5f, 0.3f));

        CreateTriggerZone("GravityZone", new Vector3(-8f, 1f, 8f),
            new Vector3(5f, 2f, 5f), typeof(GravityZone), new Color(0.5f, 0f, 1f, 0.3f));

        CreateGate("Gate_1", new Vector3(0f, 1.5f, 15f));
    }

    // ─── SCORE MANAGER ────────────────────────────────────────

    static void BuildScoreManager()
    {
        GameObject go = new GameObject("ScoreManager");
        go.AddComponent<ScoreManager>();
        Undo.RegisterCreatedObjectUndo(go, "Create ScoreManager");
    }

    // ─── HELPERI ──────────────────────────────────────────────

    static void CreateCollectible(string name, Vector3 pos)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.transform.position = pos;
        go.transform.localScale = Vector3.one * 0.5f;
        go.GetComponent<SphereCollider>().isTrigger = true;
        SetURPColor(go, new Color(1f, 0.85f, 0f));
        go.AddComponent<CollectibleItem>();
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
    }

    static void CreateHazard(string name, Vector3 pos, Vector3 scale)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.position = pos;
        go.transform.localScale = scale;
        SetURPColor(go, new Color(0.8f, 0.2f, 0.2f));
        go.AddComponent<HazardObject>();
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
    }

    static void CreateTriggerZone(string name, Vector3 pos, Vector3 scale,
                                   System.Type scriptType, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.position = pos;
        go.transform.localScale = scale;
        go.GetComponent<BoxCollider>().isTrigger = true;
        SetURPColor(go, color);
        go.AddComponent(scriptType);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
    }

    static void CreateGate(string name, Vector3 pos)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.position = pos;
        go.transform.localScale = new Vector3(4f, 3f, 0.3f);
        SetURPColor(go, new Color(0.6f, 0.4f, 0.1f));
        go.AddComponent<InteractableGate>();
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
    }

    static GameObject CreateBox(string name, Vector3 pos, Vector3 scale, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.position = pos;
        go.transform.localScale = scale;
        SetURPColor(go, color);
        Undo.RegisterCreatedObjectUndo(go, "Create " + name);
        return go;
    }

    static void SetURPColor(GameObject go, Color color)
    {
        Renderer rend = go.GetComponent<Renderer>();
        if (rend == null) return;
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.SetColor("_BaseColor", color);
        rend.sharedMaterial = mat;
    }
}
#endif
