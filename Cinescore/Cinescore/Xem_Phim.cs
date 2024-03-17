using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outside_Interface;
using User;
using static System.Net.Mime.MediaTypeNames;

namespace Admin
{
    internal class Xem_Phim
    {
        static string dataFilePath = @"Movie0.txt";

        public static void DisplayAllMovieNames(int edit_num = 0)
        {
            string[] lines = File.ReadAllLines(dataFilePath);
            // mangtam được tạo nhằm mục đích 1. phục vụ cho việc khi chọn vào một phim để hiện thị thông tin
            // Nó sẽ trả về đúng thông tin đấy cho chúng ta, bất kể film id có khác nhau
            // 2. Không cần phụ thuộc quá nhiều vào film id
            List<List<string>> mangtam = new List<List<string>>();
            string prompts = null;
            bool display_movie = false;

            for (int i = 0; i < lines.Length; i++)
            {
                // Đọc từng trường dữ liệu và tách nó ra theo dấu phẩy
                // và lấy theo từng dữ liệu trong trường đó theo cột
                string[] movieData = lines[i].Split(','); 
                List<string> m_1 = new List<string>(movieData);

                // Kiểm tra xem có đủ thông tin để hiển thị film ID và tên phim không
                if (movieData.Length > 0)
                {
                    // Lấy ra film ID
                    string filmId = movieData[0];
                    // dùng hàm Khong_Vuot_60 để hạn chế việc nếu tên phim quá dài dẫn đến việc in ra bị lỗi
                    // Ta chỉ cần lấy số lượng từ vừa đúng giới hạn 60 bất kể nó dài đến đâu
                    // Dù nó có hạn chế là sẽ làm tên phim trở nên thiếu rõ ràng do sự không đầy đủ đó
                    string movieName = Khong_Vuot_60(Convert.ToString(movieData[1]));
                    mangtam.Add(m_1);
                    prompts += $"{i + 1}. Tên Phim: {movieName}";
                    // Nhằm mục đích đến dòng cuối thì không thực hiện xuống dòng.
                    if (i + 1 < lines.Length) prompts += "\n";
                }
            }

            string[] menuOptions = { "Trước", "Tiếp", "Chọn Danh sách", "Quay lại"};
            // Nguyên nhân cho sự tồn tại của film_max:
            // Dùng để giới hạn khi truyền vào biến prompt đến Menu.cs, biến chỉ có số dòng giới hạn là 20
            // Nhằm để tránh việc nếu biến prompt không có giới hạn, khi in nó sẽ vượt qua WindowHeight -> Dẫn đến
            // Việc in ra bị lỗi
            // Thế tại sao không tạo một hàm mới để có thể fix bug lỗi này?
            // Nhằm tận dụng tối đa hàm Menu -> Tránh việc tạo ra quá nhiều hàm giống nhau về chức năng mà chỉ dùng 
            // Duy nhất có một lần
            int film_max = 0;
            // Điều kiện phòng trường hợp trường dữ liệu trong file Movie0 nhỏ hơn 20
            // Thì thực hiện việc in ra bình thường không cần đến sự tồn tại của film_max
            // Vì nó nhỏ hơn giới hạn 20 -> Nên việc in sẽ không ra lỗi
            if (prompts.Split('\n').Length > 20) film_max = 20;

            Menu menu = new Menu(menuOptions, prompts);

            while (!display_movie)
            {
                Console.Clear();
                int selectedIndex = menu.Run(ref film_max);
                switch (selectedIndex)
                {
                    case 0:
                        // Thực hiện tra cứu danh mục film ở phía trước
                        film_max -= 20;
                        if (film_max < 20)
                        {
                            if (prompts.Split('\n').Length > 20) film_max = 20;
                            else film_max = 0;
                        }
                        break;
                    case 1:
                        // Thực hiện tra cứu danh mục film ở phía sau
                        film_max += 20;
                        if (film_max > prompts.Split('\n').Length)
                        {
                            if (prompts.Split('\n').Length > 20) film_max = prompts.Split('\n').Length;
                            else film_max = 0;
                        }
                        break;
                    case 2:
                        // Thực hiện các chức năng sâu hơn
                        // Như là 1. hiển thị tất cả thông tin một phim mà ta đã chọn
                        // 2. Thực hiện chỉnh sửa lại thông tin phim đã chọn
                        Ham_moi(mangtam, prompts, film_max, edit_num);
                        break;
                    case 3:
                        display_movie = true;
                        break;

                }
            }       
        }
        // tham số edit_num tồn tại để phân biệt khi nào in ra hiển thị tất cả thông tin phim
        // Khi nào dùng để chỉnh sửa thông tin phim
        static void Ham_moi(List<List<string>> mangtam, string text, int film_max,int edit_num)
        {
            List<string> movieOptions = new List<string>();
            // Biến rac tồn tại để truyền vào menu.Run() vì tham số của nó là một ref int 
            // Tránh việc bị lỗi, đồng thời đây cũng là thông báo cho thấy việc prompt có số dòng nhỏ hơn
            // giới hạn (20)
            int rac = 0;
            // Biến check_num tồn tại như một giá trị khởi đầu của danh sách các phim
            // Nếu ví dụ có khoảng 26 bộ phim trong Movie0
            // Nếu bắt đầu check_num nó bằng 0 vì khi này film_max bằng 20 -> Nó thực hiện điều kiện bên dưới
            // Nếu ta chọn case 1 ở Switch case trong hàm trên -> Nó sẽ yêu cầu chuyển tiếp
            // Khi này film_max -> tới 26 -> check_num -> 6
            // Dùng trong hàm for và hỗ trợ việc trả về thông tin dựa vào hàm mangtam một cách chính xác hơn.

            int check_num = 0;// Khi film_max = 0 hoặc nhỏ hơn -> Tức là ở điều kiện bình thường
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
                int sele_num = selectedIndex + check_num; // Nhằm tăng tính chính xác trong index của mangtam khi
                // Trả về thông tin
                int F_id = Convert.ToInt32(mangtam[sele_num][0]);
                string F_name = Convert.ToString(mangtam[sele_num][1]);

                string thongbao = $"Tên Phim: {F_name}\n" +
                    $"Nội dung: {mangtam[sele_num][2]}\nThể loại: {mangtam[sele_num][3]}\n" +
                    $"Nhà sản xuất: {Convert.ToString(mangtam[sele_num][4])}\nLượt Xem: {Convert.ToInt32(mangtam[sele_num][5])}\n" +
                    $"Rating: {Convert.ToDouble(mangtam[sele_num][6])}";

                if (edit_num == 0) Search_Film.Print_Normal(thongbao);
                else Admin_Interface.EditMovie(F_id, thongbao, mangtam[selectedIndex]);
                Console.ReadLine();
            }
        }

        public static string Khong_Vuot_60(string moviedata)
        {
            int col = 0;
            string prompt = null;
            if(moviedata.Length >= 60)
            {
                string[] text = moviedata.Split(' ');
                for (int i = 0; i < text.Length; i++)
                {
                    if (col + text[i].Length <= 60)
                    {
                        prompt += text[i];
                        col += text[i].Length;
                    }
                    else break;

                    if (i + 1 < text[i].Length) prompt += " ";
                }    
            }    
            else
            {
                prompt = moviedata;
            }

            return prompt;
        }
    }
}
