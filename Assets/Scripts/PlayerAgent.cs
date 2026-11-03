using UnityEngine;

public class PlayerAgent : MonoBehaviour
{
    public float moveSpeed = 5.2f;
    private Vector3 target;

    void Start() { target = transform.position; }

    public void SetTarget(Vector3 point)
    {
        target = new Vector3(point.x, 1f, point.z);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        Vector3 flat = target - transform.position; flat.y = 0;
        if (flat.sqrMagnitude > 0.05f) transform.rotation = Quaternion.LookRotation(flat.normalized);
    }
}
