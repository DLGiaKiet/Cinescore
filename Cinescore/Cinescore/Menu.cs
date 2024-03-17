using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outside_Interface
{
    internal class Menu
    {
        private string Prompts;
        private string[] Options;
        private int SelectedIndex;
        private bool Flag;

        // Khởi tạo một constructor -> Thuận tiện khi tạo một object
        public Menu(string[] options, string prompts, bool flag = true, int selected_in = 0)
        {
            Prompts = prompts;
            Flag = flag;
            Options = options;
            SelectedIndex = selected_in;
        }

        // Hàm chạy các lựa chọn
        private void Display(int film_max) // Cơ chế hiển thị tiếp -> Nếu chạm mức giới hạn -> Trả về số tới giới hạn = dòng cuối + 4
            // In ra thông báo lựa chọn thứ dòng cuối + 1 để chuyển sang hiển thị tiếp
        {
            try 
            {
                int num_title = 2;
                int max = 0;
                if (!Flag)
                {
                    Print_title();
                    num_title = 0;
                }
                else Print_Prompts(film_max);

                if (film_max == 0) film_max = Prompts.Split('\n').Length;
                else film_max = 20;

                // In ra dòng có độ dài lớn nhất, làm chuẩn cho việc in ra khung
                for (int i = 0; i < Options.Length; i++)
                {
                    if (Options[i].Length > max) max = Options[i].Length;
                    else continue;
                }

                string text = null;

                // Mục đích khi đến chọn film -> Lúc này số danh sách film_max được truyền vào = 0
                // -> t2 là cách tính toán cộng với số dòng trong Prompts (Lưu ý Prompts này khác với Prompts có danh sách phim
                // Nó có thể là bất cứ thông báo nào tùy thuộc vào bạn) và hai khung trên dưới.
                // Mục đích nhằm tính số dòng sẽ in ra khung và Options -> Options lúc này chứa danh sách các film
                int t2 = film_max + num_title;
                Console.SetCursorPosition(0, t2);
                Console.WriteLine('╔' + new string('═', 16 + max) + '╗');

                for (int i = 0; i < Options.Length; i++)
                {
                    text = Options[i];
                    char prefix;
                    Console.SetCursorPosition(0, t2 + 1 + i);
                    Console.Write("║");
                    Console.SetCursorPosition(6, t2 + 1 + i);
                    if (i == SelectedIndex)
                    {
                        prefix = '>';
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        prefix = ' ';
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write($"{new string(prefix, 2)}--{text}--");
                    Console.SetCursorPosition(17 + max, t2 + 1 + i);
                    Console.ResetColor();
                    Console.WriteLine("║");
                }

                Console.SetCursorPosition(0, t2 + Options.Length + 1);
                Console.WriteLine('╚' + new string('═', 16 + max) + '╝');
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error Display: {ex.Message}");
            }
        }

        // In ra một tiêu đề bình thường
        // Dù ở các hàm khác cũng có biến film_max nhưng nó đóng vai trò chủ yếu ở hàm Print_Prompt
        private void Print_Prompts(int film_max)
        {
            int max = 0;
            int start_film = 0;
            int a = 0;

            // Đk bên dưới dùng để phân biệt khi nào in bình thường, khi nào thực hiện in danh sách các phim
            // Nếu in danh sách các film thì thiết lập start_film mới
            // Nếu in bình thường thì thiết lập film_max -> Phục vụ cho hàm for
            if (film_max > 0) start_film = film_max - 20;
            else film_max = Prompts.Split('\n').Length;

            // Tìm dòng có độ dài lớn nhất, từ đó lấy đó làm chuẩn để tính toán độ dài các khung
            for (int i = start_film; i < film_max; i++)
            {
                if (Prompts.Split('\n')[i].Length > max) max = Prompts.Split('\n')[i].Length;
                else continue;
            }

            Console.SetCursorPosition(0, 0);
            Console.WriteLine('╔' + new string('═', 10 + max) + '╗');

            for (int i = start_film; i < film_max; i++)
            {
                Console.SetCursorPosition(0, 1 + a);
                Console.Write("║");
                Console.SetCursorPosition(6, 1 + a);
                Console.WriteLine($"{Prompts.Split('\n')[i]}");
                Console.SetCursorPosition(11 + max, 1 + a);
                Console.WriteLine("║");
                a++;
            }
            // Phép tính dưới là tìm số dòng của prompts + dòng trống -> Rồi mới thực hiện in khung dưới
            // Câu hỏi: Tại sao lại là film_max vs start_film mà không phải là Prompts.Split('\n').Length
            // Answer: Đơn giản là khi danh sách film ở dưới 20, ta không thể thấy rõ
            // Nếu trong trường hợp nó trên 20, cụ thể là 26 -> Tức là Prompts lúc này là 26 dòng
            // --> Việc này sẽ làm cho việc in ra bị lỗi -> Khi này film_max sẽ là cứu cánh
            // Khi giới hạn cho việc thiết lập nó chỉ dừng lại ở mức 20
            // Lúc bắt đầu film_max = 20 nó sẽ in ra danh sách film từ dòng 0 đến dòng 20 -> Dù thực tế lúc đó
            // Trong Prompts có 26 dòng -> Nếu chọn case 1 trong Xem_Phim.cs film_max = Prompts.Split('\n').Length = 26
            // start_film = 6 -> Nó sẽ in từ dòng 6 đến dòng 26 -> Mặc dù tương tự Prompts lúc đó có 26 dòng
            Console.SetCursorPosition(0, film_max - start_film + 1);
            Console.WriteLine('╚' + new string('═', 10 + max) + '╝');
        }

        public int Run(ref int film_max)
        {
            ConsoleKey Keypress;
            do
            {
                Console.Clear();
                Display(film_max);
                var key = Console.ReadKey(intercept: true).Key;
                Keypress = key;

                if (Keypress == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex < 0)
                    {
                        SelectedIndex = Options.Length - 1;
                    }
                }
                else if (Keypress == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex == Options.Length)
                    {
                        SelectedIndex = 0;
                    }
                }
            } while (Keypress != ConsoleKey.Enter);

            return SelectedIndex;
        }

        // In ra các tiêu đề như: Logo UEH, Logo App, những thứ nhiều ký tự '\n'
        private void Print_title()
        {
            for (int i = 0; i < Prompts.Split('\n').Length; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine($"{Prompts.Split('\n')[i]}");
            }
        }
    }
}
