using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use();
    public abstract override void Reload();
    public abstract override void Recoil();
    public abstract override void Recover();
    public abstract override bool getRecoiling();
    public abstract override bool getRecovering();

    public GameObject bulletImpactPrefab;
    public GameObject CraterFrefabs;
    public  GameObject currentWeaponGraphics;
}
