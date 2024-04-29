using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemInfo Info;
    public GameObject itemGameObject;

    public abstract void Use();
    public abstract void Reload();
    public abstract void Recoil();
    public abstract void Recover();

    public abstract bool getRecoiling();
    public abstract bool getRecovering();
}
