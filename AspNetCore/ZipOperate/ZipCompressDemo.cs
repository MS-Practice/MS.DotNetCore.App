using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.ZipOperate {
    // https://blog.stephencleary.com/2016/11/streaming-zip-on-aspnet-core.html
    public class ZipCompressDemo {
        public static HttpClient Client { get; } = new HttpClient();

        public async Task Download() {
            var stream = await Client.GetStreamAsync("https://www.baidu.com");

        }
    }
}