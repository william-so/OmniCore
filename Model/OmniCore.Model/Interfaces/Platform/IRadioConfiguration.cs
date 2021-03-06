﻿using System;
using OmniCore.Model.Enumerations;

namespace OmniCore.Model.Interfaces.Platform
{
    public interface IRadioConfiguration
    {
        bool KeepConnected { get; set; }
        TimeSpan RadioResponseTimeout { get; set; }
        TimeSpan RadioConnectTimeout { get; set; }
        TimeSpan? RssiUpdateInterval { get; set; }
        int FrequencyShift { get; set; }
        TxPower Amplification { get; set; }
        int RxIntermediateFrequency { get; set; }
        int PqeThreshold { get; set; }
        int FilterBWExponent { get; set; }
        int FilterBWDecimationRatio { get; set; }
        bool DataWhitening { get; set; }
        bool PreambleCheckWithCarrierSense { get; set; }
        int TxPreambleCountSetting { get; set; }
        bool ForwardErrorCorrection { get; set; }
        int RxAttenuationLevel { get; set; }
    }
}
