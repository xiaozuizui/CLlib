using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCL.Net;


namespace CLlib
{
    public class clinfo
    {
        ErrorCode err { get; set; }
        Platform[] platforms { get; set; }
        Device[]  device { get; set; }
        int num_of_platforms { get; set; }
        public clinfo()
        {
            device = new Device[1];
            err = ErrorCode.Unknown;
            platforms = new Platform[3];
        }
        
        public void Run()
        {
            ErrorCode e = ErrorCode.Unknown;
            uint num = 0;
            err = Cl.GetPlatformIDs(1, platforms, out num);
            InfoBuffer buf =  Cl.GetPlatformInfo(platforms[0], PlatformInfo.Name,out e);

           device =   Cl.GetDeviceIDs(platforms[0], DeviceType.Gpu, out e);
            buf = Cl.GetDeviceInfo(device[0], DeviceInfo.Name, out e);
        }
    }
}
