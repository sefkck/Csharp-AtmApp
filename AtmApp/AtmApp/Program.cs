class BankaHesap {
    public int Kartno;
    int sifre ;
    int bakiye;
    static void Main(string[] args)
    {
        int girkartno, girsifre, yanlıssayı = 0,secim,yatpara,cekpara;
        BankaHesap Kart = new BankaHesap();
        Kart.Kartno = 15061406;
        Kart.sifre = 1406;
        Kart.bakiye = 0;
        
            Console.Write("Kart No Giriniz: ");
            girkartno = Convert.ToInt32(Console.ReadLine());
            Console.Write("Şifrenizi Giriniz: ");
            girsifre = Convert.ToInt32(Console.ReadLine());
        while (true)
        {
            if (girkartno == Kart.Kartno && girsifre == Kart.sifre)
            {
                Console.WriteLine("Ne İşlem Yapmak İstiyorsunuz? (1 - Para Ekleme 2 - Para Çekme 3 - Bakiye Görüntüleme) ");
                secim = Convert.ToInt32(Console.ReadLine());
                if (secim == 1)
                {
                    Console.Write("Ne kadar Yatırmak istiyorsunuz? ");
                    yatpara = Convert.ToInt32(Console.ReadLine());
                    Kart.bakiye = Kart.bakiye + yatpara;
                }
                else if (secim == 2 )
                {
                    Console.Write("Ne kadar Çekmek istiyorsunuz? ");
                    cekpara = Convert.ToInt32(Console.ReadLine());
                    if (cekpara <= Kart.bakiye)
                    {
                        Kart.bakiye = Kart.bakiye - cekpara;
                        Console.WriteLine("Kalan Bakiye: "+Kart.bakiye);
                    }
                    else { Console.WriteLine("Bakiye Yetersiz!"); }    
                }

                else if (secim == 3) { Console.WriteLine("Bakiyeniz: " + Kart.bakiye); }
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