using Cinemachine;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    public float backgroundSpeed;
    public Renderer backgroundrenderer;
    public CinemachineVirtualCamera virtualCamera;

    void Update()
    {
        transform.position = new Vector2 (virtualCamera.transform.position.x, transform.position.y);
        backgroundrenderer.material.mainTextureOffset += new Vector2(backgroundSpeed * Time.deltaTime, 0f);
    }
}
