using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEditor.SearchService;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float minPosition = -10f;
    public float groundDetectingDisctance = 0.4f;
    public float swipeDownThreshold = -10f; // Adjust the threshold as needed
    public CinemachineVirtualCamera cinemachine;
    public float cameraTransitionDuration;
    public float bounceForce = 5f;

    public GameObject playerDiesParticlePrefab;
    public GameObject endPopup;
    public GameObject finish;
    public GameObject pauseButton;
    public GameObject panel;

    private Rigidbody2D rb;
    private bool isGrounded;

    private bool canCompress;
    private bool isScalingDown;
    private bool isTouching;
    private bool canWalljump;
    private bool isMovingLeft;
    private bool playerParticleInstantiated;
    private bool canSpeedUp;
    private bool canJump = true;
    private bool playerDied = false;
    private GameObject backgroundPlayer;

    PlayerIndicatorController playerIndicatorController;
    CoinHandler coinHandler;
    AudioManager audioManager;


    // raycast parameters
    private float halfPlayerWidth;
    private Vector2 rightSideOrigin;
    private Vector2 leftSideOrigin;
    private Vector2 rightSideBottomOrigin;

    // Character parameters
    [System.Serializable]
    public struct Colors
    {
        public Color color;
        public string name;
    }


    public Colors[] colors;
    public Sprite[] sprites;

    private int selectedSprite;
    private string selectedColor;


    void Start()
    {
        // get sprite and color of selected character
        selectedSprite = PlayerPrefs.GetInt("CurrentSprite", 0);
        selectedColor = PlayerPrefs.GetString("CurrentColor", "Blue");

        // set sprite, color and trailColor
        transform.GetComponent<SpriteRenderer>().sprite = sprites[selectedSprite];
        Color colorValue = colors.FirstOrDefault(color => color.name == selectedColor).color;
        TrailRenderer playerTrail = transform.GetChild(0).transform.GetComponent<TrailRenderer>();
        playerTrail.startColor = colorValue;
        playerTrail.endColor = colorValue;
        transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = colorValue;


        rb = GetComponent<Rigidbody2D>();
        isGrounded = true;
        playerParticleInstantiated = false;
        Time.timeScale = 1.0f;

        halfPlayerWidth = transform.localScale.x / 2f;


        playerIndicatorController = GameObject.Find("PlayerIndicator").GetComponent<PlayerIndicatorController>();
        coinHandler = GameObject.Find("Coins").GetComponent<CoinHandler>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    void Update()
    {
        MovePlayer();
      
        if (HandleInput())
        { 
            if (isGrounded && !canCompress && canJump)
            {
                Jump();
            } else
            {
                Debug.Log("Cannot jump here");
                Debug.Log(isGrounded);
                Debug.Log(canJump);
                Debug.Log(canJump);
            }
        }

        if (transform.position.y < minPosition)
        {
            PlayerDies();
        }
    }

    bool HandleInput()
    {

        if (Input.GetMouseButtonDown(0) && Time.timeScale == 1f)
        {
            if ((Input.mousePosition.y < Screen.height * 0.75f))
            {
                isTouching = true;
                return true;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            isTouching = false;
        }

        if (isTouching)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void MovePlayer()
    {
        // Automatically move to the right
        if (isMovingLeft)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        } else
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
       
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isGrounded = false; // Set to false immediately to prevent multiple jumps
    }

/*    IEnumerator ActivateSpeedUp()
    {
        moveSpeed = moveSpeed * 1.5f;
        yield return new WaitForSeconds(10f);
        moveSpeed = moveSpeed / 1.5f;
    }*/

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            Time.timeScale = 0f;
            Debug.Log("Function runs");
            audioManager.StopPlayingBackgroundMusic();
            audioManager.PlaySoundEffect("Finish");
            ShowPopupAfterSeconds(1f);

            string activeScene = SceneManager.GetActiveScene().name;
            ActivateEndPopup(activeScene);
            string floatname = activeScene + "_Progress";
            string lastTryString = activeScene + "_LastTry";
            PlayerPrefs.SetInt(activeScene, 1);
            PlayerPrefs.SetFloat(floatname, 1f);
            PlayerPrefs.SetFloat(lastTryString, 1f);

        }
        else if (IsGround(collision))
        {
            isGrounded = true;
            canJump = true;
        }


        else if (IsObstacle(collision))
        {
            // Game ends
            PlayerDies();
        }
        
        if (collision.transform.CompareTag("Star"))
        {
            PlayerDies();
        }
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (checkCollisionExit(collision) == "Ground")
        {
            isGrounded = false;
/*            playerIndicatorController.HideLine();*/
        }

        if (collision.transform.parent.CompareTag("Wall"))
        {
            canWalljump = false;
        }
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Compress"))
        {
            canCompress = true;
        } else if (collision.CompareTag("SpeedUp"))
        {
            /*StartCoroutine(ActivateSpeedUp());*/
            moveSpeed = moveSpeed * 1.5f;
            audioManager.PlaySoundEffect("SpeedUp");
        } else if (collision.CompareTag("SpeedDown"))
        {
            moveSpeed = moveSpeed / 1.5f;
            audioManager.PlaySoundEffect("SpeedDown");
        } 
        else if (collision.CompareTag("JumpBoost"))
        {
            audioManager.PlaySoundEffect("JumpBoostUp");
            jumpForce = jumpForce * 1.5f;
        } else if (collision.CompareTag("JumpBoostDown"))
        {
            audioManager.PlaySoundEffect("JumpBoostDown");
            jumpForce = jumpForce / 1.5f;
        }
        if (collision.transform.CompareTag("BouncePad"))
        {
            audioManager.PlaySoundEffect("BouncePad");
            /*rb.AddForce(Vector3.up * bounceForce, (ForceMode2D)ForceMode.VelocityChange);*/
            if (collision.name == "MegaBounce")
            {
                rb.velocity = new Vector2(rb.velocity.x, bounceForce + 15);
                canJump = false;
            } else
            {
                rb.velocity = new Vector2(rb.velocity.x, bounceForce);
                canJump = false;
            }

        }
        if (collision.CompareTag("Coin"))
        {
            // Coin SFX
            audioManager.PlaySoundEffect("Coin");

            collision.gameObject.SetActive(false);
            // Collect coin animation
            string coin = collision.transform.name;
            string sceneName = SceneManager.GetActiveScene().name;
            string coinName = sceneName + "_" + coin;
            Debug.Log(coinName);
            if (PlayerPrefs.GetInt(coinName, 0) == 1)
            {
                // don't know 
            } else
            {
                PlayerPrefs.SetInt(coinName, 1);
                string currentCollectedCoinsName = sceneName + "_CollectedCoins";
                int currentCollectedCoins = PlayerPrefs.GetInt(currentCollectedCoinsName, 0);
                currentCollectedCoins++;
                PlayerPrefs.SetInt(currentCollectedCoinsName, currentCollectedCoins);
                int currentCoins = PlayerPrefs.GetInt("Coins", 0);
                int newCoins = currentCoins + 1;
                PlayerPrefs.SetInt("Coins", newCoins);
                coinHandler.UpdateCoinAmount();
            }

            
        }

        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Compress"))
        {
            canCompress = false;
        } else if (collision.CompareTag("SpeedUp"))
        {
            canSpeedUp = false;
        }
    }


    bool IsGround(Collision2D collision)
    {
        if (collision.transform.parent.CompareTag("Ground"))
        {
            RaycastHit2D bottomRaycastDown = Physics2D.Raycast(new Vector2(transform.position.x + halfPlayerWidth, transform.position.y - halfPlayerWidth), Vector2.down, 1f, LayerMask.GetMask("Ground"));
            Debug.DrawRay(new Vector2(transform.position.x + halfPlayerWidth, transform.position.y - halfPlayerWidth), Vector2.down * 1f, Color.red, 1f);

            if (bottomRaycastDown.collider == null)
            {
                PlayerDies();
            }
            
            if (Physics2D.Raycast(rb.transform.position, Vector2.down, 2f))
            {
                playerIndicatorController.ShowLine(collision);
                return true;
            }
            
            

        }
        return false;

        // Your original check for ground
        
    }

    string checkCollisionExit(Collision2D collision)
    {
        if (collision.transform.parent.CompareTag("Ground"))
        {
            return "Ground";
        }
        else return null;
    }

    bool IsObstacle(Collision2D collision)
    {
        // Assuming obstacles have a specific tag (e.g., "Obstacle")
        return collision.transform.parent.CompareTag("Obstacles");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    IEnumerator ChangeCameraOffsetSmoothly()
    {
        // Get the current offset
        Vector3 startOffset = cinemachine.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;

        // Target offset with reversed x component
        Vector3 targetOffset = new Vector3(-startOffset.x, startOffset.y, startOffset.z);

        // Time elapsed
        float elapsed = 0f;

        while (elapsed < cameraTransitionDuration)
        {
            // Interpolate between start and target offset
            Vector3 newOffset = Vector3.Lerp(startOffset, targetOffset, elapsed / cameraTransitionDuration);

            // Set the new offset
            cinemachine.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = newOffset;

            // Increment elapsed time
            elapsed += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the final offset is exactly the target offset
        cinemachine.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = targetOffset;
    }

    void PlayerDies()
    {
        if (playerDied == false)
        {
            playerDied = true;
            // Play SFX and stop playing Background Music
            audioManager.StopPlayingBackgroundMusic();
            audioManager.PlaySoundEffect("PlayerDies");


            // set progress
            string currentScene = SceneManager.GetActiveScene().name;
            string playerPrefsName = currentScene + "_Progress";
            string lastTryName = currentScene + "_LastTry";
            float currentProgress = PlayerPrefs.GetFloat(playerPrefsName);
            float newProgress = CalculateProgress();
            PlayerPrefs.SetFloat(lastTryName, newProgress);
            if (currentProgress < newProgress)
            {
                PlayerPrefs.SetFloat(playerPrefsName, newProgress);
            }

            // set opacity to 0 and deactivate background + trail
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color currentColor = spriteRenderer.color;
            currentColor.a = 0f;
            rb.gameObject.GetComponent<SpriteRenderer>().color = currentColor;
            moveSpeed = 0f;
            rb.transform.GetChild(0).transform.gameObject.SetActive(false);
            rb.transform.GetChild(1).transform.gameObject.SetActive(false);
            cinemachine.enabled = false;

            if (!playerParticleInstantiated)
            {
                playerParticleInstantiated = true;
                Instantiate(playerDiesParticlePrefab, rb.transform.position, Quaternion.identity);
                
                /*           StartCoroutine(ReloadSceneAfterSeconds(1f));*/
            }

            StartCoroutine(ShowPopupAfterSeconds(1f));
        }
        


    }

    private IEnumerator ShowPopupAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        endPopup.SetActive(true);
        panel.SetActive(true);
        pauseButton.SetActive(false);

        string scene = SceneManager.GetActiveScene().name;
        Color color = Color.blue;
        switch (scene)
        {
            case "Level2":
                color = Color.red;
                break;
            case "Level3":
                ColorUtility.TryParseHtmlString("EC00FF", out color);
                break;


        }
        endPopup.transform.GetChild(0).GetChild(0).transform.GetComponent<Image>().color = color;

    }

    private IEnumerator ReloadSceneAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ReloadScene();
    }

    private float CalculateProgress()
    {
        float progress = 0f;
        float position = rb.transform.position.x;
        float endpoint = finish.transform.position.x;
        progress = (position / endpoint);

        return progress;
    }

    public void ToggleCanJump()
    {
        canJump = !canJump;
    }

    private void ActivateEndPopup(string scene)
    {
        endPopup.SetActive(true);
        Color color = Color.blue;
        switch (scene)
        {
            case "Level2":
                color = Color.red;
                break;
            case "Level3":
                ColorUtility.TryParseHtmlString("EC00FF", out color);
                break;


        }
        endPopup.transform.GetChild(0).GetChild(0).transform.GetComponent<Image>().color = color;
    }

    


}

