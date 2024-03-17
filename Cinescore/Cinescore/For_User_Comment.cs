using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outside_Interface;

namespace User
{
    internal class For_User_Comment
    {
        static string filePath1 = @"danhsachcmtid.txt";
        static string filePath2 = @"Movie0.txt";
        static string filePath3 = @"Truyvan.txt";

        //Hàm tính điểm rating trung bình
        static void AverageRating(string filmid)
        {
            try
            {
                List<string> reviews = File.ReadAllLines(filePath1).ToList();
                List<string> movie_1 = File.ReadAllLines(filePath2).ToList();

                double rating = 0;
                int count = 0;

                foreach (string review in reviews)
                {
                    string[] reviewData = review.Split(',');

                    if (reviewData[1] == filmid)
                    {
                        rating += double.Parse(reviewData[7]);
                        count++;
                    }             
                }
                double vag_rating = Math.Round(rating / count, 2);

                for(int i = 0; i < movie_1.Count; i++)
                {
                    string[] movieData = movie_1[i].Split(',');
                    if (movieData[0] == filmid)
                    {
                        movieData[6] = Convert.ToString(vag_rating);
                        string text = $"{movieData[0]},{movieData[1]},{movieData[2]},{movieData[3]},"+
                            $"{movieData[4]},{movieData[5]},{movieData[6]}";
                        movie_1[i] = text;
                        break;
                    }
                }    
                // Ghi dữ liệu đã được cập nhật vào filePath2
                File.WriteAllLines(filePath2, movie_1);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error F_U_C.AverageRatting: {e.Message}");
            }
        }

