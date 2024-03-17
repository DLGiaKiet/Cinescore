using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outside_Interface;
using User;

namespace Admin
{
    internal class CommentManager
    {
        public static string dataFilePath = @"danhsachcmtid.txt";
        static string FilePath_2 = @"Movie0.txt";

        public static void Comment_Control()
        {
            // UIHelper thành Menu lựa chọn, Hiển thị danh sách cmtid, Xoá comment theo Account ID, Thoát 
            string[] line_2 = File.ReadAllLines(FilePath_2);
            string[] options_4 = { "Hiển thị tất cả các Comment", "Xóa comment dựa trên User ID", "Các comment có điểm nhỏ hơn 3", "Quay lại" };
            string prompts = "Quản Lý Comment của User ID";
            bool stop_flag = false;
            int rac = 0;

            while(!stop_flag)
            {
                Menu menu = new Menu(options_4, prompts);
                int choice = menu.Run(ref rac);
                switch (choice)
                {
                    case 0:
                        // Xem tất cả các review
                        Console.Clear();
                        // Điều kiện phòng trường hợp không có phim nào trong Movie0.txt
                        if(line_2.Length > 0) DisplayAllComments();
                        else
                        {
                            Console.WriteLine("Hiện tại không có phim. Nên không có review nào.\nVui lòng nhập phim mới");
                            Console.ReadLine();
                        } 
                            
                        break;

                    case 1:
                        // Xóa review dựa trên Account Id của User
                        Console.Clear();
                        if (line_2.Length > 0) DeleteCommentByAccountId(); 
                        else
                        {
                            Console.WriteLine("Hiện tại không có phim. Nên không có review nào.\nVui lòng nhập phim mới");
                            Console.ReadLine();
                        }
                        break;

                    case 2:
                        Console.Clear();
                        if (line_2.Length > 0) Ratingsmallerthan3();
                        else
                        {
                            Console.WriteLine("Hiện tại không có phim. Nên không có review nào.\nVui lòng nhập phim mới");
                            Console.ReadLine();
                        }

                        break;

                    case 3:
                        stop_flag = true;
                        break;
                }
            }    
        }

        public static void DeleteCommentByAccountId()
        {
            Console.Write("Nhập Account ID để hiển thị tất cả các comment và số lần comment: ");
            if (int.TryParse(Console.ReadLine(), out int accountId))
            {
                // Đọc toàn bộ nội dung từ tệp văn bản vào một danh sách
                string[] lines = File.ReadAllLines(dataFilePath);

                bool found = false;
                Console.Clear();
                Console.WriteLine($"Danh sách tất cả các comments của Account ID {accountId}:\n");

                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length >= 3)
                    {
                        int currentAccountId = int.Parse(parts[2]);

                        // Nếu Account ID trùng khớp, hiển thị thông tin comment, Comment ID và Film ID
                        if (currentAccountId == accountId)
                        {
                            int cmtId = int.Parse(parts[0]);
                            int filmId = int.Parse(parts[1]);
                            string comment = parts[3];

                            Search_Film.Print_Normal($"CMT ID: {cmtId}, Film ID: {filmId}, Comment: {comment}");
                            found = true;
                        }
                    }
                }

                if (!found)
                {
                    Console.WriteLine("Không tìm thấy comment với Account ID này.");
                }
                else
                {
                    // Đếm số lần comment của Account ID
                    int commentCount = CountCommentsByAccountId(accountId);
                    Console.WriteLine($"\nSố lần comment của Account ID {accountId}: {commentCount}\n");

                    Console.Write("Nhập Comment ID để xoá (hoặc Enter để bỏ qua): ");
                    if (int.TryParse(Console.ReadLine(), out int commentIdToDelete))
                    {
                        // Xoá comment dựa trên Comment ID và giảm số lần comment của Account ID
                        DeleteCommentByCommentId(commentIdToDelete, accountId);
                    }
                    else
                    {
                        Console.WriteLine("Không nhập hoặc nhập một giá trị không hợp lệ cho Comment ID.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Vui lòng nhập một số nguyên cho Account ID.");
            }
            Console.ReadLine();
        }

