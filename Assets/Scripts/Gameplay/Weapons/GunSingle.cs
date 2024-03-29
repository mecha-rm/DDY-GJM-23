using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDY_GJM_23
{
    // A gun that fires a single shot.
    public class GunSingle : Weapon
    {
        [Header("Gun (Single)")]

        // The prefab for the bullet.
        // Gun power is multiplied by the bullet's power.
        public Bullet bulletPrefab;

        // If 'true', the bullet's power is multiplied by the gun's power.
        public bool multBulletPowerByGunPower = true;

        // TODO: set up bullet pool.
        // The pool for the bullets.
        // public Queue<Bullet> bulletPool;

        // // Awake is called when the script is being loaded
        // protected override void Awake()
        // {
        //     base.Awake();
        // }
        // 
        // // Start is called before the first frame update
        // protected override void Start()
        // {
        //     base.Start();
        // }

        // Use the weapon.
        public override void UseWeapon()
        {
            // Weapon can't be used.
            if (!IsUsable())
                return;

            // TODO: use bullet pool.

            // Generates a new bullet.
            Bullet newBullet = Instantiate(bulletPrefab);

            // Multiplies the power of the bullet by the gun's power.
            newBullet.power *= power;

            // Give base position.
            newBullet.transform.position = owner.transform.position;

            // TODO: maybe change how bullet direction is set.
            // TODO: calculate the rotation properly for where the player should be facing.

            // Set bullet's rotation for its direction.
            // TODO: is this working?

            // Rotates the bullet, and gets its direction.
            newBullet.transform.eulerAngles = new Vector3(0, 0, owner.GetFacingDirectionAsRotation());
            Vector2 bulletRight = newBullet.transform.right;
            
            // Sets the rotation, and sets it using the function.
            newBullet.transform.rotation = Quaternion.identity;
            newBullet.SetBulletDirection(bulletRight);

            // Set the bullet to go its max speed.
            newBullet.SetBulletToMaxSpeed();

            // Called when the weapon was used.
            OnUseWeapon(1);


            // SFX
            // Grabs the game audio and plays the shot SFX.
            GameplayAudio gameAudio = owner.gameManager.gameAudio;
            gameAudio.PlayPlayerShotSfx();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}