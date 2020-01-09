﻿using System;
using System.Collections.Generic;
using System.Text;
using OmniCore.Model.Interfaces.Common;

namespace OmniCore.Client.Models
{
    public class PodModel
    {
        public readonly IPod Pod;
        public PodModel(IPod pod)
        {
            Pod = pod;
        }
    }
}