using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tv : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] SpriteRenderer TvFrame;
    [SerializeField] SpriteRenderer ChannelWindow;
    Sprite[] TvChannelsSource;
    float sizeMargin = 0.1f;
    int[] Channels;
    bool isSet = false;
    void Start()
    {
        TvChannelsSource = Resources.LoadAll<Sprite>("Images/TuneTv/Channel");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChannelChange(Sprite sprite)
    {
        Debug.Log($"ChannelChange : {sprite.name}");
        ChannelWindow.sprite = sprite;
        MatchSpriteSize();
    }
    public void SettingChannels(int[] Channels)
    {
        if (TvChannelsSource == null)
        {
            TvChannelsSource = TvChannelsSource = Resources.LoadAll<Sprite>("Images/TuneTv/Channel");
        }
        this.Channels = Channels;
        ChannelWindow.sprite = TvChannelsSource[Channels[0]];
        MatchSpriteSize();
        isSet = true;
    }
    void ChannelUp() { }
    void ChannelDown() { }

    void MatchSpriteSize()
    {


        if (ChannelWindow != null && TvFrame != null)
        {
            // B의 크기 구하기
            Vector2 sizeTv = TvFrame.bounds.size;

            // A의 현재 크기 구하기
            Vector2 sizeChannel = ChannelWindow.bounds.size;

            // A의 스케일을 B의 크기에 맞추기 위해 비율 계산
            Vector3 newScale = ChannelWindow.gameObject.transform.localScale;
            newScale.x *= (sizeTv.x - sizeMargin) / sizeChannel.x;
            newScale.y *= (sizeTv.y - sizeMargin) / sizeChannel.y;

            // A의 스케일을 새 크기에 맞게 조정
            ChannelWindow.gameObject.transform.localScale = newScale;
        }
    }
}
