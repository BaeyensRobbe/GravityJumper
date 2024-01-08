using Unity.VisualScripting;
using UnityEngine;

public class PlayerIndicatorController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject Player;
    public LayerMask groundLayer;
    public float lineWidth = 10f;

    private float playerHeight;

    private GameObject currentGroundObject;
    private float leftBoundOfCurrentGround;
    private float rightBoundOfCurrentGround;
    private float topBoundOfCurrentGround;


    void Start()
    {
        playerHeight = Player.GetComponent<SpriteRenderer>().size.y;
    }

    void Update()
    {
        if (Player.transform.position.x > rightBoundOfCurrentGround)
        {
            currentGroundObject = null;
            lineRenderer.enabled = false;
        }
        // Raycast downward to detect ground


        float lineStart = 0f;
        float lineEnd = 0f;

        // Calculate endpoint object
        if (currentGroundObject != null)
        {
/*            RaycastHit2D hitLeft = Physics2D.Raycast(Player.transform.position - new Vector3(0.5f, 0, 0), Vector2.down, Mathf.Infinity, groundLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(Player.transform.position + new Vector3(0.5f, 0, 0), Vector2.down, Mathf.Infinity, groundLayer);*/

            lineStart = Player.transform.position.x - 2f;
            lineEnd = Player.transform.position.x + 4f;

            if (leftBoundOfCurrentGround > lineStart)
            {
                lineStart = leftBoundOfCurrentGround;
            }
            if (rightBoundOfCurrentGround < lineEnd)
            {
                lineEnd = rightBoundOfCurrentGround;
            }


            // Calculate angle of the ground object
            float angle = currentGroundObject.transform.rotation.eulerAngles.z;

            if (angle != 0f)
            {
                lineRenderer.enabled = false;
            }

            // Calculate Y position
            float y = currentGroundObject.transform.position.y + (currentGroundObject.GetComponent<SpriteRenderer>().bounds.size.y / 2f);

            // Calculate X and Y offsets using trigonometry
            float xOffset = Mathf.Cos(Mathf.Deg2Rad * angle) * lineStart;
            float yOffset = Mathf.Sin(Mathf.Deg2Rad * angle) * lineStart;

            Vector3 startPos = new Vector3(xOffset, y + yOffset, 0);

            xOffset = Mathf.Cos(Mathf.Deg2Rad * angle) * lineEnd;
            yOffset = Mathf.Sin(Mathf.Deg2Rad * angle) * lineEnd;

            Vector3 endPos = new Vector3(xOffset, y + yOffset, 0);

            // Adjust line width
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;

            // Set line positions
            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
        }

        else
        {
            RaycastHit2D bottomHit = Physics2D.Raycast(Player.transform.position + new Vector3(0.5f, 0, 0), Vector2.down, 10f, groundLayer);
            if (bottomHit)
            {
                currentGroundObject = bottomHit.collider.gameObject;
                CalculateGroundParameters(currentGroundObject);
                lineRenderer.enabled = true;

            }
        }
       
        

        
    }

    public void HideLine()
    {
        lineRenderer.enabled = false;
    }

    // Function to show the LineRenderer visibility
    public void ShowLine(Collision2D collision)
    {
        currentGroundObject = collision.gameObject;
        CalculateGroundParameters(currentGroundObject);
        lineRenderer.enabled = true;
    }

    private void CalculateGroundParameters(GameObject currentObject)
    {
        float groundWidth = currentObject.GetComponent<SpriteRenderer>().bounds.size.x;
        float groundHeight = currentObject.GetComponent<SpriteRenderer>().bounds.size.y;
        leftBoundOfCurrentGround = currentObject.transform.position.x - (groundWidth / 2f);
        rightBoundOfCurrentGround = currentObject.transform.position.x + (groundWidth / 2f);
        topBoundOfCurrentGround = currentObject.transform.position.y + (groundHeight / 2f);
    }
}
