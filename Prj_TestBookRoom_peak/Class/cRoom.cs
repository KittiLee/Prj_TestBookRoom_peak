using Prj_TestBookRoom_peak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj_TestBookRoom_peak.Class
{
    public class cRoom
    {
        //---------------------------------
        //Cmd: Procedure การสร้างข้อมูลโรงแรม 
        //Para: ข้อมูล Text array,ref model Hotel จำนวนชั้น + จำนวนห้องแต่ละชั้น
        //Ret: -
        //---------------------------------
        public void C_PRCxCreateRoom(string[] paVal, ref cmlHotel poHotel) {
            cmlHotel ocmlHotel = new cmlHotel
            {
                FNFloor = Convert.ToInt16(paVal[1]),
                FNRoom = Convert.ToInt16(paVal[2])
            };
            poHotel = ocmlHotel;
        }
        //---------------------------------
        //Cmd: Procedure การสร้างข้อมูลห้องพัก + Keycard  จาก Model Hotel 
        //Para: model Hotel จำนวนชั้น + จำนวนห้องแต่ละชั้น ,Ref Model keycard
        //Ret: Initial Model Room ห้องพักทั้งหมด
        //---------------------------------
        public List<cmlRoom> C_PRCoRunningRoomId(cmlHotel pCmlHotel ,ref  List<cmlKeycard> pCmlKeycard)
        {
            string tId = "";
            int nCount = 0;
            List<cmlRoom> olsRoom = new List<cmlRoom>();
            if (pCmlHotel != null)
            {
                for (int nI = 1; nI <= pCmlHotel.FNFloor; nI++)
                {
                    for (int nJ = 1; nJ <= pCmlHotel.FNRoom; nJ++)
                    {
                        tId = nI.ToString() + nJ.ToString("00");
                        olsRoom.Add(new cmlRoom
                        {
                            FTRoomId= tId,
                            FTRoomSta="0"
                        });

                        nCount += 1;
                        pCmlKeycard.Add(new cmlKeycard
                        {
                            FNKeycard = nCount,
                            FTKeycardSta="0" //0 : ไม่ได้ใช้งาน 1: ใช้งาน
                        }) ;

                    }
                }
            };
            return olsRoom;
        }

    }
}
