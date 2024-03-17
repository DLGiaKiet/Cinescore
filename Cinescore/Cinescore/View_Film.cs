using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Admin;
using Outside_Interface;

namespace User
{
    internal class View_Film
    {
        public static void User_Hienthi_Film(List<List<string>> mangtam, int ac_Id)
        {
            try
            {
                bool display_movie = false;
                string prompts = null;
                string[] menuOptions = { "Trước", "Tiếp", "Chọn Danh sách", "Quay lại" };
                int film_max = 0;
                for (int i = 0; i < mangtam.Count; i++)
                {
                    string F_name = Xem_Phim.Khong_Vuot_60(Convert.ToString(mangtam[i][1]));
                    prompts += $"{i + 1}. {F_name}";
                    if (i + 1 < mangtam.Count) prompts += "\n";
                }
                if (prompts.Split('\n').Length > 20) film_max = 20;

                Menu menu = new Menu(menuOptions, prompts);

                while (!display_movie)
                {
                    Console.Clear();
                    int selectedIndex = menu.Run(ref film_max);
                    switch (selectedIndex)
                    {
                        case 0:
                            film_max -= 20;
                            if (film_max < 20)
                            {
                                if (prompts.Split('\n').Length > 20) film_max = 20;
                                else film_max = 0;
                            }
                            break;
                        case 1:
                            film_max += 20;
                            if (film_max > prompts.Split('\n').Length)
                            {
                                if (prompts.Split('\n').Length > 20) film_max = prompts.Split('\n').Length;
                                else film_max = 0;
                            }
                            break;
                        case 2:
                            Ham_moi(mangtam, prompts, film_max, ac_Id);
                            break;
                        case 3:
                            display_movie = true;
                            break;

                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File không tồn tại.");
                Console.ReadLine();
            }
            catch (Exception r)
            {
                Console.WriteLine($"Error VF.User_Hienthi_Film: {r.Message}");
                Console.ReadLine();
            }
        }

        static void Ham_moi(List<List<string>> mangtam, string text, int film_max, int ac_Id)
        {
            try
            {
                List<string> movieOptions = new List<string>();
                int rac = 0;
                int check_num = 0;
                if (text.Split('\n').Length >= 20) check_num = film_max - 20;
                else film_max = text.Split('\n').Length;

                for (int i = check_num; i < film_max; i++)
                {
                    movieOptions.Add(text.Split('\n')[i]);
                }
                string[] menuOptions = movieOptions.ToArray();
                string prompts = "Chọn một trong số các bộ phim dưới đây";
                Menu menu = new Menu(menuOptions, prompts);
                int selectedIndex = menu.Run(ref rac);

                // Xử lý khi người dùng đã chọn một bộ phim
                if (selectedIndex >= 0 && selectedIndex < movieOptions.Count)
                {
                    int sele_num = selectedIndex + check_num;
                    string F_id = mangtam[sele_num][0];
                    // Cập nhập lượt xem của một bộ phim khi chọn phim đó để xem
                    Search_Film.Update_Luotxem(F_id, mangtam, sele_num);
                    string F_name = null;
                    string F_content = null;
                    // Kiểm tra liệu tên phim và nội dung có vượt quá giới hạn chữ là 60 không 
                    // Nếu có thì thực hiện in xuống dòng -> Tránh việc nó vượt qua WindowWidth gây ra lỗi khi cố in.
                    Search_Film.Khong_Vuot_60_Xuongdong(ref F_name, Convert.ToString(mangtam[sele_num][1]));
                    Search_Film.Khong_Vuot_60_Xuongdong(ref F_content, Convert.ToString(mangtam[sele_num][2]));

                    string thongbao = $"Tên Phim: {F_name}\n" +
                        $"Nội dung: {F_content}\nThể loại: {mangtam[sele_num][3]}\n" +
                        $"Nhà sản xuất: {Convert.ToString(mangtam[sele_num][4])}\nLượt Xem: {Convert.ToInt32(mangtam[sele_num][5])}\n" +
                        $"Rating: {Convert.ToDouble(mangtam[sele_num][6])}";
                    Menu_Film_User_D(thongbao, ac_Id, F_id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error VF_Hammoi: {e.Message}");
                Console.ReadLine();
            }
        }

        static void Menu_Film_User_D(string prompts, int ac_Id, string F_id)
        {
            try
            {
                string[] options_1 = { "Thêm Review", "Xem tất cả review", "Quay lại" };
                int rac = 0;
                Menu menu = new Menu(options_1, prompts);
                int choice = menu.Run(ref rac);

                switch (choice)
                {
                    case 0:
                        Console.Clear();
                        // Thêm review
                        For_User_Comment.AddReview(F_id, ac_Id); ; // truyền filmid, account Id
                        break;
                    case 1:
                        Console.Clear();
                        // Xem tất cả các review trong một bộ phim
                        For_User_Comment.ViewReview(F_id);
                        break;
                    case 2:
                        break;
                }
            }
            catch(Exception t)
            {
                Console.WriteLine($"Error VF.Menu_Film_User_D: {t.Message}");
                Console.ReadLine();
            }
        }
    }
}
