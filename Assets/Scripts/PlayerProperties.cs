using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;


public class PlayerProperties : NetworkBehaviour
{
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
        Transform lefthand = gameObject.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftHand);
        NetworkObject fireball = Runner.Spawn(fireballPrefab, lefthand.position, Quaternion.LookRotation(lefthand.up), Object.InputAuthority);
    }


    public void OnAnimationFireballEvent()
    {
        if (Object.HasStateAuthority) // Chỉ host mới được gửi RPC
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


}
