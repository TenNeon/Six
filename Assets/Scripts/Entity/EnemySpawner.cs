using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public float spawnRate = 2f;
	public Transform enemyPrefab;
	public float spawnDistance = 5f;
	public float enemySpeed = 0.5f;
	public float enemyMaxAge = 30f;
	public float enemyArmor = 0.5f;
	float lastSpawn = 0f;
	// Use this for initialization
	void Start () {

	}

	void Update()
	{
		if(Time.time >= lastSpawn + 1/spawnRate)
		{
			SpawnEnemy();
			lastSpawn = Time.time;
		}
	}

	void SpawnEnemy()
	{
		var ouc = OnUnitCircle()*spawnDistance;
		var enemyPos = transform.position + new Vector3(ouc.x,ouc.y,0);
		var enemyTransform = Instantiate(enemyPrefab, enemyPos, transform.rotation) as Transform; 
		var enemy = enemyTransform.GetComponent("Enemy") as Enemy;
		enemy.hitPoints = Random.Range(0.9f,3f);
		enemy.hitPoints += Random.Range(0f,2f);
		enemy.armor = enemyArmor/3;
		enemy.armor += Random.Range(0f,enemyArmor/3);
		enemy.armor += Random.Range(0f,enemyArmor/3);
		enemy.speed = enemySpeed * Random.Range(0.9f,1.1f);
		enemy.maxAge = enemyMaxAge;

		if(Random.Range(0,60) == 0)
		{
			enemy.hitPoints *= 5;
			enemy.speed *= (0.1f);
		}
	}

	Vector2 OnUnitCircle()
	{
		//generate a point in the unit circle
		Vector2 initialPoint = Random.insideUnitCircle;
		if(initialPoint == Vector2.zero)
		{
			// on the freak chance that the random vector is zero,
			// decide that it is actually one
			return Vector2.one;
		}
		//extend point to edge
		return initialPoint.normalized;
	}
}
