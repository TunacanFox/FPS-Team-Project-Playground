using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace FPSProto.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float speed = 3.0f;
        [SerializeField] float runSpeed = 6.0f;
        [SerializeField] float mouseSpeed = 5.0f;
        [SerializeField] float _rad = 1.5f;
        [SerializeField] LayerMask PickableMask;
        public Transform PickedPos;
        private float _gravity;
        private CharacterController _controller;
        private Animator _animator;
        private Vector3 _direction;
        private float _mouseX;
        private GameObject PickableItem;
        private float _currentSpeed;
        private bool _isMoving;

        private PhotonView _pv;

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Start()
        {
            _pv = GetComponent<PhotonView>();
            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _direction = Vector3.zero;
            _gravity = 9.8f;
            _currentSpeed = speed;
            _isMoving = false;
        }

        void Update()
        {


            Moving();
            Animating();

            // 물건 줍기
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (PickableItem == null)
                {
                    Check();
                }
                else
                {
                    Drop();
                }
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _rad);
        }

        public void Moving()
        {
            _mouseX += Input.GetAxis("Mouse X") * mouseSpeed;
            transform.localEulerAngles = new Vector3(0, _mouseX, 0);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _currentSpeed = runSpeed;
            }

            _direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (_direction.magnitude > 0)
            {
                _isMoving = true;
                _direction = _controller.transform.TransformDirection(_direction);
            }
            else
                _isMoving = false;

            if (!_controller.isGrounded)
            {
                _direction.y -= _gravity * Time.deltaTime;
            }

            _controller.Move(_direction * Time.deltaTime * _currentSpeed);
        }

        public void Animating()
        {
            if (_isMoving)
            {
                if (_currentSpeed == speed)
                {
                    _animator.SetBool("IsWalking", true);
                    _animator.SetBool("IsRunning", false);
                }
                else if (_currentSpeed == runSpeed)
                {
                    _animator.SetBool("IsWalking", false);
                    _animator.SetBool("IsRunning", true);
                }
            }
            else
            {
                _animator.SetBool("IsWalking", false);
                _animator.SetBool("IsRunning", false);
            }
        }

        public void Check()
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, _rad, PickableMask);
            if (cols.Length > 0)
            {
                PickUp(cols[0].gameObject);
            }
        }

        public void PickUp(GameObject item)
        {
            PickableItem = item;
            item.transform.position = PickedPos.position;
            item.transform.SetParent(PickedPos);
            Rigidbody rigidbody = item.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;

            Collider itemCollider = item.GetComponent<Collider>();
            if (itemCollider != null)
            {
                itemCollider.enabled = false; // 콜라이더 비활성화
            }
        }

        public void Drop()
        {
            PickableItem.transform.SetParent(null);
            Rigidbody rigidbody = PickableItem.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;

            Collider itemCollider = PickableItem.GetComponent<Collider>();
            if (itemCollider != null)
            {
                itemCollider.enabled = true; // 콜라이더 활성화
            }

            PickableItem = null;
        }
    }
}
