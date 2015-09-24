Imports System.IO
Imports System.Management
Module Module1
    Dim LoadFile As New List(Of String)
    Dim ReddiList As New List(Of String)
    Dim WriteFile As New List(Of String)
    Dim Log As New List(Of String)
    Dim LogPathAndFileName As String = "log.log"
    Sub Main()
        Console.WriteLine()
        Console.WriteLine("Copyright Krutonium 2015")
        Console.WriteLine("PFCKrutonium@gmail.com")
        Console.WriteLine("http://krutonium.leshcatlabs.net")
        Console.WriteLine("For use with TRON. For other use, Please contact me.")
        Console.WriteLine()

        For Each Arg In My.Application.CommandLineArgs
            If Arg.ToUpper.StartsWith("/I=") Then
                Dim I = Arg.Split("="c)
                Dim TempLoad As New List(Of String)
                TempLoad = File.ReadAllLines(I(1)).ToList
                For Each thing In TempLoad
                    thing = thing.Replace("##", "*")
                    LoadFile.Add(thing)
                Next
            ElseIf Arg.ToUpper = ("/HELP")
                Help()
            ElseIf Arg.ToUpper.StartsWith("/L=")
                Dim L = Arg.Split("="c)
                LogPathAndFileName = L(1)
            End If
        Next
        If LoadFile.Count < 1 Then
            If File.Exists("MatchFile.txt") Then
                Dim TempLoad As New List(Of String)
                TempLoad = File.ReadAllLines("MatchFile.txt").ToList
                For Each thing In TempLoad
                    thing = thing.Replace("##", "*")
                    LoadFile.Add(thing)
                Next
            Else
                Help()
            End If
        End If
        LoadSoftwareList()
        For Each app In ReddiList
            Dim appcapp As String = app
            app = app.ToUpper
            For Each listedapp In LoadFile
                listedapp = listedapp.ToUpper
                If app Like listedapp Then
                    WriteFile.Add(appcapp)
                End If
            Next
        Next
        For Each line In WriteFile
            Console.WriteLine("Uninstalling " & line)
            Dim P = UninstallProgram(line)
            If P = False = False Then
                Console.WriteLine("Failed to remove " & line)
            End If
            RemovalLog(line)
        Next
        Console.WriteLine("Done")
    End Sub
    Private Sub LoadSoftwareList()
        Console.WriteLine("Loading List of Installed Software...")
        Dim moReturn As Management.ManagementObjectCollection
        Dim moSearch As Management.ManagementObjectSearcher
        Dim mo As Management.ManagementObject

        moSearch = New Management.ManagementObjectSearcher("Select * from Win32_Product")

        moReturn = moSearch.Get
        For Each mo In moReturn
            ReddiList.Add(mo("Name").ToString)
        Next
        Console.WriteLine("Finished Loading Installed Software.")
    End Sub

    Private Function UninstallProgram(ProgramName As String) As Boolean
        Try
            Dim mos As New ManagementObjectSearcher((Convert.ToString("SELECT * FROM Win32_Product WHERE Name = '") & ProgramName) + "'")
            For Each mo As ManagementObject In mos.[Get]()
                Try
                    If mo("Name").ToString() = ProgramName Then
                        Dim hr As Object = mo.InvokeMethod("Uninstall", Nothing)
                        Return CBool(hr)
                    End If
                    'this program may not have a name property, so an exception will be thrown
                Catch ex As Exception
                End Try
            Next
            'was not found...
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Sub RemovalLog(ByVal Name As String)
        Using writer As New StreamWriter(LogPathAndFileName, True)
            If Not File.Exists(LogPathAndFileName) Then
                writer.WriteLine("Start Log")
            End If
            writer.WriteLine("Removing " & DateTime.Now)
        End Using
    End Sub
    Sub Help()
        Console.WriteLine("Help: ")
        Console.WriteLine("To specify the list to load, run this application with the /I= parameter")
        Console.WriteLine("For example: Removal.exe /I=List.txt")
        Console.WriteLine("You can also rename 'List.txt' to MatchFile.txt and it will be loaded automatically.")
        Console.WriteLine()
        Console.WriteLine("List File Syntax:")
        Console.WriteLine("Characters in pattern     Matches in string")
        Console.WriteLine("")
        Console.WriteLine("?                         Any single character")
        Console.WriteLine("*                         Zero or more characters")
        Console.WriteLine("#                         Any single digit (0–9)")
        Console.WriteLine("[ charlist ]              Any single character in charlist")
        Console.WriteLine("[! charlist ]             Any single character not in charlist")
        Console.WriteLine()
        Console.WriteLine("You can also specify the location for the logfile by doing this: ")
        Console.WriteLine("Removal.exe /I=List.exe /L=C:/.../RemovalLog.txt")
        Console.WriteLine()
        Environment.Exit(1)
    End Sub
End Module
