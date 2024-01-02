using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public MMFeedbacks AccelerateFeedback;

    public void PlayAccelerationFeedbacks()
    {
        AccelerateFeedback?.PlayFeedbacks();
    }
}
