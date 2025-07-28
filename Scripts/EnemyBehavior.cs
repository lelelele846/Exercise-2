using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private int hitCount = 0;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private const int maxHits = 4;
    private const float alphaLossPerHit = 0.2f;

    void Start()
    {
        // 设置敌人大小为5x5
        transform.localScale = new Vector3(5f, 5f, 1f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // 被蛋击中时调用
    public void HitByEgg()
    {
        hitCount++;
        // 逐渐失去力量（alpha透明度）
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = Mathf.Max(0, color.a - alphaLossPerHit);
            spriteRenderer.color = color;
        }
        // 第4次碰撞时销毁
        if (hitCount >= maxHits)
        {
            GameManager.globalGameManager.OnEnemyDestroyed();
            Destroy(gameObject);
        }
    }

    // Hero碰撞时直接销毁
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Hero"))
        {
            GameManager.globalGameManager.OnEnemyDestroyed();
            GameManager.globalGameManager.OnHeroCollide();
            Destroy(gameObject);
        }
        else if (otherCollider.CompareTag("Egg"))
        {
            HitByEgg();
        }
    }
}