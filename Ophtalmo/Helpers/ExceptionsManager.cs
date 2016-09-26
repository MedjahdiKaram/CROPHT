using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ophtalmo.Properties;


namespace Ophtalmo.Helpers
{
    public static class ExceptionsManager
    {


        public static void SaveException(this Exception exception)
        {
            string logFileName = @"log\" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year+".log";
            var date = DateTime.Now;
            var frame = new StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;
            var msg = exception.Message;
            var inner = exception.InnerException;
            var data = exception.Data;
            File.AppendAllText(logFileName, @"______________________________________________________________________________"+"\r\n");
            File.AppendAllText(logFileName,Resources.DateandTme+":"+date+"\r\n");
            File.AppendAllText(logFileName, "Class :" + type.ToString() + "\r\n");
            File.AppendAllText(logFileName, "Caller :" + method.ToString() + "\r\n");
            File.AppendAllText(logFileName, "Message :" + msg + "\r\n");
            File.AppendAllText(logFileName, "Inner error :" + inner + "\r\n");
            File.AppendAllText(logFileName, "Additional data :" + data + "\r\n");
            File.AppendAllText(logFileName, @"______________________________________________________________________________" + "\r\n\r\n");
            Debug.WriteLine(method+" "+exception.ToString());
        }

    }
}
