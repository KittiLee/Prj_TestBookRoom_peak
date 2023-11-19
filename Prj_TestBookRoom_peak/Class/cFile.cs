using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prj_TestBookRoom_peak.Class
{
    public class cFile
    {
        public void C_OPENxFileWithNotepad(string pTFilePath)
        {
            ProcessStartInfo oPsi = new ProcessStartInfo
            {
                FileName = "notepad.exe",
                Arguments = pTFilePath,
                UseShellExecute = true,
                CreateNoWindow = true
            };

            Process.Start(oPsi);
        }
    }
}
