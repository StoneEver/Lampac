﻿using Shared.Model.Base;

namespace Shared.Model.Online.Settings
{
    public class CollapsSettings : BaseSettings
    {
        public CollapsSettings(string host, bool enable = true, bool streamproxy = false, bool two = false)
        {
            this.enable = enable;
            this.streamproxy = streamproxy;
            this.two = two;

            if (host != null)
                this.host = host.StartsWith("http") ? host : Decrypt(host);
        }


        public bool two { get; set; }

        public bool dash { get; set; }


        public CollapsSettings Clone()
        {
            return (CollapsSettings)MemberwiseClone();
        }
    }
}
