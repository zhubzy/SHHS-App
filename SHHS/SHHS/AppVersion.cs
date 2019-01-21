using System;
namespace SHHS
{
    public interface IAppVersionProvider
    {
       
            string ApplicationsPublicVersion { get; set; }
            string ApplicationsPrivateVersion { get; set; }
        
    }
}
