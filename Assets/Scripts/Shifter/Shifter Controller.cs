using Study.Utilities;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.InputSystem; // mKey.Space 지정을 위해 필요!

namespace SHIFTER
{
    [RequireComponent(typeof(CharacterController2D))]
    public class ShifterController : MonoBehaviour
    {
        private CharacterController2D characterController;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private string currentState { get; set; } = "Idle";

        public int FacingDirection { get; private set; } = 1;

        public static readonly int MOVEMENT = Animator.StringToHash("Movement");
        public static readonly int IS_RUN = Animator.StringToHash("IsRun");
        public static readonly int IS_JUMP= Animator.StringToHash("IsJump");
        public static readonly int IS_DOWN = Animator.StringToHash("IsDown");
        public static readonly int IS_SHIFT = Animator.StringToHash("IsShift");

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            characterController = GetComponent<CharacterController2D>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            MoveInput();

            if (SimpleInput.GetKeyDown(Key.LeftShift) || SimpleInput.GetKeyDown(Key.RightShift))
            {
                animator.SetBool(IS_SHIFT,true);
            }
            
        }

        private void MoveInput()
        {
            Vector2 inputVector = SimpleInput.GetMoveAxisRaw();
            float absMovement = Mathf.Abs(inputVector.x);
            animator.SetFloat(MOVEMENT, absMovement);

            characterController.SetMoveInput(inputVector.x);

            if(absMovement > 0) // 이동량이 있을때
            {
                // 삼항 연산자로 정면을 설정해준다
                FacingDirection = (inputVector.x < 0) ? -1 : 1;

                // x 스케일을 반전해서 좌우를 뒤집는다.
                // (히트박스를 뒤집기 위해서 스케일 반전을 사용)
                animator.transform.localScale = new Vector3(FacingDirection, 1, 1);
            }

            if (SimpleInput.GetKeyDown(Key.Space)) characterController.RequestJump();
        }
    }
}