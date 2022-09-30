using System;

namespace DotnetSpider.Infrastructure
{
	public static class UriUtilities
	{
		/// <summary>
		/// 计算最终的URL
		/// </summary>
		/// <param name="uri">Base uri</param>
		/// <param name="relativeUri">Relative uri</param>
		/// <returns>最终的URL</returns>
		public static string CanonicalizeUrl(string uri, Uri relativeUri)
		{
			try
			{
				if (uri.StartsWith("//"))
				{
					return $"{relativeUri.Scheme}:{uri}";
				}
				else
				{
					var domain = relativeUri.Port == 80 || relativeUri.Port == 443
							? $"{relativeUri.Scheme}://{relativeUri.Host}"
							: $"{relativeUri.Scheme}://{relativeUri.Host}:{relativeUri.Port}";
					var baseUri = uri.StartsWith("?") ? new Uri(domain + relativeUri.AbsolutePath) : new Uri(domain);
					var abs = new Uri(baseUri, uri);
					return abs.AbsoluteUri;
				}
			}
			catch (Exception)
			{
				return uri;
			}
		}

		public static bool IsHttpUri(Uri uri) => IsSupportedScheme(uri.Scheme);

		public static bool IsSupportedScheme(string scheme) => IsSupportedNonSecureScheme(scheme) ||
															   IsSupportedSecureScheme(scheme);

		public static bool IsSupportedNonSecureScheme(string scheme) =>
			string.Equals(scheme, "http", StringComparison.OrdinalIgnoreCase) ||
			IsNonSecureWebSocketScheme(scheme);

		public static bool IsSupportedSecureScheme(string scheme) =>
			string.Equals(scheme, "https", StringComparison.OrdinalIgnoreCase) ||
			IsSecureWebSocketScheme(scheme);

		public static bool IsNonSecureWebSocketScheme(string scheme) =>
			string.Equals(scheme, "ws", StringComparison.OrdinalIgnoreCase);

		public static bool IsSecureWebSocketScheme(string scheme) =>
			string.Equals(scheme, "wss", StringComparison.OrdinalIgnoreCase);
	}
}
