using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;


public class PlayerProperties : NetworkBehaviour
{
    public NetworkObject Object => GetComponent<NetworkObject>();

    public Camera mainCamera; // camera chính
    public float fireballSpeed = 20f; // tốc độ fireball
    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    public int health { get; set; } = 100;
    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    public int mana { get; set; } = 100;
    [Networked, OnChangedRender(nameof(OnInfoChanged))]
    public int score { get; set; } = 0;

    public GameObject weapon;

    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    [Networked, OnChangedRender(nameof(OnAnimationFireChanged))]
    public bool Fire { get; set; } = false;
    private void OnAnimationFireChanged()
    {
        anim.SetTrigger("Fire");
    }


    Animator anim;
    [Networked, OnChangedRender(nameof(OnAnimationChanged))]
    public bool Slash { get; set; } = false;


    private void OnAnimationChanged()
    {
        anim.SetTrigger("Slash");
    }

    public Slider sliderHealth;
    public Slider sliderMana;
    public TextMeshProUGUI scoreText;


    private void OnInfoChanged()
    {
        sliderHealth.value = health;
        sliderMana.value = mana;
        scoreText.text = score + "";
    }
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim = GetComponent<Animator>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main; // tìm camera chính nếu chưa gán
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (HasInputAuthority)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Slash = !Slash;
                weapon.GetComponent<BoxCollider>().enabled = true;
            }
            else if (Input.GetMouseButtonUp(0)) weapon.GetComponent<BoxCollider>().enabled = false;

            if (Input.GetMouseButtonDown(1))
            {
                Fire = !Fire;
            }

        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcTakeDamage(int damage)
    {
        health -= damage;  // Giảm sức khỏe khi nhận sát thương
        score += 10;
        Debug.Log("Player received damage. Health: " + health);


        if (health <= 0)
        {
            //Die();  // Player chết khi sức khỏe <= 0
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_Fireball()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100f);
        }

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        NetworkObject fireball = Runner.Spawn(fireballPrefab, firePoint.position, Quaternion.LookRotation(direction), Object.InputAuthority);

        // Gán owner
        if (fireball.TryGetComponent<FireballBehaviour>(out var fb))
        {
            fb.SetOwner(Object.InputAuthority);
            fb.speed = fireballSpeed; // gán speed nếu muốn
        }
    }


    public void OnAnimationFireballEvent()
    {
        if (Object.HasStateAuthority) // Host gọi fireball
        {
            RPC_Fireball();
        }
    }

    public override void Spawned()
    {
        // Chỉ hiển thị UI cho chính người chơi
        if (Object.HasInputAuthority)
        {
            // Ẩn UI của người khác
            scoreText.gameObject.SetActive(false);
        }
    }



    public TMP_Text nameDisplay; // Gắn Text nằm trên đầu nhân vật

    private string playerName;

    public void SetPlayerName(string newName)
    {
        playerName = newName;
        if (nameDisplay != null)
            nameDisplay.text = newName;
    }

}
