using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FTR_Devices;

namespace Main_Template
{
    public class Device_Manager
    {
        //private Dictionary<int, Cam_ab> cam = new Dictionary<int, Cam_ab>();
        //public Dictionary<int, DSP_ab> dsp = new Dictionary<int, DSP_ab>();
        //public Dictionary<int, Dply_ab> dply = new Dictionary<int, Dply_ab>();


        public Device_Manager()
        {
            //AddCam(1, new Cam_Panasonic_HE("Cam01", "192.168.1.121", ComType.TCP));
            //AddCam(2, new Cam_Panasonic_HE("Cam02", "192.168.1.122", ComType.TCP));
            //AddCam(3, new Cam_Panasonic_HE("Cam03", "192.168.1.123", ComType.TCP));

            // AddDSP(); ...
            // AddDply(); ...
        }

        /*
        private void AddCam(int i, Cam_ab device)
        {
            cam.Add(i, device);
            cam[i].device_index = 2;
            cam[i].join_offset = (int)(cam[i].joins_used * (cam[i].device_index - 1));
        }
        */
        private void AddDSP()
        {

        }

        private void AddDply()
        {

        }
    }
}
