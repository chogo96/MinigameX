using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Gui_Layout : MonoBehaviour
{
	public enum positionType
	{
		TopLeft,
		TopMiddle,
		TopRight,
		MiddleLeft,
		Middle,
		MiddleRight,
		BottomLeft,
		BottomMiddle,
		BottomRight
	}

	public positionType _positionType = positionType.Middle;
	public float margin_x;
	public float margin_y;
	public int _depth;

	RectTransform _rectTransform;
	Text _gui_text;
	Image _gui_texture;

	void Awake()
	{
#if !(UNITY_EDITOR)
        _rectTransform = GetComponent<RectTransform>();
        _gui_text = GetComponent<Text>();
        _gui_texture = GetComponent<Image>();

        PositionSetting();
#endif
	}

	void Update()
	{
#if UNITY_EDITOR
		_rectTransform = GetComponent<RectTransform>();
		_gui_text = GetComponent<Text>();
		_gui_texture = GetComponent<Image>();

		this.gameObject.transform.position = new Vector3(0, 0, -0.01f * _depth);
		PositionSetting();
#endif
	}

	void PositionSetting()
	{
		if (_rectTransform == null) return;

		Vector2 anchorMin = Vector2.zero;
		Vector2 anchorMax = Vector2.zero;
		Vector2 pivot = Vector2.zero;

		switch (_positionType)
		{
			case positionType.TopLeft:
				anchorMin = anchorMax = new Vector2(0, 1);
				pivot = new Vector2(0, 1);
				break;
			case positionType.TopMiddle:
				anchorMin = anchorMax = new Vector2(0.5f, 1);
				pivot = new Vector2(0.5f, 1);
				break;
			case positionType.TopRight:
				anchorMin = anchorMax = new Vector2(1, 1);
				pivot = new Vector2(1, 1);
				break;
			case positionType.MiddleLeft:
				anchorMin = anchorMax = new Vector2(0, 0.5f);
				pivot = new Vector2(0, 0.5f);
				break;
			case positionType.Middle:
				anchorMin = anchorMax = new Vector2(0.5f, 0.5f);
				pivot = new Vector2(0.5f, 0.5f);
				break;
			case positionType.MiddleRight:
				anchorMin = anchorMax = new Vector2(1, 0.5f);
				pivot = new Vector2(1, 0.5f);
				break;
			case positionType.BottomLeft:
				anchorMin = anchorMax = new Vector2(0, 0);
				pivot = new Vector2(0, 0);
				break;
			case positionType.BottomMiddle:
				anchorMin = anchorMax = new Vector2(0.5f, 0);
				pivot = new Vector2(0.5f, 0);
				break;
			case positionType.BottomRight:
				anchorMin = anchorMax = new Vector2(1, 0);
				pivot = new Vector2(1, 0);
				break;
		}

		_rectTransform.anchorMin = anchorMin;
		_rectTransform.anchorMax = anchorMax;
		_rectTransform.pivot = pivot;

		_rectTransform.anchoredPosition = new Vector2(margin_x, margin_y);
	}
}
