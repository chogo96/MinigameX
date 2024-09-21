using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTintColor : MonoBehaviour
{
    // 변경할 색상, Inspector에서 설정할 수 있도록 SerializedField로 선언
    [SerializeField]
    private Color tintColor = Color.red;

    // 사용할 Material
    private Material material;

    void Start()
    {
        // Renderer 컴포넌트에서 Material을 가져옵니다.
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // Material을 가져옵니다.
            material = renderer.material;

            // Material이 null이 아닌 경우 Tint Color를 변경합니다.
            if (material != null)
            {
                // "_Color"는 일반적인 Tint Color의 속성 이름입니다.
                // 다른 셰이더를 사용하고 있다면 속성 이름이 다를 수 있습니다.
                material.SetColor("_Color", tintColor);
            }
            else
            {
                Debug.LogError("Material is not assigned to the object.");
            }
        }
        else
        {
            Debug.LogError("Renderer component is missing.");
        }
    }
}
