using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private void Start()
    {
        AkSoundEngine.PostEvent("Play_TrainAmbience", GameObject.FindGameObjectWithTag("Player"));
    }
    //static uint[] playingIds = new uint[50];

    //public static bool IsEventPlayingOnGameObject(string eventName, GameObject go)
    //{
    //    uint testEventId = AkSoundEngine.GetIDFromString(eventName);

    //    uint count = (uint)playingIds.Length;
    //    AKRESULT result = AkSoundEngine.GetPlayingIDsFromGameObject(go, ref count, playingIds);

    //    for (int i = 0; i < count; i++)
    //    {
    //        uint playingId = playingIds[i];
    //        uint eventId = AkSoundEngine.GetEventIDFromPlayingID(playingId);

    //        if (eventId == testEventId)
    //            return true;
    //    }

    //    return false;
    //}
}
