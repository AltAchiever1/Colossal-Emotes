// This is not my script it was given to me by a friend long time ago i just fixed it

using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Text;
using Photon.Voice.Unity;
using UnityEngine;

namespace ColossalEmotes.Patches
{
    internal class VCbanbypass
    {
        private static float silenceStartTime = -1f;
        private static float lastVolume;
        private static bool hasRestartedRecorder;

        public static void VCBanBypass()
        {
            GorillaTagger.moderationMutedTime = -1f;
            if (GorillaComputer.instance.autoMuteType != "OFF")
            {
                GorillaComputer.instance.autoMuteType = "OFF";
                PlayerPrefs.SetInt("autoMute", 0);
                PlayerPrefs.Save();
            }
            Recorder mic = NetworkSystem.Instance.VoiceConnection.PrimaryRecorder;
            if (mic == null)
            { return; }
            if (mic.SourceType == Recorder.InputSourceType.AudioClip)
            { return; }
            float volume = 0f;
            GorillaSpeakerLoudness recorder = VRRig.LocalRig.GetComponent<GorillaSpeakerLoudness>();
            if (recorder != null)
            { volume = recorder.Loudness; }
            if (volume == 0f)
            {
                if (lastVolume != 0f)
                { silenceStartTime = Time.time; 
                 hasRestartedRecorder = false; }
                if (silenceStartTime > 0f && !hasRestartedRecorder && Time.time - silenceStartTime >= 0.25f)
                { mic.RestartRecording(true);
                hasRestartedRecorder = true; }
            }
            else
            { silenceStartTime = -1f;
            hasRestartedRecorder = false; }
            lastVolume = volume;
        }
    }
}
