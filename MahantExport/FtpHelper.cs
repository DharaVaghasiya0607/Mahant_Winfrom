using System;
using System.IO;
using System.Net;

public class FtpHelper
{
    private string host;
    private string user;
    private string pass;

    public FtpHelper(string host, string user, string pass)
    {
        this.host = host;
        this.user = user;
        this.pass = pass;
    }

    public void DownloadFile(string remotePath, string localPath)
    {
        ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, sslPolicyErrors) => true;
        var request = (FtpWebRequest)WebRequest.Create(host + "/" + remotePath);
        request.Method = WebRequestMethods.Ftp.DownloadFile;
        request.Credentials = new NetworkCredential(user, pass);
        request.EnableSsl = true; // FTPS
        request.UsePassive = true; // Usually required for FTPS
        request.UseBinary = true;
        request.KeepAlive = false;

        using (var response = (FtpWebResponse)request.GetResponse())
        using (var stream = response.GetResponseStream())
        using (var fs = new FileStream(localPath, FileMode.Create))
        {
            stream.CopyTo(fs);
        }
    }

}