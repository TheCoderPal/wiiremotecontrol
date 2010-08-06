Imports System
Imports System.Threading
Imports System.Drawing

Imports WiimoteLib


Public Class Start


    Dim WM As Wiimote
    Dim Active As Boolean = False
    Dim T As Thread

    Private dLeftClick As Boolean
    Private dRightClick As Boolean

    Private Declare Sub mouse_event Lib "user32.dll" (ByVal dwFlags As Int32, ByVal dx As Int32, ByVal dy As Int32, ByVal cButtons As Int32, ByVal dwExtraInfo As Int32)

    Private Const MOUSEEVENTF_ABSOLUTE As Integer = &H8000 ' absolute move
    Private Const MOUSEEVENTF_LEFTDOWN As Integer = &H2 ' left button down
    Private Const MOUSEEVENTF_LEFTUP As Integer = &H4 ' left button up
    Private Const MOUSEEVENTF_MOVE As Integer = &H1 ' mouse move
    Private Const MOUSEEVENTF_MIDDLEDOWN As Integer = &H20
    Private Const MOUSEEVENTF_MIDDLEUP As Integer = &H40
    Private Const MOUSEEVENTF_RIGHTDOWN As Integer = &H8
    Private Const MOUSEEVENTF_RIGHTUP As Integer = &H10

    Private IRPosD As System.Drawing.PointF
    Private IREmitterOnTop As Boolean

    Public _Threshold As Double = 0

    Private WiimoteController As WiimoteControlHandler


    Private Sub Start_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

          

            AddHandler WM.WiimoteChanged, AddressOf WiimoteController.UpdateWiimoteState
            AddHandler WM.WiimoteExtensionChanged, AddressOf WiimoteController.UpdateWiimoteExtension

        Catch ex As WiimoteException
            MsgBox("An error occured when trying to instantiate the Wiimote.")
        End Try

    End Sub

    Private Sub Btn_Connect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Connect.Click
        Try

            _Threshold = 0.025

            WM = New Wiimote()
            WiimoteController = New WiimoteControlHandler(WM)
            WM.Connect()
            WM.SetReportType(InputReport.IRExtensionAccel, True)
            WM.SetLEDs(True, False, False, False)
            IOFile.AutoFlush = True

            Me.WindowState = FormWindowState.Minimized

        Catch ex As WiimoteException
            MsgBox("No Wiimote is currently connected to the PC. Please connect a Wiimote to the PC and try starting the input again")
        Catch ex As WiimoteNotFoundException
            MsgBox("An error occured when trying to connect the Wiimote:" & vbCrLf & ex.Message)
        End Try
    End Sub

    Dim IOFile As System.IO.StreamWriter = New System.IO.StreamWriter("D:\WiiLog2.txt")

End Class


