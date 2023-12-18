using UnityEngine;
public class MyGizmo : MonoBehaviour
{
    public Color Color = Color.magenta;
    public float _rad = 0.1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color;
        Gizmos.DrawSphere(transform.position, _rad);
    }
}
