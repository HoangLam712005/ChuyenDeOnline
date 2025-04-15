using UnityEngine;
using Fusion;

public class PlayerMovement : NetworkBehaviour
{
    CharacterController _controller;
    public float speed = 5f;

    // Các biến thêm vào để xử lý nhảy và trọng lực
    private Vector3 _velocity;
    private bool _jumpPressed;
    public float JumpForce = 5f;
    public float GravityValue = -9.81f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_controller.isGrounded) // khi nhân vật đang chạm sàn
        {
            // Tạo một lực nhỏ hướng xuống giúp nhân vật không bị trôi
            _velocity = new Vector3(0, -1, 0);
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * speed;

        // Tăng vận tốc trục Y theo trọng lực
        // Làm nhân vật rơi xuống khi không ở trên mặt đất
        _velocity.y += GravityValue * Runner.DeltaTime;
        if (_jumpPressed && _controller.isGrounded)
        {
            // Tăng vận tốc theo trục Y để nhân vật nhảy lên
            _velocity.y += JumpForce;
        }

        _controller.Move(move + _velocity * Runner.DeltaTime);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }
        _jumpPressed = false;
    }

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _jumpPressed = true;
        }
    }
}
