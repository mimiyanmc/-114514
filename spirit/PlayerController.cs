using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviourPunCallbacks,IDamage
{
    [SerializeField] Image HealthBarImage;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, SprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] Item[] items;
    [SerializeField] Menu menu;
    int ItemIndex;
    int PreviousItemIndex = -1;
    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    Rigidbody rb;
    PhotonView PV;
    const float maxHealth = 100f;
    float nextFire = 0.1f;
    float currentHealth = maxHealth;
    PlayerManager playerManager;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }
    private void Start()
    {
        menu.open = false;
        if(PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(UI);
        }
        /*Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;*/
    }
    private void Update()
    {
        if (!PV.IsMine)
            return;
        nextFire--;
        
        

            look();
            Move();
            Jump();
            for (int i = 0; i < items.Length; i++)
            {
                if (Input.GetKeyDown((i + 1).ToString()))
                {
                    EquipItem(i);
                    break;
                }
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                if (ItemIndex >= items.Length - 1)
                {
                    EquipItem(0);
                }
                else
                    EquipItem(ItemIndex + 1);
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                if (ItemIndex <= 0)
                {
                    EquipItem(items.Length - 1);
                }
                else
                {
                    EquipItem(ItemIndex - 1);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (nextFire < 0)
                    items[ItemIndex].Use();
                nextFire = 0.1f;
            }
            if (Input.GetKey(KeyCode.R))
            {
                items[ItemIndex].Reload();
            }
        
        
        if(transform.position.y < -10f)
        {
            Die();
        }
        if (items[ItemIndex].getRecoiling()==true)
        {
            items[ItemIndex].Recoil();
        }
        if(items[ItemIndex].getRecovering() == true)
        {
            items[ItemIndex].Recover();
        }
        /*if(Input.GetKey(KeyCode.Escape))
        {
            if(menu.open != true)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                MenuManager.Instance.openMenu(menu);
                
            }
            if(menu.open)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                MenuManager.Instance.closeMenu(menu);

            }
            
        }*/
    }
    private void Move()
    {
        Vector3 MoveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, MoveDir * (Input.GetKey(KeyCode.LeftShift) ? SprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }
    void look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraHolder.transform.localEulerAngles = verticalLookRotation * Vector3.left;
    }
    void EquipItem(int _index)
    {
        if(_index == PreviousItemIndex)
        {
            return;
        }
        ItemIndex = _index;
        items[ItemIndex].itemGameObject.SetActive(true);
        if(PreviousItemIndex!=-1)
        {
            items[PreviousItemIndex].itemGameObject.SetActive(false);
        }
        PreviousItemIndex = ItemIndex;
        if(PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("ItemIndex", ItemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(changedProps.ContainsKey("ItemIndex")&&!PV.IsMine&&targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["ItemIndex"]);
        }
    }
    public void SetGroundedState(bool _gounded)
    {
        grounded = _gounded;
    }
    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    public void TakeDamage(float damage)
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float damage,PhotonMessageInfo info)
    {
        Debug.Log("took damage:"+damage);
        currentHealth -= damage;
        HealthBarImage.fillAmount = currentHealth / maxHealth;
        if(currentHealth <= 0) {
            Die();
            PlayerManager.Find(info.Sender).getKill();
        }
    }
    void Die()
    {
        playerManager.Die();
    }
}
