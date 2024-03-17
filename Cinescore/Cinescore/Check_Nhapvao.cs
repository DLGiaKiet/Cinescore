using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outside_Interface
{
    internal class Check_Nhapvao
    {
        // Dùng để nhận một string nhập vào từ người dùng
        public static string Themdulieu_string(string text)
        {
            string at1;
            while (true)
            {
                Console.Write(text);
                at1 = Console.ReadLine();
                // Kiểm tra xem liệu user có nhập vào gì không hay là chỉ nhập khoảng trắng
                // Nếu đúng như điều trên thì đưa đến else bắt nhập lại không thì trả về giá trị kiểu string
                if (!string.IsNullOrEmpty(at1) && !string.IsNullOrWhiteSpace(at1) && !at1.Contains(','))
                {
                    return at1;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Giá trị nhập vào không hợp lệ. Vui lòng nhập lại");
                }
            }
        }

        // Dùng để nhận một int nhập vào từ người dùng
        public static int Themdulieu_int(string text, int num_dk = 0)
        {
            int num;
            while (true)
            {
                Console.Write(text);
                // Kiểm tra liệu giá trị user nhập vào có phải là kiểu int và lớn hơn 0
                // Nếu đúng điều trên thì trả về giá trị kiểu int không thì chạy qua else bắt nhập lại
                if (int.TryParse(Console.ReadLine(), out num) && num > num_dk)
                {
                    return num;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Giá trị nhập vào không hợp lệ. Vui lòng nhập lại");
                }
            }
        }

        public static double DoubleInputWithPrompt(string prompt, int a = 0, int b = 0, bool flag = false) // Cho vào Namespace Check_Nhapvao
        {
            double num;
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out num) && num > 0 && !flag)
                {
                    return num;  
                }
                else if(dk_double(num, a, b) && flag)
                {
                    return num;
                }    
                else
                {
                    Console.Clear();
                    Console.WriteLine("Giá trị nhập vào không hợp lệ. Vui lòng nhập lại");
                }
            }
        }

        private static bool dk_double(double n, int a, int b)
        {
            if (n >= a && n <= b) return true;
            else return false;
        }
        // Hàm dùng để chạy và đưa ra Id cuối cùng
        // Kiểm tra nếu có khoảng trống số Id trong các trường dữ liệu
        // Nếu không có thì tạo Id mới tiếp tục từ trường dữ liệu cuối cùng
        public static string Check_Id(int rowC, List<List<string>> database)
        {
            int max = 0;
            int c = 0;
            int accountId = 0;
            for (int i = 0; i < rowC; i++)
            {
                if (int.TryParse(database[i][0], out c))
                {
                    if (max < c) max = c;
                }
            }

            for (int i = 100; i <= max; i++)
            {
                if (Have_Space(i, rowC, database))
                {
                    accountId = i;
                    break;
                }
                else if ((i + 1) > max) accountId = i + 1;
            }

            return accountId.ToString();
        }

        // Kiểm tra liệu trong file có tồn tại trường dữ liệu trống hay không
        // Ví dụ: từ 101 -> 105, trống 102, hàm Have_Space kiểm tra thấy điều đó -> Thực hiện trả về true
        // Nếu không có khoảng trống nào -> Trả về false.
        // Mục đích: Để kiểm tra nếu tồn tại khoảng trống giống với ví dụ trên -> Khi user tạo TK thì cho user cái ID trống đó
        // Nếu không thì tạo Id mới tiếp tục từ trường dữ liệu cuối cùng.
        private static bool Have_Space(int num_check, int rowC, List<List<string>> database)
        {
            int c = 0;
            for (int i = 0; i < rowC; i++)
            {
                if (int.TryParse(database[i][0], out c))
                {
                    if (c == num_check) return false;
                }
            }
            return true;
        }

        // Hàm để chạy Nhập user name khi hàm Dang_Ky được thực hiện
        // Kiểm tra liệu user có điền gì vào username hoặc điền không trùng username không
        // Nếu đúng thì hàm while sẽ được thoát ra và trả về kiểu string
        // Không thì bắt nhập lại cho tới khi đúng
        public static string Check_Same_Name(int rowC, List<List<string>> database)
        {
            string Username = " ";
            do
            {
                Console.Clear();
                if (!string.IsNullOrEmpty(Username) && Same_Name(Username, rowC, database))
                {
                    Console.WriteLine("Username đã tồn tại. Vui lòng nhập lại.");
                }
                Username = Themdulieu_string("Nhập vào Username: ");
            } while (Same_Name(Username, rowC, database));

            return Username;
        }

        // Kiểm tra liệu có trùng User_Name khi hàm Dang_Ky được thực hiện
        // Nếu có thì trả về true -> Nếu không có thì trả về false
        // Mục đích là bắt user nhập lại nếu trùng username khi Đăng Ký
        private static bool Same_Name(string Username, int rowC, List<List<string>> database)
        {
            for (int i = 0; i < rowC; i++)
            {
                if (Username == database[i][1]) return true;
            }
            return false;
        }
    }
}
