using Microsoft.Data.SqlClient;
using System;

// 1. ADIM: Enum Tanımlaması
enum Choices
{
    ch_noChoices = 0,
    ch_Login = 1,
    ch_Register = 2,
    ch_list = 3,
    ch_admin = 4,
    ch_numOfChoices = 5
};

class Program
{
    static void Main(string[] args)
    {
        // Veritabanı Bağlantı Cümlesi
        string connectionString = "Server=.\\sqlexpress; Database=BankaSistemi; Trusted_Connection=True; TrustServerCertificate=True;";

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== ATM SİSTEMİNE HOŞGELDİNİZ ===");
            Console.WriteLine($"{(int)Choices.ch_Login}- Giriş Yap");
            Console.WriteLine($"{(int)Choices.ch_Register}- Kayıt Ol");
            Console.WriteLine($"{(int)Choices.ch_list}- Kullanıcıları Listele");
            Console.WriteLine($"{(int)Choices.ch_admin}- Admin Paneli");
            Console.WriteLine("0- Çıkış");
            Console.Write("\nSeçiminiz: ");

            Choices secim = (Choices)Convert.ToInt32(Console.ReadLine());
            if (secim == 0) break;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                switch (secim)
                {
                    case Choices.ch_Login:
                        Console.Write("Kart Numarası: ");
                        string girilenKart = Console.ReadLine();
                        Console.Write("Şifre: ");
                        int girilenSifre = Convert.ToInt32(Console.ReadLine());

                        string loginSorgu = "SELECT KullanıcıId, Ad FROM Kullanıcılar WHERE KartNo = @KartNo AND Sifre = @Sifre";

                        using (SqlCommand cmd = new SqlCommand(loginSorgu, con))
                        {
                            cmd.Parameters.AddWithValue("@KartNo", girilenKart);
                            cmd.Parameters.AddWithValue("@Sifre", girilenSifre);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int aktifId = Convert.ToInt32(reader["KullanıcıId"]);
                                    string ad = reader["Ad"].ToString();
                                    Console.WriteLine($"\nGiriş Başarılı! Hoşgeldin {ad}.");
                                    reader.Close(); // İşlem yapabilmek için reader'ı kapatıyoruz.

                                    Console.WriteLine("1- Para Yatır\n2- Para Çek\n3- Bakiye Görüntüle");
                                    int islem = Convert.ToInt32(Console.ReadLine());

                                    if (islem == 1) // Para Yatır (UPSERT)
                                    {
                                        Console.Write("Yatırılacak Tutar: ");
                                        int miktar = Convert.ToInt32(Console.ReadLine());
                                        string upsertSorgu = @"
                                            IF EXISTS (SELECT 1 FROM Hesap WHERE KullanıcıId = @Id)
                                                UPDATE Hesap SET Bakiye = Bakiye + @Miktar WHERE KullanıcıId = @Id
                                            ELSE
                                                INSERT INTO Hesap (KullanıcıId, Bakiye) VALUES (@Id, @Miktar)";
                                        using (SqlCommand upCmd = new SqlCommand(upsertSorgu, con))
                                        {
                                            upCmd.Parameters.AddWithValue("@Id", aktifId);
                                            upCmd.Parameters.AddWithValue("@Miktar", miktar);
                                            upCmd.ExecuteNonQuery();
                                            Console.WriteLine("Bakiye güncellendi.");
                                        }
                                    }
                                    else if (islem == 2) // Para Çek (Bakiye Kontrollü UPDATE)
                                    {
                                        Console.Write("Çekilecek Tutar: ");
                                        int cekilecek = Convert.ToInt32(Console.ReadLine());

                                        // Önce bakiye kontrolü
                                        string bKontrol = "SELECT ISNULL(Bakiye, 0) FROM Hesap WHERE KullanıcıId = @Id";
                                        using (SqlCommand bCmd = new SqlCommand(bKontrol, con))
                                        {
                                            bCmd.Parameters.AddWithValue("@Id", aktifId);
                                            decimal suAnkiBakiye = Convert.ToDecimal(bCmd.ExecuteScalar() ?? 0);

                                            if (suAnkiBakiye >= cekilecek)
                                            {
                                                string cekSorgu = "UPDATE Hesap SET Bakiye = Bakiye - @Miktar WHERE KullanıcıId = @Id";
                                                using (SqlCommand cekCmd = new SqlCommand(cekSorgu, con))
                                                {
                                                    cekCmd.Parameters.AddWithValue("@Miktar", cekilecek);
                                                    cekCmd.Parameters.AddWithValue("@Id", aktifId);
                                                    cekCmd.ExecuteNonQuery();
                                                    Console.WriteLine($"İşlem başarılı. Kalan: {suAnkiBakiye - cekilecek} TL");
                                                }
                                            }
                                            else { Console.WriteLine("Yetersiz bakiye!"); }
                                        }
                                    }
                                    else if (islem == 3) // Bakiye Görüntüle
                                    {
                                        string bakiyeSorgu = "SELECT Bakiye FROM Hesap WHERE KullanıcıId = @Id";
                                        using (SqlCommand bCmd = new SqlCommand(bakiyeSorgu, con))
                                        {
                                            bCmd.Parameters.AddWithValue("@Id", aktifId);
                                            object bakiyeVal = bCmd.ExecuteScalar();
                                            Console.WriteLine($"Mevcut Bakiyeniz: {bakiyeVal ?? 0} TL");
                                        }
                                    }
                                }
                                else { Console.WriteLine("Hatalı bilgiler!"); }
                            }
                        }
                        break;

