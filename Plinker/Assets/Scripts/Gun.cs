using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Author: Sebastian Olko

    Date: Dec 2020

    Description: Control behavior and effects when the player shoots/aims the gun
*/
public class Gun : MonoBehaviour
{

    // Camera location for raycasts
    public Transform camera;

    // Audio for gun 
    public AudioSource GunShot;
    public AudioSource ReloadSound;

    // Game controller variables for tracking score
    public GameObject GameCont;
    private GameController GameController;

    // Gun cosmetic additions
    public ParticleSystem muzzleFlash;
    public GameObject reticle;

    // Target point tracking layer masks
    public LayerMask Target, HeadTarget, Hostage;

    TargetController TargetHit;

    public Animator animator;   

    // Gun attribute Variables
    private int MaxAmmo = 10;
    public int CurrentAmmo = 10;
    private float ReloadTime = 1.5f;
    private float RecoilTime = 0.35f;
    private bool IsReloading = false;
    private bool IsShooting = false;

    // aim down sights
    private bool ADS = false;
    private float AimDownSightSpeed = 1.7f;

    // Raycast max distance
    private float maxDistance = 100f;

    private void Awake() 
    {
        GameController = GameCont.GetComponent<GameController>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Simple state machine for Gun firing/using conditions
        if (IsReloading || IsShooting)
            return;

        if (Input.GetButtonDown("Reload"))
        {
            StartCoroutine(Reload());
            return;
        }
        if (Input.GetMouseButtonDown(0) && CurrentAmmo > 0)
        {
            StartCoroutine(Shoot());
        }
        if (Input.GetMouseButtonDown(1) && ADS == false)
        {
            StartCoroutine(StartAimDownSights());
        }
        if (Input.GetMouseButtonUp(1) && ADS == true)
        {
            StartCoroutine(StopAimDownSights());
        }
    }

    // Coroutine for shooting that checks target hits, starts animations and particle effects for firing
    IEnumerator Shoot()
    {
        // set shooting to true so no other input is taken until gun finishes firing
        IsShooting = true;

        // Play particle effect
        muzzleFlash.Play();
        // play gunshot sound
        GunShot.Play();
        // adjust ammo for gun
        CurrentAmmo--;

        // Depending of whether player is aiming down sights, play different animations
        if (ADS)
            animator.SetBool("ADSRecoil", true);
        else
            animator.SetBool("Recoil", true);


        RaycastHit hit;
        // checks if raycast hit hostage target and runs hostage method to adjust points
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, Hostage))
        {
            TargetHit = hit.transform.gameObject.GetComponentInParent<TargetController>();
            if (!TargetHit.IsHit())
            {
                TargetHit.Hit();
                GameController.Hostage();
            }
        }
        // checks if raycast hit head target and runs headshot method to adjust points
        else if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, HeadTarget))
        {
            TargetHit = hit.transform.gameObject.GetComponentInParent<TargetController>();
            if (!TargetHit.IsHit())
            {
                TargetHit.Hit();
                GameController.HeadShot();
            }
        }
        // checks if raycast hit target body and runs hit method to adjust points
        else if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, Target))
        {
            TargetHit = hit.transform.gameObject.GetComponentInParent<TargetController>();
            if (!TargetHit.IsHit())
            {
                TargetHit.Hit();
                GameController.Hit();
            }
        }

        // run coroutine until recoil animation is finished
        yield return new WaitForSeconds(RecoilTime);

        // Finish recoil animations
        if (ADS)
            animator.SetBool("ADSRecoil", false);
        else
            animator.SetBool("Recoil", false);

        // set shooting to false and allow other input to be allowed
        IsShooting = false;
    }

    // Coroutine for reseting ammo count, and playing reload animations and sound
    IEnumerator Reload()
    {
        // set reloading to true so no other input is read
        IsReloading = true;
        // play reload sound
        ReloadSound.Play();

        // play seperate animations whether player is looking down sights or not
        if (ADS)
            animator.SetBool("ADSReload", true);
        else
            animator.SetBool("Reload", true);

        // wait for animation to finish
        yield return new WaitForSeconds(ReloadTime);

        // stop animations
        if (ADS)
            animator.SetBool("ADSReload", false);
        else
            animator.SetBool("Reload", false);

        // reset ammo to the max amount
        CurrentAmmo = MaxAmmo;
        // set reloading to false and allow input
        IsReloading = false;
    }

    // Coroutine for playing Aim down sights transition, new idle position, and deactivating the reticle
    IEnumerator StartAimDownSights()
    {
        // set Aim Down Sight flag to true
        ADS = true;
        // Deactivate reticle on UI
        reticle.SetActive(false);
        // Play transition animation to ADS
        animator.SetBool("AimDownSights", true);
        // wait for animation to finish
        yield return new WaitForSeconds(AimDownSightSpeed);
        // Set guns new idle position to Aim down sights Idle
        animator.SetBool("ADSIdle", true);

    }

    // Coroutine for playing Aim down sights return transition, new idle position, and activating the reticle
    IEnumerator StopAimDownSights()
    {
        // set aim down sight flag to false
        ADS = false;
        // set idle to false, making it switch to regular gun idle position
        animator.SetBool("ADSIdle", false);
        animator.SetBool("AimDownSights", false);
        // wait for gun to move positions
        yield return new WaitForSeconds(AimDownSightSpeed);
        // Activate the reticle in the UI
        reticle.SetActive(true);
    }
}
