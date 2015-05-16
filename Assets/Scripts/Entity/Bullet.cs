using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	float age = 0;
	public float maxAge = 2f;
	public float power = 1f;
	bool alive = true;
	// Use this for initialization
	void Start () {
		//maxAge *= Random.Range(.9f,1.1f);
		var sprite = GetComponent("SpriteRenderer") as SpriteRenderer;
		sprite.color = Color.Lerp( Color.white,Color.red, (power-1)/3f);
	}
	
	// Update is called once per frame
	void Update () {
		age += Time.deltaTime;
		transform.localScale = transform.localScale;
		if(age >= maxAge && alive)
		{
			Die();
		}	
	}

	void Die()
	{
		StartCoroutine("DeathAnimation");
	}

	IEnumerator DeathAnimation()
	{
		alive = false;
		var pos = transform.position;
		transform.GetComponent<Rigidbody2D>().velocity *= 0f;
		Ground.ground.AddMaterial("metal",Mathf.RoundToInt(power),new Vector2(pos.x,pos.y));
		yield return new WaitForSeconds(0.3f);
		Destroy (this.gameObject);	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Enemy enemy = col.gameObject.GetComponent("Enemy") as Enemy;
		if (enemy != null) 
		{
			enemy.Hit(power);
			Die ();
		}
	}

}
