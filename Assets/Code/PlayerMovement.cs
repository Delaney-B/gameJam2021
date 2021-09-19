using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    private CalmciergeControls _controls;
    private InputAction _movement;

    [SerializeField] private int maximumPanic = 3;
    private int _panic = 0;

    [SerializeField] private float beatThreshold = 0.1f;
    private bool _wantsMove = false;
    private bool _moving = false;
    private float _moveSpeed;
    private Vector2 _moveTarget = Vector2.zero;

    private void Awake() {
        _controls = new CalmciergeControls();
        _movement = _controls.Player.Move;

        Metronome.GetInstance()
            .tickEvent.AddListener(Tick);
    }

    private void OnEnable() {
        _movement.performed += OnMove;
        _movement.Enable();
    }

    private void OnDisable() {
        _movement.Disable();
    }


    private void OnMove(InputAction.CallbackContext obj) {
        float proximity = Metronome.GetInstance()
            .BeatProximity;
        if (proximity <= beatThreshold) {
            Vector2 movementValue = _movement.ReadValue<Vector2>();
            Vector3 position = transform.position;

            _wantsMove = false;

            Vector2 gridPosition = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
            if (Mathf.Abs(movementValue.x) > 0.5f) {
                _moveTarget = gridPosition + new Vector2(Mathf.Sign(movementValue.x), 0f);
                _wantsMove = true;
            }
            else if (Mathf.Abs(movementValue.y) > 0.5f) {
                _moveTarget = gridPosition + new Vector2(0f, Mathf.Sign(movementValue.y));
                _wantsMove = true;
            }
        }
        else {
            _wantsMove = false;
        }
    }

    private void Update() {
        if (_moving) {
            Vector2 position = transform.position;

            Vector3 newPosition = Vector2.MoveTowards(position, _moveTarget, Time.deltaTime * (1f / _moveSpeed));

            transform.position = newPosition;
        }
    }

    private void Tick() {
        if (_wantsMove) {
            float panicModifier = 1f / (_panic + 1);
            _moveSpeed = Metronome.GetInstance()
                             .BeatTime *
                         panicModifier;
            _moving = true;
            _panic = Mathf.Max(_panic - 1, 0);
        }
        else {
            // We're not moving; increase panic!
            _moving = false;
            _panic = Mathf.Min(_panic + 1, maximumPanic);
        }
    }
}
