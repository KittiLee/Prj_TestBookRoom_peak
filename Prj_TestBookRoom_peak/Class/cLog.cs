using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Prj_TestBookRoom_peak.Class
{
    public class cLog
    {
        public  string tC_PthFleOutput { get; set; }  //Pk
        public cLog()
            {
                try
                {
                     tC_PthFleOutput = Environment.CurrentDirectory.ToString() + @"\output" + ".txt";
                 }
                catch (Exception oEx)
                {
                    C_WRTxOutput( oEx.Message);
                }
            }

            public void C_WRTxClear()
            {
                //string tPath = Environment.CurrentDirectory.ToString() + @"\output" + ".txt";
                if (File.Exists(tC_PthFleOutput))
                    File.Delete(tC_PthFleOutput);  
            }


            public void C_WRTxOutput(string ptMsg)
            {
                //string tPath;

                try
                {            
                    //tPath = Environment.CurrentDirectory.ToString() + @"\output"  + ".txt";
                    if (!File.Exists(tC_PthFleOutput))
                        File.Create(tC_PthFleOutput).Dispose();

                    using (StreamWriter oOutputFile = new StreamWriter(tC_PthFleOutput, true, System.Text.Encoding.UTF8))
                    {
                        oOutputFile.WriteLine(ptMsg);
                        oOutputFile.Dispose();
                    }
                }
                catch (Exception oEx) { Debug.WriteLine(oEx.ToString()); }
                finally
                {
                    //tPath = null;
                    ptMsg = null;
                }
            }
     
            
        }
    
}
