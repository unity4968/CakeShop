﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour {

    [Header("Privacy Policy Link")]
    public string privacyPolicyLink;
	#region AdMob
	[Header("Admob")]
	public string adMobAppID = "";
	public string interstitalAdMobId = "";
	public string videoAdMobId = "";
	InterstitialAd interstitialAdMob;
	private RewardBasedVideoAd rewardBasedAdMobVideo; 
	AdRequest requestAdMobInterstitial, AdMobVideoRequest;
	#endregion
	[Space(15)]
	#region
	[Header("UnityAds")]
	public string unityAdsGameId;
	public string unityAdsVideoPlacementId = "rewardedVideo";
	#endregion

    [HideInInspector]
    public bool bVideoRewardReady = false;
    [HideInInspector]
    public bool bPlayVideoReward = false;

    public static bool unlockingInProgress = false;
    static bool rewardPlayer = false;
    public static GameObject itemForUnlocking;

	static AdsManager instance;

	public static AdsManager Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType(typeof(AdsManager)) as AdsManager;
			
			return instance;
		}
	}

	void Awake ()
	{
		gameObject.name = this.GetType().Name;
		DontDestroyOnLoad(gameObject);
	}

    void Start()
    {
        InitializeAds();
    }

	public void ShowInterstitial()
	{
		ShowAdMob();
	}

    public void IsVideoRewardAvailable()
    {
        Debug.Log("Is video reward available?");
        if (isVideoAvaiable())
        {
            Debug.Log("Video reward is available!");
            VideoRewardIsReady();
        }
        else
        {
            Debug.Log("Video reward is not available!");
            VideoRewardNotReady();
        }
    }

    public void ShowVideoReward()
    {
        rewardPlayer = false;

        if (Advertisement.IsReady(unityAdsVideoPlacementId))
        {
            UnityAdsShowVideo();
        }
        else if (rewardBasedAdMobVideo.IsLoaded())
        {
            AdMobShowVideo();
        }
        else
        {
            VideoRewardNotReady();
        }
    }

	private void RequestInterstitial()
	{
		// Initialize an InterstitialAd.
		interstitialAdMob = new InterstitialAd(interstitalAdMobId);

		// Called when an ad request has successfully loaded.
		interstitialAdMob.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		interstitialAdMob.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		interstitialAdMob.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		interstitialAdMob.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		interstitialAdMob.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		// Create an empty ad request.
		requestAdMobInterstitial = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitialAdMob.LoadAd(requestAdMobInterstitial);
	}

	public void ShowAdMob()
	{
		if(interstitialAdMob.IsLoaded())
		{
			interstitialAdMob.Show();
		}
		else
		{
			interstitialAdMob.LoadAd(requestAdMobInterstitial);
		}
	}

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        interstitialAdMob.LoadAd(requestAdMobInterstitial);
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    private void RequestRewardedVideo()
    {
        // Called when an ad request has successfully loaded.
        rewardBasedAdMobVideo.OnAdLoaded += HandleRewardBasedVideoLoadedAdMob;
        // Called when an ad request failed to load.
        rewardBasedAdMobVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoadAdMob;
        // Called when an ad is shown.
        rewardBasedAdMobVideo.OnAdOpening += HandleRewardBasedVideoOpenedAdMob;
        // Called when the ad starts to play.
        rewardBasedAdMobVideo.OnAdStarted += HandleRewardBasedVideoStartedAdMob;
        // Called when the user should be rewarded for watching a video.
        rewardBasedAdMobVideo.OnAdRewarded += HandleRewardBasedVideoRewardedAdMob;
        // Called when the ad is closed.
        rewardBasedAdMobVideo.OnAdClosed += HandleRewardBasedVideoClosedAdMob;
        // Called when the ad click caused the user to leave the application.
        rewardBasedAdMobVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplicationAdMob;
        // Create an empty ad request.
        AdMobVideoRequest = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
    }

    public void HandleRewardBasedVideoLoadedAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");

    }

    public void HandleRewardBasedVideoFailedToLoadAdMob(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
        VideoRewardNotReady();
    }

    public void HandleRewardBasedVideoOpenedAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStartedAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosedAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
        unlockingInProgress = false;
        if (!rewardPlayer)
            VideoRewardCanceled();
    }

    public void HandleRewardBasedVideoRewardedAdMob(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
        rewardPlayer = true;
        VideoRewarded();

    }

    public void HandleRewardBasedVideoLeftApplicationAdMob(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
        unlockingInProgress = false;
    }

    void InitializeAds()
    {
        MobileAds.Initialize(adMobAppID);
        this.rewardBasedAdMobVideo = RewardBasedVideoAd.Instance;
        this.RequestRewardedVideo();
        Advertisement.Initialize(unityAdsGameId);
        RequestInterstitial();
    }


    void AdMobShowVideo()
    {
        rewardBasedAdMobVideo.Show();   
    }

    void UnityAdsShowVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResultUnity;

        Advertisement.Show(unityAdsVideoPlacementId, options);
    }

    void HandleShowResultUnity(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            rewardPlayer = true;
            VideoRewarded();
            Advertisement.Initialize(unityAdsGameId);
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");
            if (!rewardPlayer)
                VideoRewardCanceled();
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
            VideoRewardNotReady();
        }
    }

    bool isVideoAvaiable()
    {
        Debug.Log("isVideoAvailable");
        #if !UNITY_EDITOR
        if(Advertisement.IsReady(unityAdsVideoPlacementId))
        {
        return true;
        }
        else if(rewardBasedAdMobVideo.IsLoaded())
        {
        return true;
        }
        //        #else
        //        return true; // Test.
        #endif
        return false;
    }

    void VideoRewardIsReady()
    {
        Debug.Log("VIDEO IS READY");
        ShowVideoReward();
        bPlayVideoReward = false;
        Camera.main.SendMessage("SetWatchVideoButtons" ,SendMessageOptions.DontRequireReceiver);    
        bVideoRewardReady = true;
    }

    void VideoRewardNotReady()
    {
        Debug.Log("VIDEO IS NOT READY");
        unlockingInProgress = false;
        bVideoRewardReady = false;
        Camera.main.SendMessage("SetWatchVideoButtons" ,SendMessageOptions.DontRequireReceiver);
        CantWatchPopUp();
    }

    void VideoRewardCanceled()
    {
        unlockingInProgress = false;
    }

    public void VideoRewarded()
    {
        Debug.Log("REWARDED ");
        unlockingInProgress = false;
        //...

        Shop.Instance.FinishWatchingVideo(true);

        //...
        rewardPlayer = false;
    }

    /***********/
    public void CantWatchPopUp( )
    {
        Debug.Log ("CANT WATCH VIDEO!");
        GameObject msgObject=GameObject.Find("Message_VideoUnavailable");
        if(msgObject==null) {Debug.Log ("object is null!");return;}
        CanvasGroup canvasGroup_msgUnavailable = /*GameObject.Find("Message_VideoUnavailable")*/msgObject.GetComponent<CanvasGroup>();
        if(canvasGroup_msgUnavailable==null) {Debug.Log ("canvas group is null!");return;}
//        canvasGroup_msgUnavailable.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text="This video is not available at this moment. Please try again later.";
        canvasGroup_msgUnavailable.interactable = true;
        canvasGroup_msgUnavailable.blocksRaycasts = true;
        StartCoroutine(ShowMsgUnavailable(canvasGroup_msgUnavailable));

    }

    IEnumerator ShowMsgUnavailable(CanvasGroup canvasGroup_msgUnavailable)
    {
        if(canvasGroup_msgUnavailable!=null) 
        {
            for(float i = 0;i<=1;i+=0.1f)
            {
                canvasGroup_msgUnavailable.alpha = i;
                yield return new WaitForSeconds(0.03f);
            }
            canvasGroup_msgUnavailable.alpha = 1;
            StartCoroutine(CloseMsgUnavailable(canvasGroup_msgUnavailable));
        }
        else
            Debug.LogError("ShowMsgUnavailable MSG OBJEKAT NIJE NADJEN!");
    }

    IEnumerator CloseMsgUnavailable(CanvasGroup canvasGroup_msgUnavailable)
    {
        yield return new WaitForSeconds(4);
        if(canvasGroup_msgUnavailable!=null) 
        {
            for(float i = 1;i>=0;i-=0.1f)
            {
                canvasGroup_msgUnavailable.alpha = i;
                yield return new WaitForSeconds(0.03f);
            }
            canvasGroup_msgUnavailable.interactable = false;
            canvasGroup_msgUnavailable.blocksRaycasts = false;
            canvasGroup_msgUnavailable.alpha = 0;
        }
        else
            Debug.LogError("CloseMsgUnavailable MSG OBJEKAT NIJE NADJEN!");
    }
}