using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 2f;

    void Update()
    {
        // Get the current position of the background
        Vector3 currentPosition = transform.position;

        // Calculate the new position based on the scroll speed
        float newPosition = currentPosition.x - scrollSpeed * Time.deltaTime;

        // Set the new position
        transform.position = new Vector3(newPosition, currentPosition.y, currentPosition.z);
    }
}
