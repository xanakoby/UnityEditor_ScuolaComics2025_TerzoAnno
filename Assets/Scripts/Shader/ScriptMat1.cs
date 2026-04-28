using UnityEngine;

public class ScriptMat1 : MonoBehaviour
{
    [SerializeField] Renderer rend;
   [SerializeField] Color newColor = Color.blue;

    [Header("Color Block")]
        [SerializeField] MaterialPropertyBlock propertyBlock;
    int colorBaseID;

    private void Start()
    {
        //rend = GetComponent<Renderer>();
        //if (!rend)
        //{
        //    var rendMat = rend.material;
        //    rendMat.SetColor("_BaseColor", newColor);
        //}


        //var rendMat = rend.material;
        //rendMat.SetColor("_BaseColor", newColor);

        ChangeColorBlock();
    }

    public void ChangeColorBlock()
    {
        propertyBlock = new MaterialPropertyBlock();
        colorBaseID = Shader.PropertyToID("_BaseColor");

        rend.GetPropertyBlock(propertyBlock);

        propertyBlock.SetColor(colorBaseID, newColor);

        rend.SetPropertyBlock(propertyBlock);
    }
}
