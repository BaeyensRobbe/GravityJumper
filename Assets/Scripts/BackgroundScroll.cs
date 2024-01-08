using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speed = 0.5f;

    public Renderer renderer;

    void Update()
    {

        renderer.material.mainTextureOffset = new Vector2(speed * Time.deltaTime, 0);
    }
}
