using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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
using System.IO;
using System.Diagnostics;


namespace Parser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public class Tenders
        {
            public string Number { get; set; }
            public string Price { get; set; }
            public string Hyperlink { get; set; }
            public string Time { get; set; }
        }

        private async void StartBtn_Click(object sender, RoutedEventArgs e)
        {

            string[] TendersData = new string[4];

            if (string.IsNullOrEmpty(Vvod.Text))
            {
                MessageBox.Show("Запрос не введен!");
                return;
            }
            
            var vvod = Vvod.Text;

            IWebDriver driver = new ChromeDriver();
            driver.Url = @"https://zakupki.gov.ru/epz/main/public/home.html";

            await Task.Delay(1000);

            driver.FindElement(By.XPath("(//a[contains(@class,'btn')])[5]")).Click();

            await Task.Delay(1000);

            driver.FindElement(By.XPath("(//span[contains(@class,'btn-text')])[16]")).Click();

            driver.FindElement(By.XPath("//input[@placeholder='Введите полностью или часть номера, наименования закупки, идентификационного кода закупки (ИКЗ), наименования или ИНН Заказчика']")).SendKeys(vvod);

            
            await Task.Delay(2000);

            driver.FindElement(By.CssSelector(".search__btn")).Click();

            await Task.Delay(2000);

            driver.FindElement(By.XPath("(//label[@class='params-text'])[1]")).Click();

            await Task.Delay(2000);

            driver.FindElement(By.CssSelector(".search__btn")).Click();

            await Task.Delay(2000);
            int i = 5;
            int j = 1;
            int k = 5;
            int l = 1;
            try
            { 
                while (true)
                {
                    var number = driver.FindElement(By.XPath($"(//a[@target='_blank'])[{i}]")).GetAttribute("textContent");
                    var href = driver.FindElement(By.XPath($"(//a[@target='_blank'])[{i}]")).GetAttribute("href");
                    i++;
                    i++;
                    i++;
                    i++;
                    i++;
                    var price = driver.FindElement(By.XPath($"(//div[@class='price-block__value'])[{j}]")).GetAttribute("textContent");
                    j++;
                    TendersData[0] = number;
                    TendersData[1] = href; 
                    TendersData[2] = price;
                    TendersData[3] = Convert.ToString(DateTime.Now);
                    PlusTenders(TendersData);
                }
            }
            catch
            {
               
            }
            driver.FindElement(By.XPath("(//label[@class='params-text'])[2]")).Click();

            driver.FindElement(By.XPath("(//label[@class='params-text'])[1]")).Click();

            driver.FindElement(By.XPath("(//label[@class='params-text'])[2]")).Click();

            driver.FindElement(By.CssSelector(".search__btn")).Click();
            try
            {
                for (int c = 2; c < 5; c++)
                {
                    for (int b = 0; b < 9; i++)
                    {
                        var number = driver.FindElement(By.XPath($"(//a[@target='_blank'])[{k}]")).GetAttribute("textContent");
                        var href = driver.FindElement(By.XPath($"(//a[@target='_blank'])[{k}]")).GetAttribute("href");
                        k++;
                        k++;
                        k++;
                        k++;
                        var price = driver.FindElement(By.XPath($"(//div[@class='price-block__value'])[{l}]")).GetAttribute("textContent");
                        l++;
                        TendersData[0] = number;
                        TendersData[1] = href;
                        TendersData[2] = price;
                        TendersData[3] = Convert.ToString(DateTime.Now);
                        PlusTenders(TendersData);
                    }
                    driver.FindElement(By.XPath($"//a[@href='javascript:goToPage({c})'][contains(.,'{c}')]")).Click();
                }
            }
            catch
            {
                driver.Quit();
            }
            ReadTenders();
        
        void PlusTenders(string[] DataPlus)
        {
            StreamWriter tenders = new StreamWriter(Directory.GetCurrentDirectory() + "/tenders.txt", append: true);
            string line = $"{DataPlus[0]};{DataPlus[1]};{DataPlus[2]};{DataPlus[3]}";
            line = line.Replace(Environment.NewLine, " ");
            line = line.Trim();
            tenders.WriteLine(line);
            tenders.Close();
        }
        void ReadTenders()
        {
            StreamReader tenders = new StreamReader(Directory.GetCurrentDirectory() + "/tenders.txt");
            string line = tenders.ReadLine();
            int n = 0;
                List<Tenders> tendersList = new List<Tenders>
                    {

                    };
                while (line != null)
                {
                    n++;
                    string[] splitLine = line.Split(';');
                    tendersList.Add(new Tenders { Number = splitLine[0], Price = splitLine[2], Hyperlink = splitLine[1], Time = splitLine[3] });
                    line = tenders.ReadLine();
                }
                tenderGrid.ItemsSource = tendersList;
                tenders.Close();
        }
        // //a[contains(.,'№ 32211172930')]
        //driver.FindElement(By.XPath("//a[@href='javascript:goToPage(2)'][contains(.,'2')]")).Click();
    }
    }
}