        //Hàm thêm review mới của người dùng nhập vào
        public static void AddReview(string filmid, int accountId)
        {
            try
            {
                List<string> movies = File.ReadAllLines(filePath2).ToList();
                List<string> reviews_root = File.ReadAllLines(filePath1).ToList();

                string cmt = Check_Nhapvao.Themdulieu_string("Bình luận của bạn: ");
                double rating = Check_Nhapvao.DoubleInputWithPrompt("Số điểm Rating của bạn: ", 0, 10, true);

                //Cập nhật cmt id (nếu trong TH không có sẽ bắt đầu = 1)
                int commentIdCounter = File.ReadAllLines(filePath1).Length + 1;
                DateTime now = DateTime.Now;
                int year = now.Year;
                int month = now.Month;
                int day = now.Day;

                string review = $"{commentIdCounter},{filmid},{accountId},{cmt},{year},{month},{day},{rating}";

                reviews_root.Add(review);

                //ghi dữ liệu vừa thêm vào cuối tệp tin filePath1.
                File.WriteAllLines(filePath1, reviews_root); 

                Console.WriteLine("Bình luận của bạn đã được thêm thành công.");
                Search_Film.Print_Normal($"Bình luận: {cmt}\nRating: {rating.ToString()}");

                AverageRating(filmid);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Hỗ trợ cho hàm EditReview() để người dùng có thể xem các comment của mình từng comment
        public static void ViewUserComments(int accountId)
        {
            List<string> reviews = File.ReadAllLines(filePath1).ToList();
            List<string> movies = File.ReadAllLines(filePath2).ToList();
            bool found = false;

            foreach (string review in reviews)
            {
                string[] reviewData = review.Split(',');

                if (int.Parse(reviewData[2]) == accountId)
                {
                    int commentId = int.Parse(reviewData[0]);
                    int filmId = int.Parse(reviewData[1]);
                    string filmName = GetMovieName(filmId.ToString(), movies);
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
                Console.WriteLine("Bạn chưa có bình luận nào. Hãy tìm kiếm và thực hiện bình luận.");
            }
        }

        //Sửa review
        public static void EditReview(int cmtid)
        {
            try
            {
                List<string> reviews = File.ReadAllLines(filePath1).ToList();
                bool flag = false;
                string filmid_1 = null;

                for (int i = 0; i < reviews.Count; i++)
                {
                    string[] reviewData = reviews[i].Split(',');

                    if (int.Parse(reviewData[0]) == cmtid)
                    {
                        filmid_1 = reviewData[1];
                        Console.WriteLine("Bình luận hiện tại:");
                        Search_Film.Print_Normal($"Bình luận: {reviewData[3]}\nRating: {reviewData[7]}");

                        Console.WriteLine("Nhập các thông tin mới.");
                        string cmt = Check_Nhapvao.Themdulieu_string("Bình luận mới của bạn: ");
                        double rating = Check_Nhapvao.DoubleInputWithPrompt("Số điểm Rating mới của bạn: ", 0, 10, true);

                        reviewData[3] = cmt;
                        reviewData[7] = rating.ToString();

                        reviews[i] = string.Join(",", reviewData);
                        File.WriteAllLines(filePath1, reviews);

                        Console.WriteLine("Bình luận đã được cập nhật thành công.");
                        Console.WriteLine("Bình luận sau chỉnh sửa!");
                        Search_Film.Print_Normal($"Bình luận: {cmt}\nRating: {rating.ToString()}");

                        //Tính lại điểm rating trung bình sau khi sửa
                        AverageRating(filmid_1);
                        flag = true;
                        break;
                    }
                }

                if (!flag) Console.WriteLine("Bạn chưa có bình luận nào. Hãy tìm kiếm và thực hiện bình luận.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Xem review về 1 bộ phim nào đó mà người dùng nhập vào
        public static void ViewReview(string filmid)
        {
            int exist_u_r = 0;
            try
            {
                List<string> movies = File.ReadAllLines(filePath2).ToList();
                List<string> reviews = File.ReadAllLines(filePath1).ToList();

                foreach (string movie in movies)
                {
                    string[] movieData = movie.Split(',');

                    if (movieData[0] == filmid)
                    {
                        string movieName = movieData[1];
                        Console.WriteLine($"Dưới đây là các bình luận về phim {movieName}:");
                        break;
                    }
                }

                foreach (string review in reviews)
                {
                    string[] reviewData = review.Split(',');

                    string film_Id = reviewData[1];

                    if (film_Id != null && film_Id == filmid)
                    {
                        string user_id = Get_usern_fvr(reviewData[2]);
                        if (user_id != null)
                        {
                            string cmt = reviewData[3];
                            int year = Convert.ToInt32(reviewData[4]);
                            int month = Convert.ToInt32(reviewData[5]);
                            int day = Convert.ToInt32(reviewData[6]);
                            double rating = double.Parse(reviewData[7]);
                            Search_Film.Print_Normal($"--{user_id}--   {year}-{month}-{day}\nBình luận: {reviewData[3]}\nRating: {reviewData[7]}");
                            exist_u_r++;
                        }
                    }
                }
                if (exist_u_r == 0) Console.WriteLine("Bộ phim này hiện chưa có review nào.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        //Lấy tên phim thích hợp với phim id được gán trong bảng dữ liệu
        public static string GetMovieName(string filmId, List<string> movies)
        {
            foreach (string movie in movies)
            {
                string[] movieData = movie.Split(',');

                if (movieData.Length >= 2 && movieData[0] == filmId)
                {
                    return movieData[1];
                }
            }

            return null;
        }

        // Bổ sung cho hàm ViewReview
        // Thực hiện kiểm tra nếu account Id không tồn tại thì trả về null
        // Nếu tồn tại thì trả về tên của user dựa theo Account id của user đó
        static string Get_usern_fvr(string u_id)
        {
            List<string> ac_list = File.ReadAllLines(filePath3).ToList();
            foreach (string rine in ac_list)
            {
                string[] ac_field = rine.Split(',');

                if (ac_field[0] == u_id)
                {
                    return ac_field[1];
                }
            }

            return null;
        }

        //Kiểm tra xem commentId có tồn tại không
        public static int GetValidCommentId(int accountId)
        {
            int cmtid = 0;
            List<string> reviews = File.ReadAllLines(filePath1).ToList();
            bool commentExists = false;

            while (!commentExists)
            {
                cmtid = Check_Nhapvao.Themdulieu_int("Nhập comment id bạn muốn chỉnh sửa: ");

                // Kiểm tra xem commentId có tồn tại trong dữ liệu không
                foreach (string review in reviews)
                {
                    string[] reviewData = review.Split(',');

                    if (int.Parse(reviewData[0]) == cmtid && int.Parse(reviewData[2]) == accountId)
                    {
                        commentExists = true;
                        break;
                    }
                }

                if (!commentExists)
                {
                    Console.Clear();
                    Console.WriteLine($"Không tìm thấy commentId {cmtid}. Vui lòng nhập lại.");
                }
            }
            return cmtid;
        }
    }
}
