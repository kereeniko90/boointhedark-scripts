using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{   
    [SerializeField] int numberOfEnemies = 100;
    [SerializeField] private Terrain terrain; 
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform enemyParent;
    public GameObject enemyPrefab;  
    public float spawnRadius = 10f;
    private float coolDownTimerMin = 10f;
    private float coolDownTimerMax = 60f;
    private float cooldownTimer = 0f;

    private float spawnTime = 10f;

     
    
    
    public float checkRadius = 30f; 

     

    private void Start()
    {
        terrain = Terrain.activeTerrain;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    private void Update() {
        
        

        if (enemyParent.childCount < numberOfEnemies) {

            cooldownTimer += Time.deltaTime;

            if (cooldownTimer > spawnTime) {
                SpawnEnemy();
            cooldownTimer = 0;
            spawnTime = Random.Range(coolDownTimerMin, coolDownTimerMax);
            }
            
        }

        

            
    }

    

    private void SpawnEnemy()
    {   
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        
        
            
            if (!IsNearTree(spawnPoint.position) && IsValidNavMeshPosition(spawnPoint.position))
            {
                
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                enemy.transform.SetParent(enemyParent);
                
                
                
            }
            
        

        
    }

    private bool IsNearTree(Vector3 spawnPoint)
    {
        TerrainData terrainData = terrain.terrainData;

        foreach (TreeInstance tree in terrainData.treeInstances)
        {
            Vector3 treeWorldPos = Vector3.Scale(tree.position, terrainData.size) + terrain.transform.position;
            float distanceToTree = Vector3.Distance(spawnPoint, treeWorldPos);
            float treeRadius = 2.0f;  

            if (distanceToTree < treeRadius)
            {
                return true;  
            }
        }

        return false;
    }

    private bool IsValidNavMeshPosition(Vector3 position)
    {
        NavMeshHit hit;
        
        return NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas);
    }
}
