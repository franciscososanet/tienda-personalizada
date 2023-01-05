using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour {

    public void CambiarEscenaA(string escena){
        SceneManager.LoadScene(escena);
    }

}
