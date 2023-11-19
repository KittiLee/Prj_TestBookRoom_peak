using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj_TestBookRoom_peak.Models
{
    public class cmlBook
    {
        public int FNBookId { get; set; }  //Pk
        public string FTRoomId { get; set; } //ref key
        public int FNKeycard { get; set; } //ref key
        public string FTCustName { get; set; } 
        public string FTBookSta { get; set; } //0 : Checkin/Book  1: Checkout
        public int FNCustAge { get; set; }
    }
}
