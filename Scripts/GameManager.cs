using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager globalGameManager = null;
    public HeroBehaviour heroBehaviour = null;  // 必须在编辑器中设置
    public TMP_Text echoText = null;

    [SerializeField]
    private int maxEnemyCount = 10;
    [SerializeField]
    private float spawnRangeX = 15f;
    [SerializeField]
    private float spawnRangeY = 10f;

    private int currentEnemyCount = 0;
    private int destroyedEnemyCount = 0;
    private int eggCount = 0; // 当前蛋的数量
    private int heroCollideCount = 0; // 英雄碰撞次数

    void Awake()
    {
        // 确保 globalGameManager 在 Awake 中初始化，这样其他脚本的 Start/Update 就能访问到
        globalGameManager = this;
    }

    void Start()
    {
        // 检查必要组件是否已赋值
        if (echoText == null)
        {
            Debug.LogError("EchoText 未赋值！请在 Inspector 中拖拽 UI 文本组件。");
            return;
        }
        
        if (heroBehaviour == null)
        {
            Debug.LogError("HeroBehaviour 未赋值！请在 Inspector 中拖拽主角对象。");
            return;
        }

        // 生成初始敌人
        for (int index = 0; index < maxEnemyCount; index++)
        {
            SpawnOneEnemy();
        }
    }

    // 每帧调用一次
    void Update()
    {
        HandleInput();
        MaintainEnemyCount();
        UpdateUI();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    private void MaintainEnemyCount()
    {
        if (currentEnemyCount < maxEnemyCount)
        {
            SpawnOneEnemy();
        }
    }

    private void UpdateUI()
    {
        // 添加空值检查，防止空引用异常
        if (echoText == null || heroBehaviour == null) return;

        echoText.text = "Control Mode: " +
            (heroBehaviour.controlMode == HeroBehaviour.ControlMode.Mouse ? "Mouse " : "Keyboard ") + "\n" +
            "Hero Collides: " + heroCollideCount + "\n " +
            "Eggs: " + eggCount + "\n" +
            "Total Enemies: " + currentEnemyCount + "\n " +
            "Destroyed Enemies: " + destroyedEnemyCount;
    }

    // 生成一个敌人
    public void SpawnOneEnemy()
    {
        GameObject enemyObject = Instantiate(Resources.Load("Prefab/Enemy") as GameObject);
        float randomPositionX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomPositionY = Random.Range(-spawnRangeY, spawnRangeY);
        enemyObject.transform.localPosition = new Vector3(randomPositionX, randomPositionY, 0f);
        enemyObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        currentEnemyCount++;
    }

    // 敌人被销毁
    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
        destroyedEnemyCount++;
    }

    // 蛋生成
    public void OnEggCreate()
    {
        eggCount++;
    }

    // 蛋销毁
    public void OnEggDestroy()
    {
        eggCount--;
    }

    // 英雄碰撞
    public void OnHeroCollide()
    {
        heroCollideCount++;
    }
}