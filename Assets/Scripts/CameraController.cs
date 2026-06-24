using UnityEngine;

namespace SHIFTER
{
    public class CameraController : MonoBehaviour
    {
        public Transform player;     // 따라갈 플레이어의 Transform
        public float smoothing = 5f;  // 카메라가 따라오는 속도

        [Header("Horizontal Dead Zone")] // 좌우 데드존 크기
        public float horizontalDeadZone = 4.0f;

        [Header("Vertical Dead Zone")] // 상하 데드존 크기
        public float deadZoneUp = 3.5f;
        public float deadZoneDown = 1.0f;

        private Vector3 offset = new Vector3(0, 0, -10);

        private float targetX;
        private float targetY;

        void Start()
        {
            if (player != null)
            {
                targetX = player.position.x;
                targetY = player.position.y;

                Vector3 initPos = new Vector3(targetX, targetY, 0f) + offset;
                transform.position = initPos;
            }
        }

        void LateUpdate()
        {
            if (player == null) return;

            // 1. [좌우 데드존 계산]
            float deltaX = player.position.x - targetX;
            if (Mathf.Abs(deltaX) > horizontalDeadZone)
            {
                targetX += deltaX - (Mathf.Sign(deltaX) * horizontalDeadZone);
            }

            // 2. [위아래 데드존 계산 - 분리형 적용]
            float deltaY = player.position.y - targetY;

            if (deltaY > 0.0f) // 플레이어가 카메라 목표치보다 '위'에 있을 때 (점프 중)
            {
                if (deltaY > deadZoneUp)
                {
                    // 위쪽 데드존 경계선을 넘었을 때만 카메라 목표 Y를 올려줍니다.
                    targetY += deltaY - deadZoneUp;
                }
            }
            else // 플레이어가 카메라 목표치보다 '아래'에 있을 때 (추락/낙하 중)
            {
                if (Mathf.Abs(deltaY) > deadZoneDown)
                {
                    // 아래쪽 데드존 경계선을 넘었을 때 카메라 목표 Y를 내려줍니다.
                    // deadZoneDown을 작게 잡으면 거의 실시간으로 바닥을 쫓아 내려갑니다.
                    targetY += deltaY + deadZoneDown;
                }
            }

            // 3. 최종 목표 위치 및 부드러운 이동
            Vector3 targetPosition = new Vector3(targetX, targetY, 0f) + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        }

        // 💡 에디터 Scene 창에서 비대칭 데드존 박스를 시각적으로 보여줍니다.
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            // 현재 카메라 목표 시스템의 중심점
            Vector3 center = new Vector3(targetX, targetY, 0f);

            // 비대칭 박스의 실제 시각적 중심과 크기 계산
            float boxHeight = deadZoneUp + deadZoneDown;
            float boxCenterY = targetY + (deadZoneUp - deadZoneDown) / 2f;
            Vector3 visualCenter = new Vector3(targetX, boxCenterY, 0f);
            Vector3 size = new Vector3(horizontalDeadZone * 2f, boxHeight, 0.1f);

            Gizmos.DrawWireCube(visualCenter, size);
        }
    }
}