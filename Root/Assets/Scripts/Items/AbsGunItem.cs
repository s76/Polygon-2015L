using UnityEngine;
using System.Collections;

public abstract class AbsGunItem : AbsItem
{
	protected int damage;
	protected float interval;
	protected float range;

	public KeyCode fire;

	protected bool armed;

	protected float timer;
	protected Ray shootRay;
	protected RaycastHit shootHit;
	protected int shootableMask;
	protected ParticleSystem gunParticles;
	protected LineRenderer gunLine;
	protected AudioSource gunAudio;
	protected Light gunLight;
	protected float effectsDisplayTime = 0.2f;

	protected AbsGunItem(string name, bool isDoubleHanded, int damage, float interval, float range):
		base(name, isDoubleHanded)	{
		this.damage = damage;
		this.interval = interval;
		this.range = range;
		armed = false;
	}
	
	void Start()
	{
	}
	
	void Awake ()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent <LineRenderer> ();
		//gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}

	void Update ()
	{

	}
	
	
	public void DisableEffects ()
	{
		gunLine.enabled = false;
		gunLight.enabled = false;
	}

	public override void OnBeingUsed ()
	{
		CountAndShoot ();
	}
	
	public override void OnBeingNotUsed ()
	{
	}

	protected void CountAndShoot ()
	{
		if(armed){
			timer += Time.deltaTime;
			
			if (timer >= interval && Time.timeScale != 0) {
				Shoot ();
			}
			
			if (timer >= interval * effectsDisplayTime) {
				DisableEffects ();
			}
		}
	}

	private void Shoot ()
	{
		Debug.Log ("Fire!");

		timer = 0f;
		
		//gunAudio.Play ();
		
		gunLight.enabled = true;
		
		gunParticles.Stop ();
		gunParticles.Play ();
		
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		
		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;
		
		if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
		{
			Debug.Log("Hostile inbound!");
			AbsHostile hostile = shootHit.collider.GetComponent<AbsHostile>();
			if(hostile != null)
			{
				hostile.TakeDamage (damage);
			}
			AbsLoot loot = shootHit.collider.GetComponent<AbsLoot>();
			Debug.Log("Loot is: "+loot);
			if(loot != null)
			{
				loot.Push (shootRay.direction);
			}

			gunLine.SetPosition (1, shootHit.point);
		}
		else
		{
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}


}