                    case Choices.ch_admin:
                        Console.Write("Admin Pin: ");
                        string pin = Console.ReadLine();
                        string adminSorgu = "SELECT 1 FROM AdminKullanıcı WHERE AdminPin = @Pin";
                        using (SqlCommand aCmd = new SqlCommand(adminSorgu, con))
                        {
                            aCmd.Parameters.AddWithValue("@Pin", pin);
                            if (aCmd.ExecuteScalar() != null)
                            {
                                Console.WriteLine("1- Kullanıcı Sil\n2- Listele");
                                int aSecim = Convert.ToInt32(Console.ReadLine());
                                if (aSecim == 1)
                                {
                                    Console.Write("Silinecek Kart No: ");
                                    string silinecekKart = Console.ReadLine();

                                    // FK HATASINI ÇÖZEN KISIM: İki aşamalı silme
                                    string idBul = "SELECT KullanıcıId FROM Kullanıcılar WHERE KartNo = @k";
                                    using (SqlCommand idCmd = new SqlCommand(idBul, con))
                                    {
                                        idCmd.Parameters.AddWithValue("@k", silinecekKart);
                                        object targetId = idCmd.ExecuteScalar();

                                        if (targetId != null)
                                        {
                                            int sid = (int)targetId;
                                            // 1. Önce bağlı olduğu Hesap tablosundaki veriyi siliyoruz
                                            string silHesap = "DELETE FROM Hesap WHERE KullanıcıId = @sid";
                                            using (SqlCommand hCmd = new SqlCommand(silHesap, con))
                                            {
                                                hCmd.Parameters.AddWithValue("@sid", sid);
                                                hCmd.ExecuteNonQuery();
                                            }
                                            // 2. Şimdi ana kullanıcı kaydını silebiliriz
                                            string silKullanici = "DELETE FROM Kullanıcılar WHERE KullanıcıId = @sid";
                                            using (SqlCommand kCmd = new SqlCommand(silKullanici, con))
                                            {
                                                kCmd.Parameters.AddWithValue("@sid", sid);
                                                kCmd.ExecuteNonQuery();
                                            }
                                            Console.WriteLine("Kullanıcı ve bağlı tüm veriler silindi.");
                                        }
                                        else { Console.WriteLine("Kullanıcı bulunamadı."); }
                                    }
                                }
                            }
                            else { Console.WriteLine("Yetkisiz erişim!"); }
                        }
                        break;
                }
            }
            Console.WriteLine("\nDevam etmek için bir tuşa basın...");
            Console.ReadKey();
        }
    }
}