using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Ptasior
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timeGry = new DispatcherTimer();
        double zdobytePunkty;
        double zyciaPtasiora = 10;
        int silaGrawitacji = 5;
        bool koniecGry;
        Rect obszarKolizyjnyPtasiora;
        Rect obszarKarmieniaPtasiora;
        Rect obszarSpowalniacza;
        public MainWindow()
        {
            InitializeComponent();
            timeGry.Tick += GlownyKrokGry;
            timeGry.Interval = TimeSpan.FromMilliseconds(20);
            StartGry();
        }
        private void GlownyKrokGry(object sender, EventArgs e)
        {
            Punktacja.Content = "Zdobyto punktów: " + zdobytePunkty;
            Zycia.Content = "Ilość żyć: " + zyciaPtasiora;
            obszarKolizyjnyPtasiora = new Rect(Canvas.GetLeft(Ptasior), Canvas.GetTop(Ptasior), Ptasior.Width, Ptasior.Height);
            Canvas.SetTop(Ptasior, Canvas.GetTop(Ptasior) + silaGrawitacji);
            if(Canvas.GetTop(Ptasior)<-10)
            {
                Canvas.SetTop(Ptasior, 190);
                zyciaPtasiora -= 1;
                if (zyciaPtasiora == 0)
                {
                    KoniecGry();
                }
            }
            if(Canvas.GetTop(Ptasior) > 458)
            {
                Canvas.SetTop(Ptasior, 190);
                zyciaPtasiora -= 1;
                if (zyciaPtasiora == 0)
                {
                    KoniecGry();
                }
            }
            Random random = new Random();
            int losowa = random.Next(500, 800);
            foreach (var obraz in powierzchniaRysowania.Children.OfType<Image>())
            {
                if((string)obraz.Tag == "przekaska")
                {
                    Canvas.SetLeft(obraz, Canvas.GetLeft(obraz) - 5);
                    Rect obszarKarmieniaPtasiora = new Rect(Canvas.GetLeft(obraz), Canvas.GetTop(obraz), obraz.Width, obraz.Height);

                    if (obszarKolizyjnyPtasiora.IntersectsWith(obszarKarmieniaPtasiora))
                    {
                        
                        zyciaPtasiora += 1;
                        Canvas.SetLeft(obraz, 1000);

                    }
                }
                if ((string)obraz.Tag == "spowalniacz")
                {
                    Canvas.SetLeft(obraz, Canvas.GetLeft(obraz) - 5);
                    Rect obszarSpowalniacza = new Rect(Canvas.GetLeft(obraz), Canvas.GetTop(obraz), obraz.Width, obraz.Height);

                    if (obszarKolizyjnyPtasiora.IntersectsWith(obszarSpowalniacza))
                    {
                        silaGrawitacji += 17;
                        Canvas.SetLeft(obraz, 1500);
                    }
                }


                if (((string)obraz.Tag == "przeszkoda1") || ((string)obraz.Tag == "przeszkoda2") || ((string)obraz.Tag == "przeszkoda3"))
                {
                    int losowa2 = random.Next(-30,50);

                    Canvas.SetLeft(obraz, Canvas.GetLeft(obraz) - 5);
                   

                    if (Canvas.GetLeft(obraz)<-50)
                    {


                        Canvas.SetLeft(obraz, 800);
                        Canvas.SetTop(obraz,Canvas.GetTop(obraz)-losowa2);
                        zdobytePunkty += 0.5;
                    }
                    Rect obszarKolizyjnyPrzeszkody= new Rect(Canvas.GetLeft(obraz), Canvas.GetTop(obraz), obraz.Width, obraz.Height);
                    
                    if(obszarKolizyjnyPtasiora.IntersectsWith(obszarKolizyjnyPrzeszkody))
                    {
                        Canvas.SetTop(Ptasior, 190);
                        zyciaPtasiora -= 1;
                        if(zyciaPtasiora <= 0)
                        {
                            KoniecGry();
                            zyciaPtasiora = 10;
                        }
                        
                    }
                
                    
                }
                if ((string)obraz.Tag == "chmura")
                {
                    Canvas.SetLeft(obraz, Canvas.GetLeft(obraz) - 2);
                    if (Canvas.GetLeft(obraz) < -250)
                    {
                        Canvas.SetLeft(obraz, 550);
                    }
                }
           
            }
        }
        private void NacisnietoKlawisz(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                Ptasior.RenderTransform = new RotateTransform(-20, Ptasior.Width / 2, Ptasior.Height / 2);
                silaGrawitacji = -5;
            }
            if((e.Key == Key.R) && (koniecGry==true))
            {
                StartGry();
            }
        }
        private void ZwolnionoKlawisz(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                Ptasior.RenderTransform = new RotateTransform(5, Ptasior.Width/2, Ptasior.Height/2);
                silaGrawitacji = 5;
            }
        }
        private void StartGry()
        {
            powierzchniaRysowania.Focus();
            int wartoscPomocnicza = 300;
            zdobytePunkty = 0;
            koniecGry = false;
            Canvas.SetTop(Ptasior, 190);
            foreach (var obraz in powierzchniaRysowania.Children.OfType<Image>())
            {
                if((string)obraz.Tag == "przeszkoda1")
                {
                    Canvas.SetLeft(obraz, 500);
                }
                if ((string)obraz.Tag == "przeszkoda2")
                {
                    Canvas.SetLeft(obraz, 800);
                }
                if ((string)obraz.Tag == "przeszkoda3")
                {
                    Canvas.SetLeft(obraz, 1100);
                }
                if ((string)obraz.Tag == "spowalniacz")
                {
                    Canvas.SetLeft(obraz, 800);
                }
                if ((string)obraz.Tag == "przekaska")
                {
                    Canvas.SetLeft(obraz, 1100);
                }
                if ((string)obraz.Tag == "chmura")
                {
                    Canvas.SetLeft(obraz, 300 + wartoscPomocnicza);
                    wartoscPomocnicza = 800;
                }

            }
            timeGry.Start();
        }
        private void KoniecGry()
        {
            timeGry.Stop();
            koniecGry = true;
           Punktacja.Content += "\n\rKoniec Gry! Naciśnij R aby grać od początku";
        }
    }
}
