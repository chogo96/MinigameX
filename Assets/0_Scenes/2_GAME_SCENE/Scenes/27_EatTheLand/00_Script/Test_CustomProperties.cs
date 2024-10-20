using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Test_CustomProperties : MonoBehaviour
{
    public string TEAM_PROPERTY = "team";
    public Hashtable property = new Hashtable();
    public bool isBlueTeam;
    // Start is called before the first frame update
    void Start()
    {
        property.Add(TEAM_PROPERTY, isBlueTeam);
        LetYouKnowPlayerTeam();
    }

    void Update()
    {
        UpdateTeamInfo();
    }
    private void UpdateTeamInfo()
    {
        if(isBlueTeam)
        {
            property[TEAM_PROPERTY] = true;
        }
        else
        {
            property[TEAM_PROPERTY] = false;
        }

        Debug.Log(property[TEAM_PROPERTY]);
    }

    //  단순히 불값으로 확인이 가능은 하지만 실제로는 커스텀 프로퍼티를 통해 확인을 하기 떄문이 임시적으로 체크해봄
    public int LetYouKnowPlayerTeam()
    {
        bool isBlueTeam = (bool)property[TEAM_PROPERTY];
        return isBlueTeam ? 1 : 0;
    }
}

