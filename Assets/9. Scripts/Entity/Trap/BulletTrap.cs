using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrap : Trap
{
	[SerializeField] private GameObject bulletPrefab;
	[SerializeField] private float bulletSpeed;

	protected override void TrapOn()
	{

		var bullet = Instantiate(bulletPrefab);
		bullet.transform.position = transform.position;

		Vector3 dir = (player.transform.position - bullet.transform.position).normalized;
		bullet.GetComponent<Rigidbody>().AddForce(dir * bulletSpeed, ForceMode.Impulse);
		GameManager.Instance.SetTimer(() => Destroy(bullet), 10.0f);
		
	}


}
