// Decompiled with JetBrains decompiler
// Type: LOB.Model.easypay.ftp
// Assembly: LOB, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3597789E-8774-4427-AE20-07195D9380BD
// Assembly location: C:\Program Files (x86)\CETAP LOB\LOB.exe

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CETAP_LOB.Model.easypay
{
  public class ftp
  {
    private int bufferSize = 4 * 1024;
    private string host;
    private string user;
    private string pass;
    private FtpWebRequest ftpRequest;
    private FtpWebResponse ftpResponse;
    private Stream ftpStream;

    public ftp(string hostIP, string userName, string password)
    {
      host = "ftp://" + hostIP;
      user = userName;
      pass = password;
    }

    public async Task downloadAsync(string remoteFile, string localFile)
    {
      try
      {
        ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + remoteFile);
        ftpRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = true;
        ftpRequest.Method = "RETR";
        ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
        ftpStream = ftpResponse.GetResponseStream();
        FileStream localFileStream = new FileStream(localFile, FileMode.Create);
        byte[] byteBuffer = new byte[bufferSize];
        int bytesRead = await ftpStream.ReadAsync(byteBuffer, 0, bufferSize);
        try
        {
          for (; bytesRead > 0; bytesRead = await ftpStream.ReadAsync(byteBuffer, 0, bufferSize))
            await localFileStream.WriteAsync(byteBuffer, 0, bytesRead);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
        localFileStream.Close();
        ftpStream.Close();
        ftpResponse.Close();
        ftpRequest = (FtpWebRequest) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    public void upload(string remoteFile, string localFile)
    {
      try
      {
        ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + remoteFile);
        ftpRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = true;
        ftpRequest.Method = "STOR";
        ftpStream = ftpRequest.GetRequestStream();
        FileStream fileStream = new FileStream(localFile, FileMode.Open);
        byte[] buffer = new byte[bufferSize];
        int count = fileStream.Read(buffer, 0, bufferSize);
        try
        {
          for (; count != 0; count = fileStream.Read(buffer, 0, bufferSize))
            ftpStream.Write(buffer, 0, count);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
        fileStream.Close();
        ftpStream.Close();
        ftpRequest = (FtpWebRequest) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    public void delete(string deleteFile)
    {
      try
      {
        ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + deleteFile);
        ftpRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = true;
        ftpRequest.Method = "DELE";
        ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
        ftpResponse.Close();
        ftpRequest = (FtpWebRequest) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    public void rename(string currentFileNameAndPath, string newFileName)
    {
      try
      {
        ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + currentFileNameAndPath);
        ftpRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = true;
        ftpRequest.Method = "RENAME";
        ftpRequest.RenameTo = newFileName;
        ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
        ftpResponse.Close();
        ftpRequest = (FtpWebRequest) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    public void createDirectory(string newDirectory)
    {
      try
      {
        ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + newDirectory);
        ftpRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = true;
        ftpRequest.Method = "MKD";
        ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
        ftpResponse.Close();
        ftpRequest = (FtpWebRequest) null;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
    }

    public string getFileCreatedDateTime(string fileName)
    {
      try
      {
        ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + fileName);
        ftpRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = true;
        ftpRequest.Method = "MDTM";
        ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
        ftpStream = ftpResponse.GetResponseStream();
        StreamReader streamReader = new StreamReader(ftpStream);
        string str = (string) null;
        try
        {
          str = streamReader.ReadToEnd();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
        streamReader.Close();
        ftpStream.Close();
        ftpResponse.Close();
        ftpRequest = (FtpWebRequest) null;
        return str;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      return "";
    }

    public string getFileSize(string fileName)
    {
      try
      {
        ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + fileName);
        ftpRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = true;
        ftpRequest.Method = "SIZE";
        ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
        ftpStream = ftpResponse.GetResponseStream();
        StreamReader streamReader = new StreamReader(ftpStream);
        string str = (string) null;
        try
        {
          while (streamReader.Peek() != -1)
            str = streamReader.ReadToEnd();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
        streamReader.Close();
        ftpStream.Close();
        ftpResponse.Close();
        ftpRequest = (FtpWebRequest) null;
        return str;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      return "";
    }

    public string[] directoryListSimple(string directory)
    {
      try
      {
        FtpWebRequest ftpWebRequest = (FtpWebRequest) WebRequest.Create(host);
        ftpWebRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpWebRequest.UseBinary = true;
        ftpWebRequest.UsePassive = true;
        ftpWebRequest.KeepAlive = true;
        ftpWebRequest.Method = "NLST";
        ftpResponse = (FtpWebResponse) ftpWebRequest.GetResponse();
        ftpStream = ftpResponse.GetResponseStream();
        StreamReader streamReader = new StreamReader(ftpStream);
        string str = (string) null;
        try
        {
          while (streamReader.Peek() != -1)
            str = str + streamReader.ReadLine() + "|";
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
        streamReader.Close();
        ftpStream.Close();
        ftpResponse.Close();
        try
        {
          return str.Split("|".ToCharArray());
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      return new string[1]{ "" };
    }

    public string[] directoryListDetailed(string directory)
    {
      try
      {
        ftpRequest = (FtpWebRequest) WebRequest.Create(host + "/" + directory);
        ftpRequest.Credentials = (ICredentials) new NetworkCredential(user, pass);
        ftpRequest.UseBinary = true;
        ftpRequest.UsePassive = true;
        ftpRequest.KeepAlive = true;
        ftpRequest.Method = "LIST";
        ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();
        ftpStream = ftpResponse.GetResponseStream();
        StreamReader streamReader = new StreamReader(ftpStream);
        string str = (string) null;
        try
        {
          while (streamReader.Peek() != -1)
            str = str + streamReader.ReadLine() + "|";
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
        streamReader.Close();
        ftpStream.Close();
        ftpResponse.Close();
        ftpRequest = (FtpWebRequest) null;
        try
        {
          return str.Split("|".ToCharArray());
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
      }
      return new string[1]{ "" };
    }
  }
}
