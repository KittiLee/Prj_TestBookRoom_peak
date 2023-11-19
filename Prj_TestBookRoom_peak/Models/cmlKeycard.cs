using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj_TestBookRoom_peak.Models
{
    public class cmlKeycard
    {
        //Class Model เก็บข้อมูล Keycard
        public int FNKeycard { get; set; } //Pk
        public string FTKeycardSta { get; set; } //0 : ไม่ใช้งาน  1: ใช้งาน
    }
}
