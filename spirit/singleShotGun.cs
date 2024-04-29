using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Rendering;
using TMPro;

public class singleShotGun : Gun
{
    [SerializeField] Camera cam;
    
    public Transform MuzzlePoint;
    PhotonView PV;
    private GunInfo GunInfo;
    public int ammo = 8;
    public int magammo = 8;
    public TextMeshProUGUI ammotext;
    public TextMeshProUGUI magammotext;
    public Animation Animation;
    public AnimationClip reload;
    [Header("Recoil Setting")]
    /*[Range(0f, 1f)]
    public float recoilPecent=0.3f;*/
    [Range(0f, 2f)]
    public float recoverPercent = 0.7f;
    [Space]
    public float recoilUp = 0.02f;
    public float recoilBack = 0.05f;
    float nextFire = 0.1f;

    private Vector3 originPosition;
    private Vector3 recoilVelocity=Vector3.zero;

    private float recoilLenth;
    private float recoverLenth;

    private bool recoiling;
    private bool recovering;


    public AudioClip clip;
    public AudioClip reloadingclip;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void Start()
    {
        /*magammotext.text = magammo.ToString();*/
        ammotext.text = ammo + "/" + magammo;
        originPosition = transform.localPosition;

        recoilLenth = 0f;
        recoverLenth = 1 / nextFire * recoverPercent;
    }
    public override void Reload()
    {
        if(ammo< magammo)
        {
            Animation.Play(reload.name);
            AudioSource.PlayClipAtPoint(reloadingclip, MuzzlePoint.position);
            ammo = magammo;
            Debug.Log("装弹成功");
        }
        /*magammotext.text = magammo.ToString();*/
        ammotext.text = ammo + "/" + magammo;

    }
    public override void Use()
    {
        if(ammo>0&&Animation.isPlaying==false)
        {
            Shoot();
        }else if(ammo==0)
        {
            Debug.Log("没子弹了");
        }
        
    }


    private void Shoot()
    {
        recoiling = true;
        recovering = false;
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        ammo--;
        
        /*magammotext.text = magammo.ToString();*/
        ammotext.text = ammo + "/" + magammo;
        if (currentWeaponGraphics != null &&clip != null)
        {
            GameObject muzzleFlashInstance = Instantiate(currentWeaponGraphics,MuzzlePoint.position,MuzzlePoint.rotation,MuzzlePoint.transform);
            AudioSource.PlayClipAtPoint(clip, MuzzlePoint.position);
            Destroy(muzzleFlashInstance ,0.1f);
        }
        if(Physics.Raycast(ray, out RaycastHit hit) )
        {
            hit.collider.gameObject.GetComponent<IDamage>() ? .TakeDamage(((GunInfo)Info).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point,hit.normal);
        }
    }
    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition,Vector3 hitnomral)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        Collider[] colliders1 = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length !=0 ) {
            GameObject bulletImpactOJ = Instantiate(bulletImpactPrefab, hitPosition + hitnomral * 0.001f, Quaternion.LookRotation(hitnomral, Vector3.up) * bulletImpactPrefab.transform.rotation);
            GameObject CraterImactOJ = Instantiate(CraterFrefabs, hitPosition + hitnomral * 0.001f, Quaternion.LookRotation(hitnomral, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactOJ,10f);
            Destroy(CraterImactOJ, 3f);
            bulletImpactOJ.transform.SetParent(colliders[0].transform);
            CraterImactOJ.transform.SetParent(colliders1[0].transform);
        }
       
    }
    public override bool getRecoiling()
    {
        return recoiling;
    }
    public override bool getRecovering()
    {
        return recovering;
    }
    public override void Recoil()
    {
        Vector3 finalPosition = new Vector3(originPosition.x,originPosition.y+recoilUp,originPosition.z-recoilBack);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition ,ref recoilVelocity,recoilLenth);

        if(transform.localPosition==finalPosition )
        {
            recoiling = false;
            recovering = true;
        }
    }
    public override void Recover()
    {
        Vector3 finalPosition = originPosition;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLenth);

        if (transform.localPosition == finalPosition)
        {
            recoiling = true;
            recovering = false;
        }
    }
}
