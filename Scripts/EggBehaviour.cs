using UnityEngine;

public class EggBehaviour : MonoBehaviour
{
    public float speed = 40f;
    private float worldBoundY;
    private float worldBoundX;
    private bool hasHit = false;

    private void Start()
    {
        worldBoundY = Camera.main.orthographicSize;
        worldBoundX = worldBoundY * Camera.main.aspect;
        transform.localScale = Vector3.one; // 保证蛋为1x1
        GameManager.globalGameManager.OnEggCreate();
    }
    void Update()
    {
        if (hasHit) return;
        transform.position += transform.up * speed * Time.deltaTime;

        Vector3 position = transform.position;
        if (Mathf.Abs(position.x) > worldBoundX || Mathf.Abs(position.y) > worldBoundY)
        {
            Destroy(gameObject);
            GameManager.globalGameManager.OnEggDestroy();
        }
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (hasHit) return;
        if (!otherCollider.CompareTag("Enemy")) return;

        hasHit = true;
        EnemyBehavior enemyBehaviour = otherCollider.GetComponent<EnemyBehavior>();
        if (enemyBehaviour != null)
        {
            enemyBehaviour.HitByEgg();
        }
        Destroy(gameObject);
        GameManager.globalGameManager.OnEggDestroy();
    }
} 