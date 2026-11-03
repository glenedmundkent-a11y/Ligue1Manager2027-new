using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MatchController : MonoBehaviour
{
    private List<GameObject> home, away;
    private GameObject ball;
    private Rigidbody ballBody;
    private string homeName, awayName;
    private int homeGoals, awayGoals, seconds;
    private float clockAccumulator;
    private bool paused;
    private Text scoreText, clockText, eventText;
    private float nextEvent = 8f;

    public void Initialize(List<GameObject> h, List<GameObject> a, GameObject matchBall, string hn, string an)
    {
        home = h; away = a; ball = matchBall; ballBody = ball.GetComponent<Rigidbody>();
        homeName = hn; awayName = an;
        BuildHUD();
        UpdateHUD("Kick-off!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) paused = !paused;
        if (Input.GetKeyDown(KeyCode.R)) Restart();
        if (paused) { Time.timeScale = 0; return; }
        Time.timeScale = 1;
        clockAccumulator += Time.deltaTime * 2.5f;
        seconds = Mathf.FloorToInt(clockAccumulator);
        clockText.text = (seconds / 60).ToString("00") + ":" + (seconds % 60).ToString("00");
        nextEvent -= Time.deltaTime;
        if (nextEvent <= 0) SimulateMoment();
        ChaseBall(home); ChaseBall(away);
        if (ball.transform.position.y < -2 || Mathf.Abs(ball.transform.position.x) > 60 || Mathf.Abs(ball.transform.position.z) > 42)
        {
            ballBody.velocity = Vector3.zero;
            ball.transform.position = Vector3.zero;
        }
    }

    void ChaseBall(List<GameObject> team)
    {
        GameObject closest = null; float best = float.MaxValue;
        foreach (var p in team)
        {
            float d = (p.transform.position - ball.transform.position).sqrMagnitude;
            if (d < best) { best = d; closest = p; }
        }
        if (closest != null && best > 5f)
            closest.GetComponent<PlayerAgent>().SetTarget(ball.transform.position + new Vector3(Random.Range(-2f,2f), 0, Random.Range(-2f,2f)));
    }

    void SimulateMoment()
    {
        nextEvent = Random.Range(6f, 13f);
        int roll = Random.Range(0, 100);
        if (roll < 12)
        {
            bool homeAttack = Random.value > 0.5f;
            var shooter = homeAttack ? home[Random.Range(1, home.Count)] : away[Random.Range(1, away.Count)];
            Vector3 goal = new Vector3(homeAttack ? 54 : -54, 1.2f, Random.Range(-2.5f, 2.5f));
            Vector3 dir = (goal - shooter.transform.position).normalized;
            ball.transform.position = shooter.transform.position + Vector3.up * 0.3f;
            ballBody.velocity = dir * Random.Range(17f, 24f) + Vector3.up * 3f;
            UpdateHUD(homeAttack ? "Attack building — shot!" : "Counter-attack — shot!");
            if (Random.value < 0.22f)
            {
                if (homeAttack) homeGoals++; else awayGoals++;
                UpdateHUD("GOAL! " + (homeAttack ? homeName : awayName));
            }
        }
        else if (roll < 48)
        {
            var team = Random.value > 0.5f ? home : away;
            var p = team[Random.Range(0, team.Count)];
            p.GetComponent<PlayerAgent>().SetTarget(new Vector3(Random.Range(-40f,40f), 1, Random.Range(-28f,28f)));
            UpdateHUD("Midfield battle — possession changes");
        }
        else UpdateHUD("Play continues");
    }

    void BuildHUD()
    {
        var canvasGO = new GameObject("Match HUD");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasGO.AddComponent<GraphicRaycaster>();
        scoreText = MakeText(canvas.transform, homeName + "  0 - 0  " + awayName, 28, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -35), new Vector2(900, 70));
        clockText = MakeText(canvas.transform, "00:00", 22, new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -95), new Vector2(180, 45));
        eventText = MakeText(canvas.transform, "Kick-off", 20, new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 35), new Vector2(700, 50));
    }

    Text MakeText(Transform parent, string value, int size, Vector2 anchor, Vector2 pivot, Vector2 pos, Vector2 dimensions)
    {
        var go = new GameObject("HUD Text");
        go.transform.SetParent(parent, false);
        var rect = go.AddComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor; rect.pivot = pivot;
        rect.anchoredPosition = pos; rect.sizeDelta = dimensions;
        var t = go.AddComponent<Text>();
        t.text = value; t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        t.fontSize = size; t.alignment = TextAnchor.MiddleCenter; t.color = Color.white;
        t.fontStyle = FontStyle.Bold;
        return t;
    }

    void UpdateHUD(string message)
    {
        if (scoreText != null) scoreText.text = homeName + "  " + homeGoals + " - " + awayGoals + "  " + awayName;
        if (eventText != null) eventText.text = message;
    }

    void Restart()
    {
        homeGoals = awayGoals = seconds = 0;
        clockAccumulator = 0f;
        ball.transform.position = Vector3.zero;
        ballBody.velocity = Vector3.zero;
        nextEvent = 5;
        UpdateHUD("New kick-off!");
    }
}
