

Imports System.Xml
Imports System
Imports System.ServiceProcess
Imports System.IO
Imports System.Threading
Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim configXml = New XmlDocument

        configXml.Load("AWSCloudWatch.exe.config")

        Dim attr As XmlAttribute
        attr = configXml.SelectSingleNode("/configuration/appSettings/add[@key = 'AWSAccessKey']/@value")
        attr.Value = txtAccessKey.Text
        attr = configXml.SelectSingleNode("/configuration/appSettings/add[@key = 'AWSSecretKey']/@value")
        attr.Value = txtSecretKey.Text
        configXml.Save("AWSCloudWatch.exe.config")

        Dim sc As ServiceController = New ServiceController("AWS CloudWatch")
        On Error Resume Next
        sc.Stop()
        Thread.Sleep(2000)
        sc.Start()
        If MessageBox.Show("All Settings Saved.", "Confirmation", MessageBoxButtons.OK,
                    Nothing, MessageBoxDefaultButton.Button1) = DialogResult.OK Then
            System.Console.WriteLine("OK")
            Application.Exit()
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
