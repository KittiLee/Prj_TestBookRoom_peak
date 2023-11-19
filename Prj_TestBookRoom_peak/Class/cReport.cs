using Prj_TestBookRoom_peak.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Prj_TestBookRoom_peak.Class
{
    public class cReport
    {

        //---------------------------------
        //Cmd: Procedure การ Write output result message on console
        //Para: ข้อมูล Text List 
        //Ret: -
        //---------------------------------
        public void C_SHWxWriteConsose(List<string> pLsMsg)
        {
            if (pLsMsg.Count > 0) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(@"Output >>>>>",ConsoleColor.Green);
                foreach (string tMsg in pLsMsg)
                {
                    Console.WriteLine(tMsg);
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(string.Format(@"Path file output: {0}",new cLog().tC_PthFleOutput));
         
            } else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"Output no data !");
                Console.ForegroundColor = ConsoleColor.White;
            } 
        }

        //---------------------------------
        //Cmd: Function หาข้อมูลห้องว่างที่สามารถ Book เข้าพักได้ (ดูรายการห้องว่างทั้งหมดได้)
        //Para: model Room 
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_LSTtAvailableRoom( List<cmlRoom> pLsRoom) {
            string tResult = "";
            try
            {
                if(pLsRoom != null)
                {
                    foreach (cmlRoom oRoom in pLsRoom.Where(x => x.FTRoomSta == "0"))
                    {
                        if(oRoom != null)
                        {
                            if(string.IsNullOrEmpty(tResult))
                            {
                                tResult = oRoom.FTRoomId;
                            } else
                            {
                                tResult += "," + oRoom.FTRoomId;
                            }
                        }
                    }
                }
                return tResult;
            }
            catch {
                return tResult;
            }
            finally {
                
            }            
        }
        //---------------------------------
        //Cmd: Function หาข้อมูลรายชื่อลูกค้าที่เข้าพักห้องสถานะ Book ( ดูรายชื่อแขกทั้งหมดได้)
        //Para: model Book 
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_LSTtGuest( List<cmlBook> pLsBook)
        {
            string tResult = "";
            try
            {
                if (pLsBook != null)
                {
                    foreach (cmlBook oRoom in pLsBook.Where(x => x.FTBookSta == "0"))
                    {
                        if (oRoom != null)
                        {
                            if (string.IsNullOrEmpty(tResult))
                            {
                                tResult = oRoom.FTCustName;
                            }
                            else
                            {

                                tResult += "," + oRoom.FTCustName;
                            }
                        }
                    }
                }
                return tResult;
            }
            catch
            {
                return tResult;
            }
            finally
            {

            }
        }
        //---------------------------------
        //Cmd: Function หาข้อมูลรายชื่อลูกค้าที่เข้าพักห้องสถานะ Book ตามเงื่อนไขหมายเลขห้อง  ( ดูชื่อแขกในห้องพักที่ระบุได้)
        //Para: model Book 
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_LSTtGuestByRoom(string[] paVal, List<cmlBook> pLsBook)
        {
            string tResult = "";
            try
            {
                if (pLsBook != null)
                {
                    foreach (cmlBook oRoom in pLsBook.Where(x => x.FTBookSta == "0" && x.FTRoomId == paVal[1].ToString()))
                    {
                        if (oRoom != null)
                        {
                            if (string.IsNullOrEmpty(tResult))
                            {
                                tResult = oRoom.FTCustName;
                            }
                            else
                            {
                                tResult += "," + oRoom.FTCustName;
                            }
                        }
                    }
                }
                return tResult;
            }
            catch
            {
                return tResult;
            }
            finally
            {

            }
        }
        //---------------------------------
        //Cmd: Function หาข้อมูลรายชื่อลูกค้าที่เข้าพักห้องสถานะ Book ตามเงื่อนไขอายุ  ( ดูรายชื่อแขกโดยกำหนดช่วงอายุได้)
        //Para: model Book 
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_LSTtGuestByAge(string[] paVal,  List<cmlBook> pLsBook)
        {
            string tResult = "";
            
            try
            {
                if (pLsBook != null)
                {
                    if (paVal != null)
                    {
                        int nAge = Convert.ToInt32(paVal[2]);
                        List<cmlBook> oCmlRoom = new List<cmlBook>();
                        switch (paVal[1])
                        {
                            case @"<":
                                oCmlRoom = pLsBook.Where(x => x.FNCustAge < nAge).ToList();
                                break;
                            case @"<=":
                                oCmlRoom = pLsBook.Where(x => x.FNCustAge <= nAge).ToList();
                                break;
                            case @">":
                                oCmlRoom = pLsBook.Where(x => x.FNCustAge > nAge).ToList();
                                break;
                            case @">=":
                                oCmlRoom = pLsBook.Where(x => x.FNCustAge >= nAge).ToList();
                                break;
                            case @"=":
                                oCmlRoom = pLsBook.Where(x => x.FNCustAge == nAge).ToList();
                                break;
                        }
                        foreach (cmlBook oRoom in oCmlRoom)
                        {
                            if (oRoom != null)
                            {
                                if (string.IsNullOrEmpty(tResult))
                                {
                                    tResult = oRoom.FTCustName;
                                }
                                else
                                {
                                    tResult += "," + oRoom.FTCustName;
                                }
                            }
                        }
                    }

                }
                return tResult;
            }
            catch
            {
                return tResult;
            }
            finally
            {

            }
        }
        //---------------------------------
        //Cmd: Function หาข้อมูลรายชื่อลูกค้าที่เข้าพักห้องสถานะ Book ตามเงื่อนไข Floor  ( ดูรายชื่อแขกโดยกำหนดชั้นที่พักได้)
        //Para: model Book 
        //Ret: Resualt string to write output file
        //---------------------------------
        public string C_LSTtGuestByFloor(string[] paVal,  List<cmlBook> pLsBook)
        {
            string tResult = "";
            try
            {
                if (pLsBook != null)
                {
                    int nFloor = Convert.ToInt32(paVal[1]);
                    foreach (cmlBook oRoom in pLsBook.Where(x => x.FTRoomId.Substring(0,1) == nFloor.ToString() && x.FTBookSta=="0"))
                    {
                        if (oRoom != null)
                        {
                            if (string.IsNullOrEmpty(tResult))
                            {
                                tResult = oRoom.FTCustName;
                            }
                            else
                            {
                                tResult += "," + oRoom.FTCustName;
                            }
                        }
                    }
                }
                return tResult;
            }
            catch
            {
                return tResult;
            }
            finally
            {

            }
        }

        

    }
}
