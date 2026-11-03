using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameBootstrap : MonoBehaviour
{
    public string homeClub = "Paris Saint-Germain";
    public string awayClub = "Olympique de Marseille";

    private MatchController match;

    void Start()
    {
        Application.targetFrameRate = 60;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        BuildLighting();
        BuildStadium();
        BuildPitch();
        BuildGoals();
        var home = BuildTeam(homeClub, new Vector3(-18, 0, 0), new Color(0.08f, 0.22f, 0.65f));
        var away = BuildTeam(awayClub, new Vector3(18, 0, 0), new Color(0.85f, 0.12f, 0.12f));
        var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Match Ball";
        ball.transform.position = new Vector3(0, 0.35f, 0);
        ball.transform.localScale = Vector3.one * 0.7f;
        ball.GetComponent<Renderer>().material = MakeMat(Color.white);
        var rb = ball.AddComponent<Rigidbody>();
        rb.mass = 0.43f;
        rb.drag = 0.15f;
        rb.angularDrag = 0.08f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        match = gameObject.AddComponent<MatchController>();
        match.Initialize(home, away, ball, homeClub, awayClub);
        BuildCamera();
    }

    void BuildLighting()
    {
        RenderSettings.ambientLight = new Color(0.48f, 0.52f, 0.58f);
        var sun = new GameObject("Stadium Floodlights").AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.25f;
        sun.transform.rotation = Quaternion.Euler(48, -32, 0);
    }

    void BuildPitch()
    {
        var grass = GameObject.CreatePrimitive(PrimitiveType.Cube);
        grass.name = "Pitch";
        grass.transform.position = new Vector3(0, -0.25f, 0);
        grass.transform.localScale = new Vector3(105, 0.5f, 68);
        grass.GetComponent<Renderer>().material = MakeMat(new Color(0.08f, 0.34f, 0.16f));
        for (int i = 0; i < 10; i++)
        {
            var stripe = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stripe.name = "Mowed Stripe";
            stripe.transform.position = new Vector3(-47 + i * 10.4f, 0.012f, 0);
            stripe.transform.localScale = new Vector3(5.2f, 0.02f, 67.5f);
            stripe.GetComponent<Renderer>().material = MakeMat(new Color(0.10f, 0.39f, 0.18f));
        }
        Line(new Vector3(0, 0.04f, -34), new Vector3(0, 0.04f, 34), Color.white, 0.12f);
        Circle(new Vector3(0, 0.05f, 0), 9.15f, Color.white);
        BoxLine(new Vector3(-52, 0.05f, 0), new Vector3(-35, 0.05f, 0), 0.12f, 40, Color.white);
        BoxLine(new Vector3(52, 0.05f, 0), new Vector3(35, 0.05f, 0), 0.12f, 40, Color.white);
    }

    void BuildGoals()
    {
        foreach (int x in new int[] { -1, 1 })
        {
            float gx = x * 53f;
            for (int z = -3; z <= 3; z += 6)
            {
                var post = GameObject.CreatePrimitive(PrimitiveType.Cube);
                post.transform.position = new Vector3(gx, 1.2f, z);
                post.transform.localScale = new Vector3(0.25f, 2.4f, 0.25f);
                post.GetComponent<Renderer>().material = MakeMat(Color.white);
            }
            var bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.transform.position = new Vector3(gx, 2.4f, 0);
            bar.transform.localScale = new Vector3(0.25f, 0.25f, 6.25f);
            bar.GetComponent<Renderer>().material = MakeMat(Color.white);
        }
    }

    void BuildStadium()
    {
        for (int side = -1; side <= 1; side += 2)
        {
            var stand = GameObject.CreatePrimitive(PrimitiveType.Cube);
            stand.name = "Grandstand";
            stand.transform.position = new Vector3(0, 4, side * 42);
            stand.transform.localScale = new Vector3(112, 8, 12);
            stand.GetComponent<Renderer>().material = MakeMat(new Color(0.12f, 0.15f, 0.20f));
            for (int row = 0; row < 3; row++)
            {
                var tier = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tier.transform.position = new Vector3(0, 8 + row * 1.2f, side * (36 + row * 2));
                tier.transform.localScale = new Vector3(108 - row * 3, 0.5f, 2);
                tier.GetComponent<Renderer>().material = MakeMat(new Color(0.22f, 0.25f, 0.31f));
            }
        }
        // Simple colored crowd blocks as a visual placeholder.
        for (int i = 0; i < 180; i++)
        {
            float x = Random.Range(-52f, 52f);
            float z = Random.value < 0.5f ? -41f : 41f;
            var fan = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fan.name = "Crowd Placeholder";
            fan.transform.position = new Vector3(x, Random.Range(4.2f, 9f), z);
            fan.transform.localScale = new Vector3(0.35f, 0.45f, 0.25f);
            fan.GetComponent<Renderer>().material = MakeMat(Random.ColorHSV(0, 1, 0.45f, 0.9f, 0.5f, 1));
        }
    }

    List<GameObject> BuildTeam(string club, Vector3 direction, Color kit)
    {
        var players = new List<GameObject>();
        for (int i = 0; i < 11; i++)
        {
            int row = i / 4;
            int col = i % 4;
            float z = (col - 1.5f) * 13f;
            float x = direction.x + (row - 1) * 10f;
            if (i == 0) { x = direction.x > 0 ? 43 : -43; z = 0; }
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = club + " Player " + (i + 1);
            player.transform.position = new Vector3(x, 1, z);
            player.transform.localScale = new Vector3(0.8f, 1f, 0.8f);
            player.GetComponent<Renderer>().material = MakeMat(kit);
            player.AddComponent<PlayerAgent>();
            players.Add(player);
        }
        return players;
    }

    void BuildCamera()
    {
        var camObj = new GameObject("Broadcast Camera");
        var cam = camObj.AddComponent<Camera>();
        cam.transform.position = new Vector3(0, 57, -78);
        cam.transform.rotation = Quaternion.Euler(34, 0, 0);
        cam.fieldOfView = 48;
        cam.tag = "MainCamera";
        camObj.AddComponent<AudioListener>();
    }

    Material MakeMat(Color c)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        var m = new Material(shader);
        m.color = c;
        return m;
    }

    void Line(Vector3 a, Vector3 b, Color c, float width)
    {
        var go = new GameObject("Pitch Marking");
        var lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2; lr.SetPositions(new Vector3[] { a, b });
        lr.startWidth = width; lr.endWidth = width;
        lr.material = MakeMat(c);
    }

    void Circle(Vector3 center, float radius, Color c)
    {
        var go = new GameObject("Center Circle");
        var lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 65; lr.loop = true; lr.startWidth = 0.12f; lr.endWidth = 0.12f;
        lr.material = MakeMat(c);
        for (int i = 0; i < 65; i++)
        {
            float a = i * Mathf.PI * 2 / 64;
            lr.SetPosition(i, center + new Vector3(Mathf.Cos(a) * radius, 0, Mathf.Sin(a) * radius));
        }
    }

    void BoxLine(Vector3 a, Vector3 b, float width, float depth, Color c)
    {
        Line(a + new Vector3(0,0,-depth/2), b + new Vector3(0,0,-depth/2), c, width);
        Line(a + new Vector3(0,0,depth/2), b + new Vector3(0,0,depth/2), c, width);
        Line(a + new Vector3(0,0,-depth/2), a + new Vector3(0,0,depth/2), c, width);
        Line(b + new Vector3(0,0,-depth/2), b + new Vector3(0,0,depth/2), c, width);
    }
}
