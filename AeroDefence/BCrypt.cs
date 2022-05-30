using System;
using System.Runtime.InteropServices;

namespace Stealer
{
	// Token: 0x02000005 RID: 5
	public static class BCrypt
	{
		// Token: 0x0600001D RID: 29
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation, uint dwFlags);

		// Token: 0x0600001E RID: 30
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint flags);

		// Token: 0x0600001F RID: 31
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptGetProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, uint flags);

		// Token: 0x06000020 RID: 32
		[DllImport("bcrypt.dll", EntryPoint = "BCryptSetProperty")]
		internal static extern uint BCryptSetAlgorithmProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

		// Token: 0x06000021 RID: 33
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptImportKey(IntPtr hAlgorithm, IntPtr hImportKey, [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, uint dwFlags);

		// Token: 0x06000022 RID: 34
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptDestroyKey(IntPtr hKey);

		// Token: 0x06000023 RID: 35
		[DllImport("bcrypt.dll")]
		public static extern uint BCryptEncrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, uint dwFlags);

		// Token: 0x06000024 RID: 36
		[DllImport("bcrypt.dll")]
		internal static extern uint BCryptDecrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, int dwFlags);

		// Token: 0x0400000F RID: 15
		public const uint ERROR_SUCCESS = 0U;

		// Token: 0x04000010 RID: 16
		public const uint BCRYPT_PAD_PSS = 8U;

		// Token: 0x04000011 RID: 17
		public const uint BCRYPT_PAD_OAEP = 4U;

		// Token: 0x04000012 RID: 18
		public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(1296188491);

		// Token: 0x04000013 RID: 19
		public static readonly string BCRYPT_OBJECT_LENGTH = "ObjectLength";

		// Token: 0x04000014 RID: 20
		public static readonly string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";

		// Token: 0x04000015 RID: 21
		public static readonly string BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength";

		// Token: 0x04000016 RID: 22
		public static readonly string BCRYPT_CHAINING_MODE = "ChainingMode";

		// Token: 0x04000017 RID: 23
		public static readonly string BCRYPT_KEY_DATA_BLOB = "KeyDataBlob";

		// Token: 0x04000018 RID: 24
		public static readonly string BCRYPT_AES_ALGORITHM = "AES";

		// Token: 0x04000019 RID: 25
		public static readonly string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";

		// Token: 0x0400001A RID: 26
		public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1;

		// Token: 0x0400001B RID: 27
		public static readonly int BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 1;

		// Token: 0x0400001C RID: 28
		public static readonly uint STATUS_AUTH_TAG_MISMATCH = 3221266434U;

		// Token: 0x02000006 RID: 6
		public struct BCRYPT_PSS_PADDING_INFO
		{
			// Token: 0x06000026 RID: 38 RVA: 0x00002DCC File Offset: 0x00000FCC
			public BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
			{
				this.pszAlgId = pszAlgId;
				this.cbSalt = cbSalt;
			}

			// Token: 0x0400001D RID: 29
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x0400001E RID: 30
			public int cbSalt;
		}

		// Token: 0x02000007 RID: 7
		public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO : IDisposable
		{
			// Token: 0x06000027 RID: 39 RVA: 0x00002DDC File Offset: 0x00000FDC
			public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag)
			{
				this = default(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
				this.dwInfoVersion = BCrypt.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
				this.cbSize = Marshal.SizeOf(typeof(BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
				if (iv != null)
				{
					this.cbNonce = iv.Length;
					this.pbNonce = Marshal.AllocHGlobal(this.cbNonce);
					Marshal.Copy(iv, 0, this.pbNonce, this.cbNonce);
				}
				if (aad != null)
				{
					this.cbAuthData = aad.Length;
					this.pbAuthData = Marshal.AllocHGlobal(this.cbAuthData);
					Marshal.Copy(aad, 0, this.pbAuthData, this.cbAuthData);
				}
				if (tag != null)
				{
					this.cbTag = tag.Length;
					this.pbTag = Marshal.AllocHGlobal(this.cbTag);
					Marshal.Copy(tag, 0, this.pbTag, this.cbTag);
					this.cbMacContext = tag.Length;
					this.pbMacContext = Marshal.AllocHGlobal(this.cbMacContext);
				}
			}

			// Token: 0x06000028 RID: 40 RVA: 0x00002EBC File Offset: 0x000010BC
			public void Dispose()
			{
				if (this.pbNonce != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbNonce);
				}
				if (this.pbTag != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbTag);
				}
				if (this.pbAuthData != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbAuthData);
				}
				if (this.pbMacContext != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(this.pbMacContext);
				}
			}

			// Token: 0x0400001F RID: 31
			public int cbSize;

			// Token: 0x04000020 RID: 32
			public int dwInfoVersion;

			// Token: 0x04000021 RID: 33
			public IntPtr pbNonce;

			// Token: 0x04000022 RID: 34
			public int cbNonce;

			// Token: 0x04000023 RID: 35
			public IntPtr pbAuthData;

			// Token: 0x04000024 RID: 36
			public int cbAuthData;

			// Token: 0x04000025 RID: 37
			public IntPtr pbTag;

			// Token: 0x04000026 RID: 38
			public int cbTag;

			// Token: 0x04000027 RID: 39
			public IntPtr pbMacContext;

			// Token: 0x04000028 RID: 40
			public int cbMacContext;

			// Token: 0x04000029 RID: 41
			public int cbAAD;

			// Token: 0x0400002A RID: 42
			public long cbData;

			// Token: 0x0400002B RID: 43
			public int dwFlags;
		}

		// Token: 0x02000008 RID: 8
		public struct BCRYPT_KEY_LENGTHS_STRUCT
		{
			// Token: 0x0400002C RID: 44
			public int dwMinLength;

			// Token: 0x0400002D RID: 45
			public int dwMaxLength;

			// Token: 0x0400002E RID: 46
			public int dwIncrement;
		}

		// Token: 0x02000009 RID: 9
		public struct BCRYPT_OAEP_PADDING_INFO
		{
			// Token: 0x06000029 RID: 41 RVA: 0x00002F3D File Offset: 0x0000113D
			public BCRYPT_OAEP_PADDING_INFO(string alg)
			{
				this.pszAlgId = alg;
				this.pbLabel = IntPtr.Zero;
				this.cbLabel = 0;
			}

			// Token: 0x0400002F RID: 47
			[MarshalAs(UnmanagedType.LPWStr)]
			public string pszAlgId;

			// Token: 0x04000030 RID: 48
			public IntPtr pbLabel;

			// Token: 0x04000031 RID: 49
			public int cbLabel;
		}
	}
}
