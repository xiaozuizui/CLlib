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
        Context context { get; set; }
        int num_of_platforms { get; set; }
        const string correctSource = @"
                // Simple test; c[i] = a[i] + b[i]

                __kernel void add_array(__global float *a, __global float *b, __global float *c)
                {
                    int xid = get_global_id(0);
                    c[xid] = a[xid] + b[xid];
                }
                
                __kernel void sub_array(__global float *a, __global float *b, __global float *c)
                {
                    int xid = get_global_id(0);
                    c[xid] = a[xid] - b[xid];
                }

                ";

        public clinfo()
        {
            context = new Context();
            device = new Device[1];
            err = ErrorCode.Unknown;
            platforms = new Platform[3];
        }
        
        public void Run()
        {
            ErrorCode e = ErrorCode.Unknown;
            uint num = 0;
            err = Cl.GetPlatformIDs(1, platforms, out num);
            InfoBuffer platformbuf =  Cl.GetPlatformInfo(platforms[0], PlatformInfo.Name,out e);

           device =   Cl.GetDeviceIDs(platforms[0], DeviceType.Gpu, out e);
           InfoBuffer devicebuf = Cl.GetDeviceInfo(device[0], DeviceInfo.Name, out e);

            IntPtr ptr = new IntPtr();
            IntPtr ptr2 = new IntPtr();
            context =  Cl.CreateContext(null, 1, new[] { device[0] }, null, ptr, out e);
            
            InfoBuffer contextbuff = new InfoBuffer();
            e =  Cl.GetContextInfo(context, ContextInfo.Devices,ptr,contextbuff,out ptr2);

            Program pg = Cl.CreateProgramWithSource(context, 1,new[] { correctSource }, null, out e);
            e = Cl.BuildProgram(pg, 1,new[] { device[0] }, string.Empty, null, IntPtr.Zero);

            InfoBuffer pgbuild =  Cl.GetProgramBuildInfo(pg, device[0], ProgramBuildInfo.Log,out e);
//            Cl.BuildProgram(pg,1,device,"-cl-std=CL")
        }
    }
}
