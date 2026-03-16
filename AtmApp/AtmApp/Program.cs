using System.IO;
using System.Threading.Channels;
class BankaHesap {
    public int Kartno;
    int sifre ;
    int bakiye;
    string kullanıcıAdı;
    static void Main(string[] args)
    {
       
        int girkartno, girsifre, yanlıssayı = 0, secim, yatpara, cekpara;

        BankaHesap SefaKart = new BankaHesap();
        SefaKart.Kartno = 15061406;
        SefaKart.sifre = 1406;
        SefaKart.bakiye = 0;
        SefaKart.kullanıcıAdı = "Sefa Koçak";

        BankaHesap NilKart = new BankaHesap();
        NilKart.Kartno = 12345678;
        NilKart.sifre = 1234;
        NilKart.bakiye = 7500;
        NilKart.kullanıcıAdı = "nil";

        BankaHesap ZeyKart = new BankaHesap();
        ZeyKart.Kartno = 10121972;
        ZeyKart.sifre = 4321;
        ZeyKart.bakiye = 1200;
        ZeyKart.kullanıcıAdı = "zey";

        
        Console.Write("Kart No Giriniz: ");
            girkartno = Convert.ToInt32(Console.ReadLine());
            Console.Write("Şifrenizi Giriniz: ");
            girsifre = Convert.ToInt32(Console.ReadLine());
            while (true)
            {

        if (girkartno == NilKart.Kartno && girsifre == NilKart.sifre)
        {
            Console.WriteLine("Hoşgeldiniz: " + NilKart.kullanıcıAdı);
            Console.WriteLine("Ne İşlem Yapmak İstiyorsunuz? (1 - Para Ekleme 2 - Para Çekme 3 - Bakiye Görüntüleme 4 - Çıkış) ");
            secim = Convert.ToInt32(Console.ReadLine());
            if (secim == 1)
            {
                Console.Write("Ne kadar Yatırmak istiyorsunuz? ");
                yatpara = Convert.ToInt32(Console.ReadLine());
                NilKart.bakiye = NilKart.bakiye + yatpara;
            }
            else if (secim == 2)
            {
                Console.Write("Ne kadar Çekmek istiyorsunuz? ");
                cekpara = Convert.ToInt32(Console.ReadLine());
                if (cekpara <= NilKart.bakiye)
                {
                    NilKart.bakiye = NilKart.bakiye - cekpara;
                    Console.WriteLine("Kalan Bakiye: " + NilKart.bakiye);
                }
                else { Console.WriteLine("Bakiye Yetersiz!"); }
            }

            else if (secim == 3) { Console.WriteLine("Bakiyeniz: " + NilKart.bakiye); }
                else if (secim == 4) { break; }
                else { Console.WriteLine("Yanlış Seçim Yaptınız!"); }
        }


        else if (girkartno == SefaKart.Kartno && girsifre == SefaKart.sifre)
        {
            Console.WriteLine("Hoşgeldiniz: " + SefaKart.kullanıcıAdı);
            Console.WriteLine("Ne İşlem Yapmak İstiyorsunuz? (1 - Para Ekleme 2 - Para Çekme 3 - Bakiye Görüntüleme 4 - Çıkış) ");
            secim = Convert.ToInt32(Console.ReadLine());
            if (secim == 1)
            {
                Console.Write("Ne kadar Yatırmak istiyorsunuz? ");
                yatpara = Convert.ToInt32(Console.ReadLine());
                SefaKart.bakiye = SefaKart.bakiye + yatpara;
            }
            else if (secim == 2)
            {
                Console.Write("Ne kadar Çekmek istiyorsunuz? ");
                cekpara = Convert.ToInt32(Console.ReadLine());
                if (cekpara <= SefaKart.bakiye)
                {
                    SefaKart.bakiye = SefaKart.bakiye - cekpara;
                    Console.WriteLine("Kalan Bakiye: " + SefaKart.bakiye);
                }
                else { Console.WriteLine("Bakiye Yetersiz!"); }
            }

            else if (secim == 3) { Console.WriteLine("Bakiyeniz: " + SefaKart.bakiye); }
            else if (secim == 4) { break; }
            else { Console.WriteLine("Yanlış Seçim Yaptınız!"); }
        }

            else if (girkartno == ZeyKart.Kartno && girsifre == ZeyKart.sifre)
            {
                Console.WriteLine("Hoşgeldiniz: " + ZeyKart.kullanıcıAdı);
                Console.WriteLine("Ne İşlem Yapmak İstiyorsunuz? (1 - Para Ekleme 2 - Para Çekme 3 - Bakiye Görüntüleme 4 - Çıkış) ");
                secim = Convert.ToInt32(Console.ReadLine());
                if (secim == 1)
                {
                    Console.Write("Ne kadar Yatırmak istiyorsunuz? ");
                    yatpara = Convert.ToInt32(Console.ReadLine());
                    ZeyKart.bakiye = ZeyKart.bakiye + yatpara;
                }
                else if (secim == 2)
                {
                    Console.Write("Ne kadar Çekmek istiyorsunuz? ");
                    cekpara = Convert.ToInt32(Console.ReadLine());
                    if (cekpara <= ZeyKart.bakiye)
                    {
                        ZeyKart.bakiye = ZeyKart.bakiye - cekpara;
                        Console.WriteLine("Kalan Bakiye: " + ZeyKart.bakiye);
                    }
                    else { Console.WriteLine("Bakiye Yetersiz!"); }
                }

                else if (secim == 3) { Console.WriteLine("Bakiyeniz: " + ZeyKart.bakiye); }
                else if (secim == 4) { break; }
                else { Console.WriteLine("Yanlış Seçim Yaptınız!"); }
            }



            else if (yanlıssayı <= 3)
        {
             Console.WriteLine("Şİfre Yanlış!!");
             yanlıssayı++;   
        }
        else { Console.WriteLine("Çok Sayıda Yanlış Şifre Girdiniz Kartınız Bloke Edildi!"); }

        }

    }

}