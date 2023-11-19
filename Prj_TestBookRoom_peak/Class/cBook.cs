using Prj_TestBookRoom_peak.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Prj_TestBookRoom_peak.Class
{
    public class cBook
    {
        //---------------------------------
        //Cmd: Function การตรวจสอบห้องพักว่ามีสถานะ พักหรือไม่ ?
        //Para: ข้อมูล Text array ,Ref ชื่อลูกค้าที่ Book กรณี สถานะห้องมีการ Book
        //Ret: True=สถานะห้องมีการ Book   False : สถานะห้องไม่มีการ Book
        //---------------------------------
        public bool C_CHKbExitsByRoomId(string ptRoomId, List<cmlBook> pLsBook,ref string ptCustBook)
        {
            bool bResult=false;
            var oBook = pLsBook.Where(x => x.FTRoomId == ptRoomId && x.FTBookSta == "0").FirstOrDefault();
            if (oBook != null)
            {
                if (!string.IsNullOrEmpty(oBook.FTRoomId))
                    bResult = true;
                ptCustBook = oBook.FTCustName;
            } else { ptCustBook = ""; }
            return bResult;
        }

        //---------------------------------
        //Cmd: Function การทำ Booking เข้าพักด้วยเงื่อนไข หมายเลขห้องพัก
        //Para: ข้อมูล Text array,Ref model Room,Ref model Book,Ref model Keycard
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_PRCtBook(string[] paVal ,ref List<cmlRoom> pLsRoom ,ref List<cmlBook> pLsBook,ref List<cmlKeycard> pCmlKeycard) {
            string tResult = "";
            string tCustBook = "";
            if (paVal!=null)
            {
                if (!C_CHKbExitsByRoomId(paVal[1].ToString(), pLsBook,ref tCustBook))
                {

                    var oRoom = pLsRoom.Where(x => x.FTRoomId == paVal[1].ToString() && x.FTRoomSta == "0").FirstOrDefault();
                    if (oRoom != null)
                    {
                        int nKeycard = C_GETnKeycardBlank( pCmlKeycard);
                        int nRunning = pLsBook.Count + 1;
                        pLsBook.Add(new cmlBook
                        {
                            FNBookId = nRunning,
                            FTRoomId = oRoom.FTRoomId,
                            FNKeycard = nKeycard,
                            FTCustName = paVal[2].ToString(),
                            FTBookSta = "0", //0 : check/Book
                            FNCustAge = Convert.ToInt32(paVal[3].ToString())
                        }); ;
                        //set masster
                        C_SETbRoom(oRoom.FTRoomId, "1", ref pLsRoom);
                        C_SETbKeycard(nKeycard, "1", ref pCmlKeycard);
                        //set resualt text
                        tResult = string.Format(@"Room {0} is booked by {1} with keycard number {2}.", oRoom.FTRoomId, paVal[2].ToString(), nKeycard);
                    }
                    else
                    {
                        tResult = string.Format(@"Cannot book room {0} for {1}, The room is currently booked by {2}.", paVal[1].ToString(), paVal[2].ToString(), tCustBook);
                    }
                } else
                {
                    tResult = string.Format(@"Cannot book room {0} for {1}, The room is currently booked by {2}.", paVal[1].ToString(), paVal[2].ToString(), tCustBook);
                }
            }
            return tResult;  
        }

        //---------------------------------
        //Cmd: Function การทำ Booking เข้าพักด้วยเงื่อนไข Floor
        //Para: ข้อมูล Text array,Ref model Room,Ref model Book,Ref model Keycard
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_PRCtBookByFloor(string[] paVal,cmlHotel pCmlHotel, ref List<cmlRoom> pLsRoom, ref List<cmlBook> pLsBook, ref List<cmlKeycard> pCmlKeycard)
        {
            string tResult = "";
            string tResult1 = "";
            string tResult2 = "";
            if (paVal != null)
            {
                List<cmlRoom> oLsRoom = pLsRoom.Where(x => x.FTRoomId.Substring(0, 1) == paVal[1].ToString() && x.FTRoomSta == "0" ).ToList();
                if (oLsRoom != null)
                {
                    if(oLsRoom.Count>0 && oLsRoom.Count== pCmlHotel.FNRoom)
                    {
                        foreach (cmlRoom oRoom in oLsRoom)
                        {
                            int nKeycard = C_GETnKeycardBlank( pCmlKeycard);
                            int nRunning = pLsBook.Count + 1;
                            pLsBook.Add(new cmlBook
                            {
                                FNBookId = nKeycard, //card id
                                FTRoomId = oRoom.FTRoomId,
                                FNKeycard = nKeycard,
                                FTCustName = paVal[2].ToString(),
                                FTBookSta = "0", //Checkin/Book
                                FNCustAge = Convert.ToInt32(paVal[3].ToString())
                            });
                            //set masster
                            C_SETbRoom(oRoom.FTRoomId,"1",ref pLsRoom);
                            C_SETbKeycard(nKeycard, "1", ref pCmlKeycard);
                            //set result text
                            if (string.IsNullOrEmpty(tResult1))
                            {
                                tResult1 = oRoom.FTRoomId;
                                tResult2 = nKeycard.ToString();
                            }
                            else
                            {
                                tResult1 += "," + oRoom.FTRoomId;
                                tResult2 += "," +  nKeycard.ToString();
                            }
                        }
                        tResult = string.Format(@"Room {0} are booked with keycard number {1}", tResult1, tResult2);
                    } else
                    {
                        tResult = string.Format(@"Cannot book floor {0} for {1}.", paVal[1].ToString(), paVal[2].ToString());
                    }                  
                } else
                {
                    tResult = string.Format(@"Cannot book floor {0} for {1}.", paVal[1].ToString(), paVal[2].ToString());
                }
            }
            return tResult;
        }

        //---------------------------------
        //Cmd: Function การทำ Check out
        //Para: ข้อมูล Text array,Ref model Room,Ref model Book,Ref model Keycard
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_PRCtCheckOut(string[] paVal,ref List<cmlRoom> pLsRoom, ref List<cmlBook> pLsBook, ref List<cmlKeycard> pCmlKeycard)
        {
            string tResult = "";
            if (paVal != null)
            {
                int nKeycard =Convert.ToInt32( paVal[1].ToString());
                var oBook = pLsBook.Where(x => x.FNKeycard == nKeycard && x.FTBookSta == "0").FirstOrDefault();
                if (oBook != null)
                {
                    if(oBook.FTCustName == paVal[2].ToString())
                    {
                        string tRoomId = oBook.FTRoomId;
                        var oRoom = pLsRoom.Where(x => x.FTRoomId == tRoomId && x.FTRoomSta == "1").FirstOrDefault();
                        if (oRoom != null)
                        {
                            //set book
                            C_SETbBook(oBook.FNBookId, "1", ref pLsBook);
                            //set masster
                            C_SETbRoom(tRoomId, "0", ref pLsRoom);
                            C_SETbKeycard(nKeycard, "0", ref pCmlKeycard);
                            //Room 201 is checkout.
                            tResult = string.Format(@"Room {0} is checkout.", tRoomId);
                        }
                        else
                        {
                            tResult = string.Format(@"Cannot checkout Keycard {0} for {1}.", nKeycard, paVal[2].ToString());
                        }
                    } else
                    {
                        tResult = string.Format(@"Only {0} can checkout with keycard number {1}.", oBook.FTCustName, oBook.FNKeycard);
                    }
                }
                else
                {
                    tResult = string.Format(@"Cannot checkout Keycard {0} for {1}.", nKeycard, paVal[2].ToString());
                }
            }
            return tResult;
        }

        //---------------------------------
        //Cmd: Function การทำ Check out ทั้ง Floor
        //Para: ข้อมูล Text array,Ref model Room,Ref model Book,Ref model Keycard
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_PRCtCheckOutByFloor(string[] paVal, ref List<cmlRoom> pLsRoom,ref List<cmlBook> pLsBook, ref List<cmlKeycard> pCmlKeycard)
        {
            string tResult = "";
            try
            {
                if (pLsBook != null)
                {
                    int nFloor = Convert.ToInt32(paVal[1]);
                    foreach (cmlBook oRoom in pLsBook.Where(x => x.FTRoomId.Substring(0, 1) == nFloor.ToString() && x.FTBookSta == "0"))
                    {
                        if (oRoom != null)
                        {
                            //set book
                            C_SETbBook(oRoom.FNBookId, "1", ref pLsBook);
                            //set masster
                            C_SETbRoom(oRoom.FTRoomId, "0", ref pLsRoom);
                            C_SETbKeycard(oRoom.FNKeycard, "0", ref pCmlKeycard);
                            if (string.IsNullOrEmpty(tResult))
                            {
                                tResult = oRoom.FTRoomId;
                            }
                            else
                            {
                                tResult += "," + oRoom.FTRoomId;
                            }
                        }
                    }
                }
                return string.Format(@"Room {0} are checkout.", tResult);
            }
            catch
            {
                return @"not found";
            }
            finally
            {

            }
        }

        //---------------------------------
        //Cmd: Function การหา Keycard Number ที่ยังไม่ได้ใช้งาน  แค่ 1 Number โดยเงื่อนไขเรียงจากน้อย > มาก
        //Para: Ref model Keycard
        //Ret: Resualt int Keycard Number
        //---------------------------------
        public int C_GETnKeycardBlank( List<cmlKeycard> pCmlKeycard) 
        {
            int nResult= 0;
            var oKeycard = pCmlKeycard.Where(x => x.FTKeycardSta == "0").FirstOrDefault();
            if (oKeycard != null)
            {
                nResult = oKeycard.FNKeycard;
            }
            return nResult;
        }

        //---------------------------------
        //Cmd: Function การทำ Set สถานะ Model Book
        //Para: ข้อมูล Text array,Ref model Book
        //Ret: Resualt bool True = Success 
        //---------------------------------
        public bool C_SETbBook(int pnBookId, string ptSta, ref List<cmlBook> pCmlBook)
        {
            //filter and Edit
            pCmlBook.Where(x => x.FNBookId == pnBookId).ToList()
                   .ForEach(iTem =>
                   {
                       iTem.FTBookSta = ptSta; // 0 : Book  1 : Checkout
                   }
            );
            return true;
        }

        //---------------------------------
        //Cmd: Function การทำ Set สถานะ Model Keycard
        //Para: ข้อมูล Text array,Ref model Keycard
        //Ret: Resualt bool True = Success 
        //---------------------------------
        public bool C_SETbKeycard(int pnKeycard,string ptSta, ref List<cmlKeycard> pCmlKeycard)
        {
            //filter and Edit
            pCmlKeycard.Where(x => x.FNKeycard == pnKeycard).ToList()
                   .ForEach(iTem =>
                   {
                       iTem.FTKeycardSta = ptSta; // 0 : ไม่ได้ใช้งาน  1 : ใช้งาน
                   }
            );
            return true;
        }

        //---------------------------------
        //Cmd: Function การทำ Set สถานะ Model Room
        //Para: ข้อมูล Text array,Ref model Room
        //Ret: Resualt bool True = Success 
        //---------------------------------
        public bool C_SETbRoom(string ptRoomId, string ptSta, ref List<cmlRoom> pLsRoom)
        {
            //filter and Edit
            pLsRoom.Where(x => x.FTRoomId == ptRoomId ).ToList()
                .ForEach(iTem =>
                {
                    iTem.FTRoomSta = ptSta; // 0 : ว่าง  1 : ไม่ว่าง  
                }
            );
            return true;
        }

    }
}
