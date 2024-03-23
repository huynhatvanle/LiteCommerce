using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020045.BusinessLayers
{
    public static class Configuration
    {
        /// <summary>
        /// Chuỗi thông số kết nối đến CSDL
        /// </summary>
        public static string ConnectionString { get; set; } = "";

        /// <summary>
        /// Khởi tạo cấu hình cho BusinessLayer
        /// (Hàm này được gọi trước khi ứng dụng chạy)
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static void Initialize(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
        }
    }
}
