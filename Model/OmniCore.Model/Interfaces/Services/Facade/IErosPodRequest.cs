﻿using OmniCore.Model.Enumerations;

namespace OmniCore.Model.Interfaces.Services.Facade
{
    public interface IErosPodRequest : IPodRequest
    {
        IErosPod ErosPod { get; }
        byte[] Message { get; }
        uint MessageRadioAddress { get; }
        int MessageSequence { get; }
        bool WithCriticalFollowup { get; }
        bool AllowAddressOverride { get; }
        TransmissionPower? TransmissionPowerOverride { get; }
    }
}