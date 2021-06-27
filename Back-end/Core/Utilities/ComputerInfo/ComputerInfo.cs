using Microsoft.Win32;
using System;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;

namespace Core.Utilities.ComputerInfo
{
    public  class ComputerInfo
    {
        public string ComputerName { get;}
        public string ComputerIPAdress { get;}
        public string ComputerMacAdress { get; }
        public string ComputerMacAdressNoBrackets { get; }
        public string ComputerCPUInfo { get; }
        public string ComputerOSInfo { get; }
        public ComputerInfo()
        {
            ComputerName= GetComputerName();
            ComputerIPAdress = GetComputerIPAdress();
            ComputerMacAdress = GetMAC();
            ComputerCPUInfo = GetCPU();
            ComputerOSInfo = GetOS();

        }
        private string GetComputerName()
        {
            return Dns.GetHostName();
            
        }


        private string GetComputerIPAdress()
        {
            var webClient = new WebClient();

           string dnsString = webClient.DownloadString("http://checkip.dyndns.org");
            dnsString = (new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b")).Match(dnsString).Value;

            webClient.Dispose();
            return dnsString;
        }
        private string GetMAC()
        {
            ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject obj in manager.GetInstances())
            {
                if ((bool)obj["IPEnabled"])
                {
                    return obj["MacAddress"].ToString();
                }
            }

            return String.Empty;
        }
        private string GetMACNoBrackets()
        {
            string mac = GetMAC().Replace(":", "");
            if (String.IsNullOrEmpty(mac))
            {
                return String.Empty;
            }
            else { return mac; }
        }
        private string GetCPUCoreCount()
        {
            string cpuCoreCount = Environment.ProcessorCount.ToString();
            return cpuCoreCount;
        }
        private string GetCPU()
        { 
            string processorInfo = null;
            string processorSerial = null;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * FROM WIN32_Processor");
            ManagementObjectCollection mObject = searcher.Get();

            foreach (ManagementObject obj in mObject)
            {
                processorSerial = obj["ProcessorId"].ToString();
            }
            processorInfo = processorSerial;

            return processorInfo;
        }
        private string GetOS()
        {
            return Environment.OSVersion.ToString();
        }
    }
}
