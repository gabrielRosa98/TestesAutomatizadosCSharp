using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.IO;
using System;
using System.Threading;
using TestesAutomatizadosCSharp.Common;

namespace TestesAutomatizadosCSharp.Infra{
    public class DriverManager{
        private static WebDriver _instance;

        public WebDriver GetInstance(){
            if(_instance==null){
                _instance = configureDriver(Enums.WebDrivers.chrome,getDirectory());
            }
            return _instance;
        }

        private WebDriver configureDriver(Enums.WebDrivers driver, string path,bool headless = false){
            switch(driver){
                case Enums.WebDrivers.chrome:
                    return new ChromeDriver(path,new ChromeOptions());
                case Enums.WebDrivers.firefox:
                    return new FirefoxDriver(path,new FirefoxOptions());
                case Enums.WebDrivers.edge:
                    return new EdgeDriver(path,new EdgeOptions());
                default:
                    return new ChromeDriver(path,new ChromeOptions());
            }            
        }

        private string getDirectory(){
            return Directory.GetCurrentDirectory()+"\\Infra\\webdrivers";
        }
        
        private WebDriverWait getWebDriverWait(){
            return new WebDriverWait(GetInstance(),TimeSpan.FromSeconds(30));
        }

        public void CloseBrowser(){
            GetInstance().Close();
            GetInstance().Quit();
        }

        public void navigate(string url){
            GetInstance().Navigate().GoToUrl(url);
        }

        public void resize(int width,int heigth){
            GetInstance().Manage().Window.Size = new System.Drawing.Size(width,heigth);
        }

        public void resize(bool fullscreen){
            GetInstance().Manage().Window.FullScreen();            
        }

        public void switchTab(int index){
            GetInstance().SwitchTo().Frame(index);
        }

        public T executeJS<T>(string command){
            IWebDriver driver = GetInstance();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            return (T) js.ExecuteScript(command);
        }

        public void wait(int timeInSeconds){
            Thread.Sleep(timeInSeconds*1000);
        }

        public IWebElement waitElementVisible(By by){
            WebDriverWait wait = getWebDriverWait();

            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(by));

            return element;
        }
    }
}