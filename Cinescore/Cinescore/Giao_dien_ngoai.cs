using Outside_Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outside_Interface
{
    internal class Giao_dien_ngoai
    {
        // Đọc dữ liệu từ file và lưu nó vào biến mảng tạm database
        public static List<List<string>> Read_File(string filepath)
        {
            List<List<string>> database = new List<List<string>>();
            using (StreamReader sr = new StreamReader(filepath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    List<string> dbphu = new List<string>(line.Split(','));
                    database.Add(dbphu);
                }
            }
            return database;
        }

        // Thực hiện việc in vào file từ một mảng sau khi thực hiện chỉnh sửa gì đó trong mảng tạm
        public static void Print_Loop_File(List<List<string>> db, string filepath)
        {
            using (StreamWriter st = new StreamWriter(filepath))
            {
                for (int i = 0; i < db.Count; i++)
                {
                    for (int j = 0; j < db[0].Count; j++)
                    {
                        st.Write(db[i][j]);
                        if (j + 1 < db[0].Count) st.Write(',');
                    }
                    st.Write('\n');
                }
            }
        }
        public static void First_I(string filepath, string title, ref string accountnotify, ref bool accessauthority, 
            ref bool gate_next, ref bool gate_end, ref int ac_Id)
        {
            List<List<string>> database = new List<List<string>>();
            string TenNguoiDung = " ";
            string AccountId = " ";
            try
            {
                database = Read_File(filepath);

                int rowC = database.Count;

                string[] option1 = new string[] { "Đăng Nhập", " Đăng Ký ", "  Thoát  " };
                Menu menu = new Menu(option1, title, false);
                int rac = 0;
                int choice1 = menu.Run(ref rac);

                switch (choice1)
                {
                    case 0:
                        // Nhận giá trị trả về là true hoặc false khi chạy hàm Dang_Nhap
                        accessauthority = DangNhap(filepath, rowC, database, ref TenNguoiDung, ref AccountId);
                        Console.Clear();
                        // Gán giá trị string được lấy từ hàm Dang_Nhap
                        if (accessauthority) accountnotify = $"{AccountId} - {TenNguoiDung} - Authority is Admin";
                        else accountnotify = $"{AccountId} - {TenNguoiDung} - Authority is User";
                        ac_Id = Convert.ToInt32(AccountId);
                        // Cập nhập giá trị logintime vào file
                        Print_Loop_File(database, filepath);
                        // một biến bool dùng để chuyển sang chức năng tiếp theo
                        gate_next = true;
                        break;
                    case 1:
                        DangKy(filepath, rowC, database);
                        Console.ReadKey();
                        break;
                    case 2:
                        gate_end = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        // Hàm chạy chức năng Đăng Ký
        // Yêu cầu người dùng nhập hai giá trị vào hai biến: Username, Password
        // Đồng thời gán những giá trị cơ sở khi tạo một tài khoản mới
        public static void DangKy(string filepath, int rowC, List<List<string>> database)
        {
            using (StreamWriter sm = new StreamWriter(filepath, true))
            {
                string accountId = Check_Nhapvao.Check_Id(rowC, database); // Check Id tự bổ sung vào chỗ trống hoặc tự tăng
                string Username = Check_Nhapvao.Check_Same_Name(rowC, database); // Kiểm tra xem liệu có trùng tên không
                string Password = Check_Nhapvao.Themdulieu_string("Nhập vào Password: ");

                int logintime = 0;
                int reporttime = 0;
                int commenttime = 0;
                string accessright = "U";

                sm.WriteLine($"{accountId},{Username},{Password},{logintime},{reporttime},{commenttime},{accessright}");
            }

            Console.WriteLine("Bạn đã đăng ký thành công. Vui lòng ấn bất kỳ nút nào để thoát ra và thực hiện việc đăng nhập.");
        }

        // Hàm chạy chức năng Đăng Nhập 
        // Hai biến nhận giá trị nhập vào từ user: Username và Password
        // Kiểm tra nếu khớp trong mảng tạm thì tiếp tục kiểm tra xem liệu phân quyền khi này là gì
        // Nếu là Admin tức "A" thì trả về true, không thì trả về false.
        // Đồng thời tăng giá trị biến logintime trong trường dữ liệu khi đăng nhập vào.
        public static bool DangNhap(string filepath, int rowC, List<List<string>> database, ref string TenNguoiDung, ref string AccountId)
        {
            string Username;
            string Password;
            // Một biến bool kiểm tra liệu khi username hoặc password hoặc cả hai không thỏa điều kiện thì in ra thông báo lỗi
            // và bắt nhập lại
            bool errormsgflag = false;
            while (true)
            {
                Console.Clear();
                if (errormsgflag) Console.WriteLine("Username hoặc Password sai. Vui lòng nhập lại");
                Username = Check_Nhapvao.Themdulieu_string("Username: ");
                Password = Check_Nhapvao.Themdulieu_string("Password: ");


                for (int i = 0; i < rowC; i++)
                {
                    if (Username == database[i][1] && Password == database[i][2])
                    {
                        AccountId = database[i][0];
                        TenNguoiDung = database[i][1];

                        // Thực hiện tăng logintime khi đăng nhập vào bất cứ tài khoản nào.
                        int login_turn = Convert.ToInt32(database[i][3]);
                        login_turn++;
                        database[i][3] = Convert.ToString(login_turn);

                        if (database[i][6] == "A") return true;
                        else return false;
                    }
                }
                errormsgflag = true;
            }
        }
    }
}
