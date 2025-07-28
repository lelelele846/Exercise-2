using UnityEngine;

public class HeroBehaviour : MonoBehaviour
{
    public enum ControlMode { Mouse, Keyboard }
    public ControlMode controlMode = ControlMode.Mouse;

    private float currentSpeed = 20f;
    private float turnSpeed = 45f;

    public GameObject eggPrefab;
    private float lastShootTime = -999f;
    private float shootCooldown = 0.2f;

    void Start()
    {
        transform.localScale = new Vector3(5f, 5f, 1f); // 保证Hero为5x5
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GameManager.globalGameManager.heroBehaviour.controlMode = controlMode == ControlMode.Mouse ? ControlMode.Keyboard : ControlMode.Mouse;
            controlMode = controlMode == ControlMode.Mouse ? ControlMode.Keyboard : ControlMode.Mouse;
        }

        if (controlMode == ControlMode.Mouse)
        {
            MouseControl();
        }
        else
        {
            KeyboardControl();
        }

        if (Input.GetKey(KeyCode.Space) && Time.time - lastShootTime >= shootCooldown)
        {
            GameObject egg = Instantiate(eggPrefab, transform.position, transform.rotation);
            lastShootTime = Time.time;
            GameManager.globalGameManager.OnEggCreate();
        }
    }

    void MouseControl()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }

    void KeyboardControl()
    {
        float vertical = Input.GetAxis("Vertical");  // W=1, S=-1
        currentSpeed += vertical * 40f * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -40f, 40f);

        float horizontal = Input.GetAxis("Horizontal");  // A=-1, D=1
        transform.Rotate(0, 0, -horizontal * turnSpeed * Time.deltaTime);

        transform.position += transform.up * currentSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            GameManager.globalGameManager.OnEnemyDestroyed();
            GameManager.globalGameManager.OnHeroCollide();
        }
    }
} 