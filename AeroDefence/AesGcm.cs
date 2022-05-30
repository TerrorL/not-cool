using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Stealer
{
	// Token: 0x02000004 RID: 4
	internal class AesGcm
	{
		// Token: 0x06000016 RID: 22 RVA: 0x00002A30 File Offset: 0x00000C30
		public byte[] Decrypt(byte[] key, byte[] iv, byte[] aad, byte[] cipherText, byte[] authTag)
		{
			IntPtr intPtr = this.OpenAlgorithmProvider(BCrypt.BCRYPT_AES_ALGORITHM, BCrypt.MS_PRIMITIVE_PROVIDER, BCrypt.BCRYPT_CHAIN_MODE_GCM);
			IntPtr hKey;
			IntPtr hglobal = this.ImportKey(intPtr, key, out hKey);
			BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO = new BCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, authTag);
			byte[] array = new byte[this.MaxAuthTagSize(intPtr)];
			int num = 0;
			uint num2 = BCrypt.BCryptDecrypt(hKey, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, null, 0, ref num, 0);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() (get size) failed with status code: {0}", num2));
			}
			byte[] array2 = new byte[num];
			num2 = BCrypt.BCryptDecrypt(hKey, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, array2, array2.Length, ref num, 0);
			if (num2 == BCrypt.STATUS_AUTH_TAG_MISMATCH)
			{
				throw new CryptographicException("BCrypt.BCryptDecrypt(): authentication tag mismatch");
			}
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() failed with status code:{0}", num2));
			}
			bcrypt_AUTHENTICATED_CIPHER_MODE_INFO.Dispose();
			BCrypt.BCryptDestroyKey(hKey);
			Marshal.FreeHGlobal(hglobal);
			BCrypt.BCryptCloseAlgorithmProvider(intPtr, 0U);
			return array2;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002B28 File Offset: 0x00000D28
		private int MaxAuthTagSize(IntPtr hAlg)
		{
			byte[] property = this.GetProperty(hAlg, BCrypt.BCRYPT_AUTH_TAG_LENGTH);
			return BitConverter.ToInt32(new byte[]
			{
				property[4],
				property[5],
				property[6],
				property[7]
			}, 0);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002B68 File Offset: 0x00000D68
		private IntPtr OpenAlgorithmProvider(string alg, string provider, string chainingMode)
		{
			IntPtr zero = IntPtr.Zero;
			uint num = BCrypt.BCryptOpenAlgorithmProvider(out zero, alg, provider, 0U);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptOpenAlgorithmProvider() failed with status code:{0}", num));
			}
			byte[] bytes = Encoding.Unicode.GetBytes(chainingMode);
			num = BCrypt.BCryptSetAlgorithmProperty(zero, BCrypt.BCRYPT_CHAINING_MODE, bytes, bytes.Length, 0);
			if (num != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptSetAlgorithmProperty(BCrypt.BCRYPT_CHAINING_MODE, BCrypt.BCRYPT_CHAIN_MODE_GCM) failed with status code:{0}", num));
			}
			return zero;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002BD8 File Offset: 0x00000DD8
		private IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
		{
			byte[] property = this.GetProperty(hAlg, BCrypt.BCRYPT_OBJECT_LENGTH);
			int num = BitConverter.ToInt32(property, 0);
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			byte[] array = this.Concat(new byte[][]
			{
				BCrypt.BCRYPT_KEY_DATA_BLOB_MAGIC,
				BitConverter.GetBytes(1),
				BitConverter.GetBytes(key.Length),
				key
			});
			uint num2 = BCrypt.BCryptImportKey(hAlg, IntPtr.Zero, BCrypt.BCRYPT_KEY_DATA_BLOB, out hKey, intPtr, num, array, array.Length, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptImportKey() failed with status code:{0}", num2));
			}
			return intPtr;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002C6C File Offset: 0x00000E6C
		private byte[] GetProperty(IntPtr hAlg, string name)
		{
			int num = 0;
			uint num2 = BCrypt.BCryptGetProperty(hAlg, name, null, 0, ref num, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() (get size) failed with status code:{0}", num2));
			}
			byte[] array = new byte[num];
			num2 = BCrypt.BCryptGetProperty(hAlg, name, array, array.Length, ref num, 0U);
			if (num2 != 0U)
			{
				throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() failed with status code:{0}", num2));
			}
			return array;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002CD4 File Offset: 0x00000ED4
		public byte[] Concat(params byte[][] arrays)
		{
			int num = 0;
			foreach (byte[] array in arrays)
			{
				if (array != null)
				{
					num += array.Length;
				}
			}
			byte[] array2 = new byte[num - 1 + 1];
			int num2 = 0;
			foreach (byte[] array3 in arrays)
			{
				if (array3 != null)
				{
					Buffer.BlockCopy(array3, 0, array2, num2, array3.Length);
					num2 += array3.Length;
				}
			}
			return array2;
		}
	}
}
