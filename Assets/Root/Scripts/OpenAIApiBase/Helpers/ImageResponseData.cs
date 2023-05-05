// ImageResponseData.cs

using System;
using UnityEngine;

namespace YagizAyer.Root.Scripts.OpenAIApiBase.Helpers
{
    /*
        {
            "created": 1683208317,
            "data": [
                {
                    "url": "https://oaidalleapiprodscus.blob.core.windows.net/private/org-jRY6V3Xe2RKnej8mIn20jK7A/user-NuIzCSRb9Lh32nvvyEglGqQW/img-UFdmu4JbZ3pDK9xMN8Jons4t.png?st=2023-05-04T12%3A51%3A57Z&se=2023-05-04T14%3A51%3A57Z&sp=r&sv=2021-08-06&sr=b&rscd=inline&rsct=image/png&skoid=6aaadede-4fb3-4698-a8f6-684d7786b067&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2023-05-04T11%3A39%3A21Z&ske=2023-05-05T11%3A39%3A21Z&sks=b&skv=2021-08-06&sig=OCHXt%2B0jCgv%2BCOutzsjEHhW8A6wndpdppjxEd6KUFlE%3D"
                }
            ]
        }
     */
    public class ImageResponseData
    {
        public int Created;
        public UrlData[] Urls;

        public class UrlData
        {
            public string Url;
        }

        public static ImageResponseData FromJson(string json)
        {
            var rawData = JsonUtility.FromJson<ImageResponseRawData>(json);
            var urls = new UrlData[rawData.data.Length];
            
            for (var i = 0; i < rawData.data.Length; i++)
            {
                var rawUrl = rawData.data[i];
                urls[i] = new UrlData
                {
                    Url = rawUrl.url
                };
            }
            
            var result = new ImageResponseData
            {
                Created = rawData.created,
                Urls = urls
            };
            return result;
        }

        // for JSON serialization
        [Serializable]
        private class ImageResponseRawData
        {
            public int created;
            public RawUrlData[] data;
        }

        [Serializable]
        private class RawUrlData
        {
            public string url;
        }
    }
}