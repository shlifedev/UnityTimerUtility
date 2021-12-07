using System;
using UnityEngine;



/// <summary>
/// 사용 예시
/// </summary>
class UseExample // : MonoBehaviour
{
    private UTimer questEndTimer;
    void Awake()
    {
        questEndTimer = UTimer.CreateBySecond(300);
    }

    void OnEndQuestTimer()
    {
        
    }
 
    void Update()
    {
        if (questEndTimer.IsFinish())
        {
            //타이머 완료콜백 호출
            questEndTimer?.onCompleteCallback();
            //타이머 재사용
            questEndTimer.SetTime(300);
        }
    }
}
/// <summary>
/// 타이머를 관리하기 위한 클래스.
/// 이후 UTC 시간을 관리하는것도 이곳에서 한다.
/// </summary>
public class UTimer
{
    /// <summary>
    /// 타이머 시간 지정
    /// </summary> 
    public static UTimer CreateBySecond(float second, System.Action onCompleteCallback = null)
    { 
        UTimer timer = new UTimer(); 
        timer.GoalTime = Time.realtimeSinceStartup + second;
        timer.onCompleteCallback = onCompleteCallback;
        return timer;
    }
    
    /// <summary>
    /// 이것으로 생성하면 자동으로 서버 UTC 시간을 계산해준다.
    /// 인자값으로 2021년 5월30일 오후3시를 넣고, 서버시간이 오후2시라면 1시간이 알아서 들어간다.
    /// 주의 : NetworkModule을 사용할 수 있는 환경에서만 사용가능
    /// </summary>  
    public static UTimer CreateUTC(System.DateTime goalTime, System.Action onCompleteCallback = null)
    { 
        UTimer timer = new UTimer();
        
        //현재 서버 시간을 가져온다.
        var serverTime = NetworkModule.Instance.GetCurrentServerTime();
        if (goalTime < serverTime)
        {
            throw new Exception("서버 시간보다 goal Time이 낮을 수 없습니다.");
        }
        var remainSec = (goalTime - serverTime).TotalSeconds;
        timer.GoalTime = Time.realtimeSinceStartup + (float)remainSec;
        timer.onCompleteCallback = onCompleteCallback;
        return timer;
    }

    public System.Action onCompleteCallback = null;

    /// <summary>
    /// 목표 시간
    /// </summary>
    public float GoalTime
    {
        get;
        private set;
    }
    /// <summary>
    /// 남은 시간
    /// </summary>
    public float RemainTime
    {
        get
        { 
            return GoalTime - Time.realtimeSinceStartup;
        }
    }

    public TimeSpan RemainTimeTimeSpan
    {
        get
        {
            return TimeSpan.FromSeconds(RemainTime);
        }
    } 
    /// <summary>
    /// 완료 되었는가?
    /// </summary>
    /// <returns></returns>
    public bool IsFinish() => RemainTime <= 0;

    /// <summary>
    /// 타이머에 시간 지정 (시간을 덮어쓴다)
    /// </summary>
    /// <param name="second"></param>
    public void SetTime(float second)
    {
        GoalTime = Time.realtimeSinceStartup + second;
    }

    /// <summary>
    /// 타이머에 시간 추가
    /// </summary>
    /// <param name="second"></param>
    public void AddTime(float second)
    {
        GoalTime += second;
    }
    
    /// <summary>
    /// 타이머에 시간 빼기
    /// </summary>
    /// <param name="second"></param>
    public void SubTime(float second)
    {
        GoalTime -= second;
    }
}
