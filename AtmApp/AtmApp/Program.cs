using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

string KartNo = "", KartAdi = "", KartSifre="";
string dosya = "Kullanıcı.txt";
string bakiye = "Bakiye.txt";

Console.WriteLine("1-Kayıt Ol, 2-Giriş Yap");
int secim =Convert.ToInt32(Console.ReadLine());

switch (secim)
{
    case 1:
        Console.Write("Kart No Giriniz: ");
        KartNo = Console.ReadLine();
        Console.Write("Kart Adı Giriniz: ");
        KartAdi = Console.ReadLine();
        Console.Write("Kart Sifresi Giriniz: ");
        KartSifre = Console.ReadLine();

        string yenisatir = $"{KartNo};{KartAdi};{KartSifre}"+Environment.NewLine;
        File.AppendAllText(dosya, yenisatir);
        break;

    case 2:
        Console.Write("Kart No Giriniz: ");
        string girilenkartno= Console.ReadLine();
        if (File.Exists(dosya))
        {
            string[] satirlar = File.ReadAllLines(dosya);
            bool kartbulundu = false;
            foreach(string satir in satirlar)
            {
                if (string.IsNullOrWhiteSpace(satir)) continue;
                string[] parcalar=satir.Split(';');
                if (parcalar[0] ==girilenkartno)
                {
                    kartbulundu=true;
                    Console.Write("Şifrenizi Giriniz: ");
                    string girilensifre=Console.ReadLine();

                    if (parcalar[2] == girilensifre)
                    {
                        KartNo = parcalar[0];
                        KartAdi = parcalar[1];
                        Console.WriteLine("Hoşgeldiniz: " + KartAdi);
                        Console.WriteLine("Yapmak istediğiniz işlem nedir?");
                        Console.Write("1-Para Yatırma, 2-Para Çekme, 3-Bakiye Görüntüleme");
                        int islem =Convert.ToInt32(Console.ReadLine());
                        if(islem == 1)
                        {
                            Console.WriteLine("Yatırmak İstediğiniz Tutarı Giriniz: ");
                            string tutar=Console.ReadLine();
                            string bakiyesatir = $"{KartNo};{tutar}"+ Environment.NewLine;
                            File.AppendAllText(bakiye, bakiyesatir);

                        }

                        else if (islem == 2)
                        {
                            Console.Write("Çekmek İstediğiniz Tutarı Giriniz: ");
                            string cektutarInput = Console.ReadLine();
                            int cektutar = Convert.ToInt32(cektutarInput);

                            if (File.Exists(bakiye))
                            {
                                string[] bakiyeSatirlari = File.ReadAllLines(bakiye);
                                bool bulundumu = false;

                                for (int i = 0; i < bakiyeSatirlari.Length; i++)
                                {
                                    // Satırı parçalara ayırıyoruz (Örn: "123;500")
                                    string[] parcalarr = bakiyeSatirlari[i].Split(';');

                                    if (parcalarr.Length >= 2 && parcalarr[0] == KartNo)
                                    {
                                        int mevcutBakiye = Convert.ToInt32(parcalarr[1]);

                                        if (mevcutBakiye >= cektutar)
                                        {
                                            int kalanBakiye = mevcutBakiye - cektutar;

                                            // SADECE bu satırı güncelliyoruz: "KartNo;YeniBakiye"
                                            bakiyeSatirlari[i] = KartNo + ";" + kalanBakiye.ToString();
                                            bulundumu = true;
                                            Console.WriteLine($"İşlem başarılı. Kalan bakiye: {kalanBakiye}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Yetersiz bakiye!");
                                            bulundumu = true; // Kart bulundu ama para yetersiz
                                        }
                                        break; // Kartı bulduğumuz için döngüden çıkabiliriz
                                    }
                                }

                                if (bulundumu)
                                {
                                    // Tüm satırları (güncellenmiş haliyle) dosyaya geri yazıyoruz
                                    File.WriteAllLines(bakiye, bakiyeSatirlari);
                                }
                                else
                                {
                                    Console.WriteLine("Hata: Bakiye kaydı bulunamadı.");
                                }
                            }
                        }
                        else if(islem == 3)
                        {
                            Console.WriteLine("Bakiyeniz: ");
                        }

                        
                    }   
                    else { Console.WriteLine("Yanlış Şifre."); }
                    break;
                }
            }
            if (!kartbulundu) {
                Console.WriteLine("Hata: Bu kart numarasına ait bir kayıt bulunamadı.");
            }

        }
        else
        {
            Console.WriteLine("Hata: Henüz hiç kayıtlı kullanıcı yok.");
        }
        break;
    default:
        {
            Console.WriteLine("Hata: Henüz hiç kayıtlı kullanıcı yok.");
        }
        break;
}