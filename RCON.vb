Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Module RCON

	'***********
	'** USAGE **
	'***********
	'Add RCON.vb class to your project
	'Set rcon_password cvar in your server.cfg file
	'Call class SendRCONcommand sub in this way:
	'SendRCONCommand("ServerIP", "port", "RCON_Password", "Command")
	'Example: SendRCONCommand("123.321.213.312", "27015", "mypassword", "say Hello!")

    Public A2S_INFO_Request() As Byte = {&HFF, &HFF, &HFF, &HFF, &H54, &H53, &H6F, &H75, &H72, &H63, &H65, &H20, &H45, &H6E, &H67, &H69, &H6E, &H65, &H20, &H51, &H75, &H65, &H72, &H79, &H0}
    Public A2S_PLAYER_Request() As Byte = {&HFF, &HFF, &HFF, &HFF, &H55, &HFF, &HFF, &HFF, &HFF}

    Public Sub SendRCONCommand(ByVal Server As String, ByVal port As Integer, ByVal Password As String, ByVal Command As String)
        '***************************************
        '** Connect TCP socket to server:port **
        '***************************************
        Dim Client As New TcpClient
        Dim Stream As NetworkStream

        Try
            If Client.ConnectAsync(Server, port).Wait(TimeSpan.FromSeconds(3)) Then
                Stream = Client.GetStream()
            Else               
                MsgBox("Socket timeout. Server took too long to respond.", vbExclamation, "RCON Error")
                Exit Sub
            End If
        Catch ex As Exception           
            MsgBox("Connection refused or host unreachable.", vbExclamation, "RCON Error")
            Exit Sub
        End Try

        '********************************
        '** Send AUTH Packet to server **
        '********************************
        If Client.Connected = True Then
            Try
                Dim AuthPacket As Byte() = New Byte(CByte((4 + 4 + 4 + Password.Length + 1))) {}
                AuthPacket(0) = Password.Length + 9         'Packet Size (Integer)
                AuthPacket(4) = 99                          'Request Id (Integer)
                AuthPacket(8) = 3                           ' 3 = SERVERDATA_AUTH
                For X As Integer = 0 To Password.Length - 1
                    AuthPacket(12 + X) = System.Text.Encoding.UTF8.GetBytes(Password(X))(0)
                Next
                Stream.Write(AuthPacket, 0, AuthPacket.Length)
                Dim data As Byte() = New Byte(4096) {}
                Dim bytes As Integer
                Do
                    bytes = Stream.Read(data, 0, data.Length)
                Loop While bytes = 0
                Dim result As Byte() = New Byte(bytes - 1) {}
                Array.Copy(data, 0, result, 0, bytes)
                Dim ID As Integer = BitConverter.ToInt32(data, 4)
                Dim Type As Integer = BitConverter.ToInt32(data, 8)
                If ID = 99 And Type = 2 Then
                    '*******************************************
                    '** Authorized Succesfully, send command. **
                    '*******************************************
                    Dim CommandPacket As Byte() = New Byte(CByte((4 + 4 + 4 + Command.Length + 1))) {}
                    CommandPacket(0) = Command.Length + 9          'Packet Size (Integer)
                    CommandPacket(4) = 99                          'Request Id (Integer)
                    CommandPacket(8) = 2                           '2 = SERVERDATA_EXECCOMMAND
                    For X As Integer = 0 To Command.Length - 1
                        CommandPacket(12 + X) = System.Text.Encoding.UTF8.GetBytes(Command(X))(0)
                    Next
                    Stream.Write(CommandPacket, 0, CommandPacket.Length)                  
                    Dim data1 As Byte() = New Byte(4096) {}
                    Dim bytes1 As Integer
                    Do
                        bytes1 = Stream.Read(data1, 0, data1.Length)
                    Loop While bytes1 = 0
                    Dim result1 As Byte() = New Byte(bytes1 - 1) {}
                    Array.Copy(data1, 0, result1, 0, bytes1)
                    Dim size As Integer = BitConverter.ToInt32(data1, 0)
                    Dim ID1 As Integer = BitConverter.ToInt32(data1, 4)
                    Dim Typex As Integer = BitConverter.ToInt32(data1, 8)
                    Dim Payload As String = Encoding.UTF8.GetString(data1, 12, size - 10)
                    If ID1 = 99 And Typex = 0 Then
                        '**********************************************
                        '** Command sent and received response is OK **
                        '********************************************** 
                    Else
                        '***********************************************
                        '** Command sent and received response FAILED **
                        '***********************************************                       
                    End If
                Else
                    '***************************************
                    '** Authentication failed with server **
                    '***************************************                  
                    MsgBox("The RCON password for this server seems invalid!", vbExclamation, "Authentication failed")
                End If
                If Client.Connected = True Then
                    Client.Close()
                    Stream.Flush()
                    Stream.Close()
                End If
            Catch ex As Exception               
                MsgBox("Unexpected error:" & vbCrLf & vbCrLf & ex.Message, vbExclamation, "RCON Error")
            Finally
                If Client.Connected = True Then
                    Client.Close()
                    Stream.Flush()
                    Stream.Close()
                End If
            End Try
        End If
    End Sub
	
	
		
	

    Public Function ReadSteamString(ByVal reader As BinaryReader) As String
        Dim str As List(Of Byte) = New List(Of Byte)()
        Dim nextByte As Byte = reader.ReadByte()

        While nextByte <> 0
            str.Add(nextByte)
            nextByte = reader.ReadByte()
        End While

        Return Encoding.UTF8.GetString(str.ToArray())
    End Function
	
	
	
	

    Public Sub ListPlayersA2S()
        Main.PlayerList.Items.Clear()
        Try
            Dim targetServer As New IPEndPoint(IPAddress.Parse(Main.ServerList.SelectedItems(0).SubItems(1).Text), CInt(Main.ServerList.SelectedItems(0).SubItems(2).Text))
            Using client As New UdpClient
                client.Client.ReceiveTimeout = 2000
                client.Client.SendTimeout = 2000
                client.Send(A2S_PLAYER_Request, A2S_PLAYER_Request.Length, targetServer)
                Dim Challenge_Request As Byte() = client.Receive(targetServer)
                Dim Challenge_Bytes As Byte() = {Challenge_Request(5), Challenge_Request(6), Challenge_Request(7), Challenge_Request(8)}
                Dim A2S_Request_With_Challenge_Bytes As Byte() = {&HFF, &HFF, &HFF, &HFF, &H55, Challenge_Bytes(0), Challenge_Bytes(1), Challenge_Bytes(2), Challenge_Bytes(3)}
                client.Send(A2S_Request_With_Challenge_Bytes, A2S_Request_With_Challenge_Bytes.Length, targetServer)
                Dim A2S_Player As Byte() = client.Receive(targetServer)
                Dim stream As MemoryStream = New MemoryStream(A2S_Player)
                Dim reader As BinaryReader = New BinaryReader(stream)
                stream.Seek(4, SeekOrigin.Begin)
                Dim header As Integer = reader.ReadByte()
                Dim PlayerCounter As Integer = reader.ReadByte()
                For i = 0 To PlayerCounter - 1
                    Dim index As Integer = reader.ReadByte()
                    Dim Name As String = ReadSteamString(reader)
                    Dim Score As Integer = reader.ReadInt32()
                    Dim Duration As Single = reader.ReadSingle()
                    Dim tsSeconds = TimeSpan.FromSeconds(Convert.ToDouble(Duration))
                    Dim objItem As ListViewItem
                    objItem = Main.PlayerList.Items.Add(Name)
                    With objItem
                        .ImageIndex = 7
                        .SubItems.Add(Score.ToString)
                        .SubItems.Add(tsSeconds.ToString("hh\:mm\:ss"))
                    End With
                Next
                stream.Close()
                stream.Dispose()
                reader.Close()
                reader.Dispose()
            End Using
        Catch ex As Exception
            LogMessage(ex.Message, 1)
        End Try
    End Sub    

End Module
