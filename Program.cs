using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

var patrologiaGregaUrl = "https://drive.google.com/embeddedfolderview?id=1fjm7q8GwWG52PaMXkmBWAq7KsTqakQPV#list";
var patrologiaLatinaUrl = "https://drive.google.com/embeddedfolderview?id=11ch2xdSp26wqcbJ-sIBt4NJvDqbNTZGS#list";
var itemClass = "flip-entry";
var urls = new List<string>();

var options = new ChromeOptions()
{
    AcceptInsecureCertificates = true,
};
options.AddUserProfilePreference("plugins.always_open_pdf_externally", true);
options.AddUserProfilePreference("download.prompt_for_download", false);
options.AddUserProfilePreference("download.directory_upgrade", true);

var driver = new ChromeDriver(options);
driver.Manage().Window.FullScreen();

NavigateToPageAndGetUrls(patrologiaGregaUrl);
foreach (var url in urls)
{
    driver.Navigate().GoToUrl(url);
    if (IsElementPresent(By.Id("uc-download-link")))
    {
        driver.FindElement(By.Id("uc-download-link")).Click();
    }

    await Task.Delay(3000);
    driver.Navigate ().GoToUrl("https://google.com");
}

urls = new List<string>();
NavigateToPageAndGetUrls(patrologiaLatinaUrl);
foreach (var url in urls)
{
    driver.Navigate().GoToUrl(url);
    if (IsElementPresent(By.Id("uc-download-link")))
    {
        driver.FindElement(By.Id("uc-download-link")).Click();
    }

    await Task.Delay(3000);
    driver.Navigate().GoToUrl("https://google.com");
}

void NavigateToPageAndGetUrls(string url)
{
    driver.Navigate().GoToUrl(url);
    var elements = driver.FindElements(By.ClassName(itemClass));
    foreach (var element in elements)
    {
        var googleDriveUrl = element.FindElement(By.TagName("a"))?.GetDomProperty("href")!;
        googleDriveUrl = googleDriveUrl.Replace("/file/", "/u/");
        googleDriveUrl = googleDriveUrl.Replace("/d/", "/0/uc?id=");
        googleDriveUrl = googleDriveUrl.Replace("/view", "&export=download");
        urls.Add(googleDriveUrl);
    }
}

bool IsElementPresent(By by)
{
    try
    {
        driver?.FindElement(by);
        return true;
    }
    catch (Exception)
    {
        return false;
    }
}