using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outside_Interface;

namespace User
{
    internal class Report_A_Password
    {
        static string File_F_Report_Password = @"Truyvan.txt";

        public static void R_And_P(int ac_Id)
        {
            List<string> dataLines = File.ReadAllLines(File_F_Report_Password).ToList();
            string[] menuOptions = { "Báo cáo", "Đổi mật khẩu người dùng", "Quay lại" };
            string prompts = "Đổi mật khẩu và Báo cáo";
            int rac = 0;

            Menu menu = new Menu(menuOptions, prompts);
            int choice_Phu = menu.Run(ref rac);

            switch (choice_Phu)
            {
                case 0:
                    Console.Clear();
                    AddCommentReport(dataLines);
                    break;
                case 1:
                    Console.Clear();
                    ChangeUserPassword(dataLines, ac_Id);       
                    break;
                case 2:
                    break;
            }

        }
        static void AddCommentReport(List<string> dataLines)
        {
            Console.Clear();
            Search_Film.Print_Normal("Thêm báo cáo bình luận ");
            string username = "";

            do
            {
                Console.Write("Nhập tên người dùng (username): ");
                username = Console.ReadLine();

                // Kiểm tra xem người dùng có tồn tại trong dữ liệu hay không
                string userData = dataLines.FirstOrDefault(line => line.Contains(username));
                if (GetValidReport(dataLines, username))
                {
                    // Lấy số lần báo cáo hiện tại của người dùng
                    int reportTime = int.Parse(userData.Split(',')[4].Trim());
                    reportTime++;

                    // Cập nhật dữ liệu người dùng
                    for (int i = 0; i < dataLines.Count; i++)
                    {
                        if (dataLines[i].Contains(username))
                        {
                            // Cập nhật số lần báo cáo mới
                            string[] userDataParts = dataLines[i].Split(',');
                            userDataParts[4] = reportTime.ToString();
                            dataLines[i] = string.Join(",", userDataParts);
                            break;
                        }
                    }

                    // Ghi lại dữ liệu vào tệp
                    File.WriteAllLines(File_F_Report_Password, dataLines);

                    Console.WriteLine("Báo cáo bình luận thành công.");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine("Người dùng không tồn tại. Vui lòng thử lại.");
            } while (true);    
        }

        static void ChangeUserPassword(List<string> dataLines, int ac_Id)
        {
            Console.Clear();
            Search_Film.Print_Normal("Đổi mật khẩu người dùng");
            string User_Id = Convert.ToString(ac_Id);

            Console.Write("Nhập mật khẩu cũ: ");
            string oldPassword = Console.ReadLine();
            string passwordData = dataLines.FirstOrDefault(line => line.Contains(User_Id) && line.Contains(oldPassword));
            while (passwordData == null)
            {
                Console.WriteLine("Mật khẩu cũ không chính xác. Vui lòng thử lại.");
                Console.Write("Nhập mật khẩu cũ: ");
                oldPassword = Console.ReadLine();
                passwordData = dataLines.FirstOrDefault(line => line.Contains(User_Id) && line.Contains(oldPassword));
            }

            Console.Write("Nhập mật khẩu mới: ");
            string newPassword = Console.ReadLine();

            // Cập nhật mật khẩu mới trong dữ liệu
            for (int i = 0; i < dataLines.Count; i++)
            {
                if (dataLines[i].Contains(User_Id) && dataLines[i].Contains(oldPassword))
                {
                    string[] userDataParts = dataLines[i].Split(',');
                    userDataParts[2] = newPassword;
                    dataLines[i] = string.Join(",", userDataParts);
                    break;
                }
            }

            // Ghi lại dữ liệu vào tệp
            File.WriteAllLines(File_F_Report_Password, dataLines);
            Console.WriteLine("Đổi mật khẩu thành công.");
            Console.ReadLine();
        }

        static bool GetValidReport(List<string> datalines, string username)
        {
            foreach (string line in datalines)
            {
                string[] check_data = line.Split(',');
                if (check_data[1] == username && check_data[6] != "A") return true;
            }

            return false;
        }
    }
}
