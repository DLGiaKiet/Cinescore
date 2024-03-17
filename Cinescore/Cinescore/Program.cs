using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Admin;
using Outside_Interface;
using User;

namespace Cinescore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            // gate_next dùng để chuyển sang chức năng khác
            // gate-end dùng để kết thúc chương trình
            bool gate_next = false;
            bool gate_end = false;

            // Nếu true là phân quyền Admin, false là phân quyền user -> Kết quả được gán dựa vào hàm Dang_Nhap
            bool accessauthority = false;
            // Giúp người dùng nhận biết Account Id, Username và Phân quyền của mình dễ dàng hơn
            string accountnotify = " ";
            int ac_Id = 0;
            string filepath = @"Truyvan.txt";

            string title = Logo.Logo_App();
            Logo.Run_Intro();

            while (!gate_next && !gate_end)
            {
                Giao_dien_ngoai.First_I(filepath, title, ref accountnotify, ref accessauthority, 
                    ref gate_next, ref gate_end, ref ac_Id);
            }

            if (accessauthority)
            {
                // Admin
                while (!gate_end)
                {
                    Admin_Interface.ProcessSelectedOption(filepath, accountnotify, ref gate_end);
                }
            }
            else
            {
                // User
                while(!gate_end)
                {
                    Search_Film.ProcessSelectedOption(ref gate_end, ac_Id, accountnotify);
                }    
            }
        }
    }
}
