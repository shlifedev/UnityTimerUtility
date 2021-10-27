using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 백그라운드 상태 상관없이 남은 시간을 체크해주는 유틸리티
/// </summary>
public static class TimerUtility
{
    /// <summary>
    /// realtimeSinceUp 으로부터 남은 목표시간을 기록하는 저장소
    /// </summary>
    static Dictionary<string, float> TimeMap = new Dictionary<string, float>();
    
    
    /// <summary>
    /// 씬이 변경되기 직전에 호출해서 상태를 초기화 시키는 용도 혹은 각 상황에 맞게 필요할때 호출
    /// </summary>
    public static void Clear()
    {
        TimeMap.Clear();
    }
    
    /// <summary>
    /// 남은 시간을 0으로 만듬 (남은시간 체크시 true를 반환하는 상태가 됨)
    /// </summary>
    public static void SetAviliable(string key)
    {
        if (TimeMap.ContainsKey(key)) TimeMap[key] = float.MinValue;
        else
        {
            TimeMap.Add(key, float.MinValue);
        }
    }

    
    
    
    /// <summary>
    /// 현재 시간으로부터 remainTime만큼 남은시간을 계산하려면 사용
    /// </summary>
    public static void SetRemainTime(string key, float remainTime)
    {
        if (TimeMap.ContainsKey(key)) TimeMap[key] = Time.realtimeSinceStartup + remainTime;
        else
        {
            TimeMap.Add(key, Time.realtimeSinceStartup + remainTime);
        }
    }

    /// <summary>
    /// 남은 시간 가져옴
    /// </summary>
    /// <param name="whenKeyNullSetRemainTime"> 만약 시간을 가져오려고 했는데 키값이 없으면 설정하고 싶은 남은 시간 </param>
    /// <returns></returns>
    public static float GetRemainTime(string key, float whenKeyNullSetRemainTime)
    {
        var flag = TimeMap.TryGetValue(key, out var goalTime);
        if (flag == false)
        {
            if(whenKeyNullSetRemainTime != -1)
               SetRemainTime(key, whenKeyNullSetRemainTime);
        } 

        var remainTime = goalTime - Time.realtimeSinceStartup;
        return remainTime;
    }


    /// <summary>
    /// 남은 시간 가져옴
    /// </summary> 
    /// <returns></returns>
    public static float GetRemainTime(string key)
    {
        var flag = TimeMap.TryGetValue(key, out var goalTime);
        if (flag == false)
            throw new Exception(key +" is null!!");

        var remainTime = goalTime - Time.realtimeSinceStartup;
        return remainTime;
    }


    /// <summary>
    /// 시간이 흘렀는지 체크
    /// </summary>
    /// <param name="key"></param>
    /// <param name="whenKeyNullSetRemainTime"> 만약 시간을 가져오려고 했는데 키값이 없으면 설정하고 싶은 남은 시간 (안하고싶으면 -1) </param>
    /// <returns></returns>
    public static bool IsTimerLeft(string key, float whenKeyNullSetRemainTime)
    {  
        return GetRemainTime(key, whenKeyNullSetRemainTime) <= 0;
    } 
}
