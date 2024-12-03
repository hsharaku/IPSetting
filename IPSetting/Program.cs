using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace IPSetting
{
    internal class Program
    {
        static void Main()
        {
            // ネットワークインターフェース情報を取得
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                // インターフェース名を取得
                string interfaceName = networkInterface.Name;

                // MACアドレスを取得
                PhysicalAddress macAddress = networkInterface.GetPhysicalAddress();

                // MACアドレスを16進数形式で表示
                string macAddressString = BitConverter.ToString(macAddress.GetAddressBytes()).Replace("-", ":");

                if (macAddressString == "6C:02:E0:4E:EA:64")
                {
                    Console.WriteLine($"インターフェース名: {interfaceName}, MACアドレス: {macAddressString}");

                    string ipAddress = "192.168.1.10";  // 設定するIPアドレス
                    string subnetMask = "255.255.255.0";  // サブネットマスク
                    string gateway = "192.168.1.1";  // ゲートウェイ

                    SetStaticIP(interfaceName, ipAddress, subnetMask);
                }
            }
        }

        static void SetStaticIP(string interfaceName, string ipAddress, string subnetMask, string gateway="")
        {
            // netshコマンドを使ってIPアドレス設定を変更
            string command = $"interface ip set address name=\"{interfaceName}\" static {ipAddress} {subnetMask} {gateway}";

            // コマンドを実行
            ProcessStartInfo processInfo = new ProcessStartInfo("netsh", command)
            {
                Verb = "runas",  // 管理者権限で実行
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (Process process = Process.Start(processInfo))
            {
                process?.WaitForExit();
            }

            Console.WriteLine("IPアドレスの設定が完了しました。");
        }
    }
}
