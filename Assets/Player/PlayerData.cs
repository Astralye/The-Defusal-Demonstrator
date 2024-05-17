using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class PlayerData : MonoBehaviour
{

    private List<Item> playerInventory;

    private Weapons itemOut;

    public static bool disarmed;
    public static bool enablePlayerMovement;
    public static bool getItem;

    [SerializeField] private Rig Aimrig;
    [SerializeField] private Animator animator;

    [SerializeField] private PlayerInputValues _input;

    public ParticleSystem Bullethole;
    public ParticleSystem muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = new List<Item>();

        disarmed = false;
        enablePlayerMovement = true;
        getItem = false;
    }

    public List<Item> getPlayerInventory()
    {
        return playerInventory;
    }

    public void setInventory(List<Item> data)
    {
        playerInventory = data;
    }

    private void Update()
    {
        checkAnimationFlags();

    }
    private void checkAnimationFlags()
    {

        foreach (Item item in playerInventory)
        {

            switch (item.name)
            {
                case "Pistol":
                    {
                        animator.SetBool("holdPistol", true);
                        Aimrig.weight = 1.0f;
                        
                        if (_input.aim)
                        {
                            animator.SetBool("Aiming", true);
                            GameObject.Find("HandAAim").GetComponent<MultiAimConstraint>().weight = 1;
                        }
                        else
                        {
                            animator.SetBool("Aiming", false);
                            GameObject.Find("HandAAim").GetComponent<MultiAimConstraint>().weight = 0;
                        }
                        
                        // set a timer for when the player can shoot again.
                        if (_input.attack)
                        {
                            shoot(item, GameObject.Find("Bullet Spawn").transform);

                            // change this later
                            _input.attack = false;
                            animator.SetBool("Attack", true);
                        }
                        else
                        {
                            animator.SetBool("Attack", false);
                        }

                        break;
                    }
            }
        }
    }

    private void shoot(Item item, Transform bulletSpawn)
    {

        muzzleFlash.Play();

        RaycastHit hit;
        Weapons weapon = (Weapons)item;
        float damage = weapon.getDamage();

        if (Physics.Raycast(bulletSpawn.position, bulletSpawn.forward, out hit , weapon.getAttackRange()))
        {
            var hitbox = hit.collider.GetComponent<Hitbox>();
            if(hitbox != null)
            {
                hitbox.OnRaycastHit(damage, bulletSpawn.forward);
            }
        }
    }
}
