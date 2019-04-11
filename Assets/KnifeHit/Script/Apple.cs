/**
 * Apple.cs
 * Created by: #PaulCornel#
 * Created on: #CREATIONDATE# (dd/mm/yy)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour {

	public ParticleSystem splatApple;
	public SpriteRenderer Sprite;
	public AudioClip appleHitSfx;

	// Use this for initialization
	public Rigidbody2D rb;
	void Start () {
		rb = GetComponentInChildren<Rigidbody2D> ();    
		rb.isKinematic = true;
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Knife")) {
			//if (!other.gameObject.GetComponent<Knife> ().isHitted) {
				SoundManager.instance.PlaySingle (appleHitSfx);
				GameManager.Apple++;
				transform.parent = null;
				GetComponent<CircleCollider2D> ().enabled = false;
				Sprite.enabled = false;
				splatApple.Play ();
				Destroy (gameObject, 3f);
			//}
		}
	}
}


