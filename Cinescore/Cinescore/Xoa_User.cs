using Outside_Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Outside_Interface
{
    internal class Xoa_User
    {
        static string filePath1 = @"danhsachcmtid.txt";
        public static void Bin(string filepath, string accountnotify, ref bool gate_end)
        {
            List<List<string>> database = new List<List<string>>();
            List<List<string>> Mangtam = new List<List<string>>();
            List<List<string>> Mangbin = new List<List<string>>();
            try
            {
                database = Giao_dien_ngoai.Read_File(filepath);
                string[] option2 = new string[] {"Lọc theo số lần đăng nhập", "Lọc theo số lần bị report",
                "Lọc theo số lần comment", "Lọc theo AccountId", "Quay Lại"};
                Menu menu = new Menu(option2, accountnotify + "\n" + "LỌC USER");
                int rac = 0;
                int choice2 = menu.Run(ref rac);
                switch (choice2)
                {
                    case 0:
                        Loc_User(database, Mangtam, Mangbin, 3);
                        Giao_dien_ngoai.Print_Loop_File(Mangtam, filepath);
                        Console.ReadKey(); // Dùng để nhận giá trị nhập vào từ bàn phím user
                                           // -> Mục đích để hiện thị thông báo sau khi lọc rác thành công
                        break;
                    case 1:
                        Loc_User(database, Mangtam, Mangbin, 4);
                        Giao_dien_ngoai.Print_Loop_File(Mangtam, filepath);
                        Console.ReadKey();
                        break;
                    case 2:
                        Loc_User(database, Mangtam, Mangbin, 5);
                        Giao_dien_ngoai.Print_Loop_File(Mangtam, filepath);
                        Console.ReadKey();
                        break;
                    case 3:
                        Loc_User(database, Mangtam, Mangbin, 0);
                        Giao_dien_ngoai.Print_Loop_File(Mangtam, filepath);
                        Console.ReadKey();
                        break;
                    case 4:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Một hàm lọc User đảm nhiệm lọc theo 4 kiểu: Thời gian đăng nhập (logintime), Số lần bị report(reportime), Số lần review (commenttime),
        // Nhập trực tiếp một Id muốn xóa.
        private static void Loc_User(List<List<string>> database, List<List<string>> Mangtam, List<List<string>> Mangbin, int b)
        {
            Console.Clear();
            // Dùng để hiện thị tùy theo từng chức năng, giá trị kiểm tra là b
            // Tương ứng với vị trị từng biến trong trường dữ liệu
            // Ví dụ b = 3 tức là vị trí của logintime và tương tự vậy
            string text = "Lọc theo ";
            string text2 = null;
            if (b == 3)
            {
                text2 = "Số lần đăng nhập";
                text += text2 + " nhỏ hơn: ";
            }
            else if (b == 4)
            {
                text2 = "Số lần report";
                text += text2 + " lớn hơn: ";
            }
            else if (b == 5)
            {
                text2 = "Số lần review phim";
                text += text2 + " nhỏ hơn: ";
            }
            else
            {
                text2 = "Số lần Account Id";
                text += text2 + " bằng: ";
            }


            while (true)
            {
                // Nhận giá trị nhập vào từ người dùng, kiểu int
                int a = Check_Nhapvao.Themdulieu_int(text);
                int n = 0;
                for (int i = 0; i < database.Count; i++)
                {
                    // Kiểm ta liệu có chuyển kiểu thành công, đưa vào biến n từ giá trị tạm thời trong mảng database
                    // và phân quyền có phải là user hay không
                    // Đúng thì chạy tiếp
                    if (int.TryParse(database[i][b], out n) && database[i][6] == "U")
                    {
                        if (b == 4 && n >= a) // Lọc theo reporttime, nếu số lần báo cáo lớn hơn điều kiện, lưu vào mảng Bin
                        {
                            Mangbin.Add(database[i]);
                        }
                        else if (b == 0 && n == a) // Lọc trực tiếp từ giá trị nhận vào Account Id, nếu bằng thì lưu vào mảng Bin
                        {
                            Mangbin.Add(database[i]);
                        }
                        else if ((b == 3 || b == 5) && n <= a) // Lọc theo logintime hoặc commenttime, nếu nhỏ hơn điều kiện thì lưu vào mảng Bin
                        {
                            Mangbin.Add(database[i]);
                        }
                        else Mangtam.Add(database[i]);
                    }
                    else Mangtam.Add(database[i]); // Lưu vào mảng tạm
                                                   // Mục đích để lưu lại các trường dữ liệu không thỏa điều kiện vào file
                }

                if (Mangtam.Count < database.Count) // Thực hiện in thông báo những trường dữ liệu bị xóa, xong rồi break
                {
                    Console.Clear();
                    Console.WriteLine("Đã xóa thành công, những user bị xóa là: ");
                    for (int i = 0; i < Mangbin.Count; i++)
                    {
                        text = $"{Mangbin[i][1]} - AccountId: {Mangbin[i][0]}";
                        text += b != 0 ? $" - {text2}: {Mangbin[i][b]}" : " ";
                        Loc_User_vComment(Mangbin[i][0]);
                        Console.WriteLine(text);
                    }
                    break;
                }
                else // Nếu không thì in ra lỗi và bắt nhập lại
                {
                    Console.Clear();
                    Console.WriteLine("Không có User nào thỏa điều kiện.");
                    // Xóa mảng để tránh việc khi ta nhập vào a mà đến else này thì nó sẽ xóa tất cả data có trong hai mảng dưới
                    // Tránh trường hợp là dữ liệu vẫn còn lưu lại dẫn đến lỗi khi ta nhập lại a để lọc user.
                    Mangtam.Clear();
                    Mangbin.Clear();
                }
            }
        }

        // Mục đích: Khi tiến hành lọc User thì tiện thể xóa lun các review mà user đó đã thực hiện
        private static void Loc_User_vComment(string account_ID)
        {
            try
            {
                List<List<string>> db_1 = Giao_dien_ngoai.Read_File(filePath1);
                List<List<string>> db_u = new List<List<string>>();
                for (int i = 0; i < db_1.Count; i++)
                {
                    string line = db_1[i][2]; // Lấy ra account id từ file danhsachcmtid.txt
                    if (line != account_ID) // rồi check nó với tham số account_Id truyền vào
                    {
                        db_u.Add(db_1[i]); // Nếu trùng thì loại nó ra khỏi mảng
                        // Mục đích để khi dùng hàm Print_Loop_File để khi đè data từ mảng db_u vào 
                        // file danhsachcmtid.txt thì những review của user đã bị xóa cũng sẽ mất theo
                        // Do nó không được Add vào mảng db_u
                    }
                }
                Giao_dien_ngoai.Print_Loop_File(db_u, filePath1); // Thực hiện ghi đè file từ mảng db_u vào file txt
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Loc_User_vComment: {e.Message}");
                Console.ReadLine();
            }
        }
    }
}
