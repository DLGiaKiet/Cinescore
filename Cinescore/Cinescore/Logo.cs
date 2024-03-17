using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Outside_Interface
{
    internal class Logo
    {
        public static string Logo_UEH()
        {
            string introtext = @"
            ______       ______ __________________ ______         ______
            |    |       |    | |                | |    |         |    |
            |    |       |    | |    ____________| |    |         |    |
            |    |       |    | |    |             |    |         |    |
            |    |       |    | |    |____________ |    |_________|    |
            |    |       |    | |                | |                   |
            |    |       |    | |    ____________| |    ___________    |
            |    |_______|    | |    |             |    |         |    |
            |                 | |    |____________ |    |         |    |
            |                 | |                | |    |         |    |
            |_________________| |________________| |____|         |____|
                                 _                    _ _         
                                (_)                  (_| |        
                     _   _ _ __  ___   _____ _ __ ___ _| |_ _   _ 
                    | | | | '_ \| \ \ / / _ | '__/ __| | __| | | |
                    | |_| | | | | |\ V |  __| |  \__ | | |_| |_| |
                     \__,_|_| |_|_| \_/ \___|_|  |___|_|\__|\__, |
                                                             __/ |
                                                            |___/ 



                             - Nhập Enter để bỏ qua -
        ";

            return introtext;
        }

        public static string Logo_App()
        {
            string title = @" 
██████╗██╗███╗   ██╗███████╗███████╗ ██████╗ ██████╗ ██████╗ ███████╗
██╔════╝██║████╗  ██║██╔════╝██╔════╝██╔════╝██╔═══██╗██╔══██╗██╔════╝
██║     ██║██╔██╗ ██║█████╗  ███████╗██║     ██║   ██║██████╔╝█████╗  
██║     ██║██║╚██╗██║██╔══╝  ╚════██║██║     ██║   ██║██╔══██╗██╔══╝  
╚██████╗██║██║ ╚████║███████╗███████║╚██████╗╚██████╔╝██║  ██║███████╗
 ╚═════╝╚═╝╚═╝  ╚═══╝╚══════╝╚══════╝ ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝                                                       
        ";
            return title;
        }

        public static void Run_Intro()
        {
            string introtext = Logo_UEH();
            // biến bool dùng để dừng một task và chuyển sang task hoặc đoạn code tiếp theo
            bool flag = false;

            // Khởi tạo task1 với Logo "UEH"
            // Khởi tạo task2 với In ra tên nhóm
            Task t1 = new Task(() => Logo_Print(introtext, ref flag));
            Task t2 = new Task(() => TenNhom(ref flag));

            t1.Start(); // Chạy task1
            Waiting_for_you(ref flag); // Hàm để kiếm tra đến khi thỏa điều kiện thì dừng Task1 lại
            // Và chuyển sang Task2
            t1.Wait(); // Đợi task1 thực hiện xong
            flag = false; // Mục đích dùng để làm điều kiện dừng Task2
            Console.Clear();
            t2.Start(); // Chạy Task2
            Waiting_for_you(ref flag);// Hàm để kiếm tra đến khi thỏa điều kiện thì dừng Task2 lại
            t2.Wait();// Đợi task2 thực hiện xong
            // Rồi chuyển sang đoạn code tiếp theo
        }
        // Hàm in ra logo UEH
        private static void Logo_Print(string text, ref bool flag)
        {
            // Console.Out: Kiểu đại diện cho object đầu ra (output stream) trong console của C#
            // lock -> Đảm bảo rằng không có thread nào can thiệp việc in ra màn hình của thread khác trong
            // quá trình đang thực hiện in

            // Đảm bảo rằng chỉ có một thread được phép truy cập vào Console trong một thời điểm
            // -> Đảm bảo rằng in ra một cách tuần tự và không bị trộn lẫn với đầu ra của thread khác

            // Mục đích: Tránh xung đột dữ liệu
            lock (Console.Out)
            {
                while (!flag)
                {
                    int screenWidth = Console.WindowWidth;
                    int screenHeight = Console.WindowHeight;

                    // Tính toán vị trí in để đưa văn bản vào giữa màn hình
                    int leftMargin = (screenWidth - text.Split('\n')[1].Length - 12) / 2;
                    int topMargin = (screenHeight - text.Split('\n').Length) / 2;

                    // Xóa nội dung cũ bằng Console.Clear()
                    Console.Clear();

                    // Tách các dòng văn bản và in chúng
                    string[] lines = text.Split('\n');

                    // Kiểm tra nếu vị trí đặt con trỏ nằm ngoài giới hạn thì không đặt
                    // Mục đích để tránh bị lỗi khi in ra
                    if (leftMargin > 0 && topMargin > 0 && leftMargin < screenWidth && topMargin < screenHeight)
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            Console.SetCursorPosition(leftMargin, topMargin + i);
                            Console.WriteLine(lines[i]);
                        }

                        Thread.Sleep(500);
                    }
                }
            }
        }

        // Hàm in ra tên nhóm
        private static void TenNhom(ref bool flag)
        {
            lock (Console.Out)
            {
                while (!flag)
                {
                    string a1 = "Một sản phẩm đến từ nhóm VN5";
                    int screenWidth = Console.WindowWidth;
                    int screenHeight = Console.WindowHeight;
                    int leftMargin = (screenWidth - a1.Length) / 2;
                    int topMargin = (screenHeight - 1) / 2;
                    Console.Clear();
                    if (leftMargin > 0 && topMargin > 0 && leftMargin < screenWidth && topMargin < screenHeight)
                    {
                        Console.SetCursorPosition(leftMargin, topMargin);
                        Console.WriteLine(a1);
                    }
                    Thread.Sleep(500);
                }
            }
        }

        // Hàm kiểm tra điều kiện dùng Task
        private static void Waiting_for_you(ref bool flag)
        {
            bool localflag = flag;
            Task.Delay(2000).ContinueWith(b => // Dừng lại 2 giây rồi đưa biến bool localflag về true
            {                                  // Mục đích để dừng vòng lặp while bên dưới và chuyển sang Task khác
                localflag = true;
            });

            while (!localflag) // Liên tục in ra Logo hoặc tên nhóm
            {
                if (Console.KeyAvailable) // Nếu giá trị nhập vào là nút Enter thì
                                          // chuyển sang Task tiếp theo hoặc đoạn code tiếp theo
                {
                    var key = Console.ReadKey(intercept: true).Key;
                    if (key == ConsoleKey.Enter)
                    {
                        localflag = true; // Dùng vòng lặp while
                    }
                }

            }

            flag = localflag;
        }
    }
}