        public static void DeleteCommentByCommentId(int commentId, int accountId)
        {
            // Đọc toàn bộ nội dung từ tệp văn bản vào một danh sách
            string[] lines = File.ReadAllLines(dataFilePath);

            bool found = false;

            // Tạo một StringBuilder để lưu trữ dữ liệu mới
            StringBuilder newData = new StringBuilder();

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 1)
                {
                    int currentCmtId = int.Parse(parts[0]);
                    int currentAccountId = int.Parse(parts[2]);

                    // Nếu Comment ID trùng khớp và Account ID trùng khớp, không thêm dòng này vào newData
                    if (currentCmtId == commentId && currentAccountId == accountId)
                    {
                        found = true;
                        continue;
                    }
                }
                newData.AppendLine(line);
            }

            // Ghi dữ liệu mới vào tệp văn bản
            File.WriteAllText(dataFilePath, newData.ToString());

            if (found)
            {
                // Giảm số lần comment của Account ID
                DecreaseCommentCount(accountId);
                Console.WriteLine("Xoá comment thành công và giảm số lần comment của Account ID.");
            }
            else
            {
                Console.WriteLine("Không tìm thấy comment với Comment ID này hoặc có lỗi xảy ra khi xoá.");
            }
        }

        public static int CountCommentsByAccountId(int accountId)
        {
            // Đọc toàn bộ nội dung từ tệp văn bản vào một danh sách
            string[] lines = File.ReadAllLines(dataFilePath);

            int commentCount = 0;

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 3)
                {
                    int currentAccountId = int.Parse(parts[2]);

                    // Nếu Account ID trùng khớp, tăng biến đếm số lần comment
                    if (currentAccountId == accountId)
                    {
                        commentCount++;
                    }
                }
            }

            return commentCount;
        }

        public static void DecreaseCommentCount(int accountId)
        {
            // Đọc toàn bộ nội dung từ tệp văn bản vào một danh sách
            string[] lines = File.ReadAllLines(dataFilePath);

            // Tạo một StringBuilder để lưu trữ dữ liệu mới
            StringBuilder newData = new StringBuilder();

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 3)
                {
                    int currentAccountId = int.Parse(parts[2]);

                    // Nếu Account ID trùng khớp, giảm số lần comment
                    if (currentAccountId == accountId)
                    {
                        int commentCount = CountCommentsByAccountId(accountId);
                        commentCount--;

                        // Thêm dòng mới với số lần comment cập nhật
                        newData.AppendLine($"{parts[0]},{parts[1]},{currentAccountId},{commentCount},{parts[4]}");
                    }
                    else
                    {
                        newData.AppendLine(line);
                    }
                }
            }

            // Ghi dữ liệu mới vào tệp văn bản
            File.WriteAllText(dataFilePath, newData.ToString());
        }

        public static void DisplayAllComments()
        {
            // Đọc toàn bộ nội dung từ tệp văn bản
            string[] lines = File.ReadAllLines(dataFilePath);

            Console.WriteLine("Danh sách tất cả các comments:");

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    int cmtid = int.Parse(parts[0]);
                    int filmid = int.Parse(parts[1]);
                    int accountId = int.Parse(parts[2]);
                    string cmt = parts[3];

                    Search_Film.Print_Normal($"CMT ID: {cmtid}, Film ID: {filmid}, Account ID: {accountId}, Comment: {cmt}");
                }
            }
            Console.ReadLine();
        }
        public static void Ratingsmallerthan3()
        {
            List<string> reviews = File.ReadAllLines(dataFilePath).ToList();
            List<string> movies = File.ReadAllLines(FilePath_2).ToList();
            bool found = false;

            foreach (var review in reviews)
            {
                string[] reviewData = review.Split(',');
                if (double.Parse(reviewData[7]) < 3)
                {
                    int commentId = int.Parse(reviewData[0]);
                    int filmId = int.Parse(reviewData[1]);
                    string filmName = For_User_Comment.GetMovieName(filmId.ToString(), movies);
                    string cmt = reviewData[3];
                    int year = Convert.ToInt32(reviewData[4]);
                    int month = Convert.ToInt32(reviewData[5]);
                    int day = Convert.ToInt32(reviewData[6]);
                    double rating = double.Parse(reviewData[7]);
                    string text = $"{year}-{month}-{day}\nComment ID: {commentId.ToString()}\nFilm ID: {filmId.ToString()}\n" +
                        $"Tên Phim: {filmName}\nBình luận: {cmt}\nRating: {rating.ToString()}";

                    Search_Film.Print_Normal(text);
                    Console.WriteLine();
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("Không tìm thấy bình luận có điểm đánh giá nhỏ hơn 3.");
            }
            Console.ReadLine();
        }
    }
}
