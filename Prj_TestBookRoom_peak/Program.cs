using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Prj_TestBookRoom_peak.Class;
using Prj_TestBookRoom_peak;
using static System.Net.Mime.MediaTypeNames;

namespace Prj_TestBookRoom_peak
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // รับที่อยู่ของไดเรกทอรีที่ exe ทำงานอยู่
            string tBaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // ระบุชื่อไฟล์ที่ต้องการอ่าน
            string tFileName = "input.txt";
            bool bExit = false; //
            // รวมที่อยู่ของไดเรกทอรีและชื่อไฟล์
            string tFilePath = Path.Combine(tBaseDirectory, tFileName);
            //string tFilePath = "ตัวอย่าง.txt";
            if (!File.Exists(tFilePath))
            {
                tFilePath = Path.GetDirectoryName(tFilePath);
                if (File.Exists(tFilePath))
                {
                    bExit = true;
                }
            } else
            {
                bExit = true;
            }                
            cRoom oRoom = new cRoom();
            cBook oBook = new cBook();
            cReport oReport = new cReport();
            List<string> lsMsgOutput = new List<string>();
            try
            {
                if (bExit)
                {
                    Console.WriteLine(string.Format(@"Path file input: {0}", tFilePath));
                    Console.WriteLine(@"Input >>>>> ");
                    // ใช้ StreamReader เพื่ออ่านไฟล์
                    using (StreamReader reader = new StreamReader(tFilePath))
                    {
                        string tPntOut = "";
                        new cLog().C_WRTxClear();
                        // อ่านไฟล์ทีละบรรทัดจนกว่าจะถึงท้ายไฟล์
                        while (!reader.EndOfStream)
                        {
                            // อ่านบรรทัดถัดไป
                            string tLine = reader.ReadLine();

                            // ทำตามกระบวนการที่ต้องการด้วยข้อมูลในบรรทัด
                            Console.WriteLine(tLine);
                            if (!string.IsNullOrEmpty(tLine))
                            {
                                string[] aLine = tLine.Split(' ');
                                switch (aLine[0])
                                {
                                    case @"create_hotel":
                                        oRoom.C_PRCxCreateRoom(aLine, ref cVar.oCN_Hotel);
                                        cVar.oCN_Room = oRoom.C_PRCoRunningRoomId(cVar.oCN_Hotel, ref cVar.oCN_Keycard);
                                        lsMsgOutput.Add(string.Format(@"Hotel created with {0} floor(s), {1} room(s) per floor.", cVar.oCN_Hotel.FNFloor, cVar.oCN_Hotel.FNRoom));
                                        new cLog().C_WRTxOutput(string.Format(@"Hotel created with {0} floor(s), {1} room(s) per floor.", cVar.oCN_Hotel.FNFloor, cVar.oCN_Hotel.FNRoom));
                                        break;
                                    case @"book":
                                        tPntOut = oBook.C_PRCtBook(aLine, ref cVar.oCN_Room, ref cVar.oCN_Book, ref cVar.oCN_Keycard);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                    case @"list_available_rooms": //ok
                                        tPntOut = oReport.C_LSTtAvailableRoom(cVar.oCN_Room);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                    case @"checkout": //ok
                                        tPntOut = oBook.C_PRCtCheckOut(aLine, ref cVar.oCN_Room, ref cVar.oCN_Book, ref cVar.oCN_Keycard);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                    case @"list_guest": //ok
                                        tPntOut = oReport.C_LSTtGuest(cVar.oCN_Book);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                    case @"get_guest_in_room": //ok
                                        tPntOut = oReport.C_LSTtGuestByRoom(aLine, cVar.oCN_Book);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                    case @"list_guest_by_age": //ok
                                        tPntOut = oReport.C_LSTtGuestByAge(aLine, cVar.oCN_Book);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                    case @"list_guest_by_floor": //ok
                                        tPntOut = oReport.C_LSTtGuestByFloor(aLine, cVar.oCN_Book);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                    case @"checkout_guest_by_floor": //ok
                                        tPntOut = oBook.C_PRCtCheckOutByFloor(aLine, ref cVar.oCN_Room,ref cVar.oCN_Book, ref cVar.oCN_Keycard);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                    case @"book_by_floor": //ok
                                        tPntOut = oBook.C_PRCtBookByFloor(aLine,cVar.oCN_Hotel, ref cVar.oCN_Room, ref cVar.oCN_Book, ref cVar.oCN_Keycard);
                                        lsMsgOutput.Add(string.Format(@"{0}", tPntOut));
                                        new cLog().C_WRTxOutput(string.Format(@"{0}", tPntOut));
                                        break;
                                }
                            }

                        }
                    }
                } else { Console.WriteLine("Not found file input " + tFilePath); }

            }
            catch (Exception ex)
            {
                // จัดการข้อผิดพลาดที่เกิดขึ้น
                Console.WriteLine("เกิดข้อผิดพลาด: " + ex.Message);
            }
            finally
            {
                oReport.C_SHWxWriteConsose(lsMsgOutput);

                new cFile().C_OPENxFileWithNotepad(new cLog().tC_PthFleOutput);
                Console.WriteLine("<Press Any Key to Continue.>");
                Console.ReadKey();
                oRoom=null; oBook=null;oReport=null;
                Environment.Exit(0);
            }
        }


    }
}
