using UnityEngine;
using UnityEngine.AI;

public class ConsumableSpawner : MonoBehaviour
{   
    [SerializeField] int numberOfConsumables = 100;
    
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private Terrain terrain; 
    [SerializeField] private Transform consumableParent;

    [SerializeField] private GameObject[] consumables;
    
    public float spawnRadius = 5f;
    
    private float coolDownTimerMin = 10f;
    private float coolDownTimerMax = 60f;
    private float cooldownTimer = 0f;

    private float spawnTime = 10f;
    
    public float checkRadius = 30f; 

     

    private void Start()
    {
        terrain = Terrain.activeTerrain;

        for (int i = 0; i < numberOfConsumables; i++)
        {
            SpawnConsumable();
        }
    }

    private void Update() {
        
        

        if (consumableParent.childCount < numberOfConsumables) {
            
            cooldownTimer += Time.deltaTime;

            if (cooldownTimer > spawnTime) {
                    SpawnConsumable();
                    cooldownTimer = 0;
                    spawnTime = Random.Range(coolDownTimerMin, coolDownTimerMax);
                    
            }

            
        }
    }

    

    private void SpawnConsumable()
    {   
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        
            Vector3 randomPosition = GetRandomPointWithinRadius(spawnPoint.position, spawnRadius);
            
            if (!IsNearTree(randomPosition))
            {
                int randomConsumable = Random.Range(0, consumables.Length);
                GameObject consumable = Instantiate(consumables[randomConsumable], randomPosition, Quaternion.identity);
                consumable.transform.SetParent(consumableParent);
                
                
                
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

    private Vector3 GetRandomPointWithinRadius(Vector3 origin, float radius)
    {
        // Generate random point in a circle on the XZ plane
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        Vector3 spawnPosition = new Vector3(origin.x + randomPoint.x, origin.y + 0.5f, origin.z + randomPoint.y);

        return spawnPosition;
    }

    
}
