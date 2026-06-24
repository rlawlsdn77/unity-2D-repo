using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SHIFTER
{
    public class SceneChanger : MonoBehaviour
    {
        public void ChangeSceneByName(string sceneName)
        {
            if (sceneName == "Exit")
            {
                Application.Quit(); // 게임 나가기
                UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터 나가기
            }
            else SceneManager.LoadScene(sceneName);  // 씬 이름에 맞게 이동
        }
    }
}