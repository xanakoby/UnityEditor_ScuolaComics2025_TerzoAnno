using UnityEngine;
using UnityEditor;

public class EsempioMenu : MonoBehaviour
{

    //mi fa una sezione in alto nuova con il nome tools e dentro la voce creaogg
    [MenuItem("Tools/voce di menu personalizzata")]
    private static void CreaOggettoTMP()
    {
        Debug.Log("creato ogg tmp");
    }

    //a seconda del context che prendo la voce resetyyy viene data a quella
    [MenuItem("CONTEXT/Transform/ResetYYY")]
    private static void InContestoooo()
    {
        Debug.Log("InContestoooo");
        Debug.Log(Selection.activeTransform.name);
    }
    //Batch Object Manager
}
