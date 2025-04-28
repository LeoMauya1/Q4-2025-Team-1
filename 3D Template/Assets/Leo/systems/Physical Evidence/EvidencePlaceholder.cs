using UnityEngine;

public class EvidencePlaceholder : MonoBehaviour
{
    public PhysicalEvidence evidence;
    public Collider InteractionField;
    public Material highightItme;
    public Material material;
    private Color ogColor;
    public Color glowColor;
    private bool isHighleted = false;





    private void Update()
    {
         if(isHighleted)
        {
            float pingPong = (Mathf.Sin(Time.time * 5f) + 1f) / 2f;
            material.color = Color.Lerp(ogColor, glowColor, pingPong);
        }
    }



    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        ogColor = material.color;
    }


    public void HighLightItemMaterial()
    {
        isHighleted = true;
    }
    public void ReturnItemMaterial()
    {
        isHighleted = false;
        GetComponent<MeshRenderer>().material.color = ogColor;
    }



}
