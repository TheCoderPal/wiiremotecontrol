Imports System
Imports System.Threading
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

    Private Sub Start_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            WM = New Wiimote()
            AddHandler WM.WiimoteChanged, AddressOf UpdateWiimoteState

            Console.Out.Write(3)

        Catch ex As WiimoteException
            MsgBox("An error occured when trying to instantiate the Wiimote.")
        End Try


    End Sub

    Private Sub Btn_Connect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Connect.Click
        Try

            WM.Connect()
            WM.SetReportType(InputReport.IRAccel, True)
            WM.SetLEDs(True, False, False, False)
            IOFile.AutoFlush = True

            Me.WindowState = FormWindowState.Minimized


        Catch ex As WiimoteNotFoundException
            MsgBox("An error occured when trying to connect the Wiimote:" & vbCrLf & ex.Message)
        End Try
    End Sub

    Dim IOFile As System.IO.StreamWriter = New System.IO.StreamWriter("D:\WiiLog2.txt")

    Private Sub UpdateWiimoteState(ByVal sender As Object, ByVal e As WiimoteChangedEventArgs)

        SetIRCursorPosition(e)
        Console.WriteLine(e.WiimoteState.AccelState.Values)

        If e.WiimoteState.AccelState.Values.X > 1 Or e.WiimoteState.AccelState.Values.X < -1 Then
            IOFile.WriteLine(e.WiimoteState.AccelState.Values)
        End If

        If e.WiimoteState.ButtonState.A And Not dLeftClick Then

            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0)
            dLeftClick = True

        ElseIf Not e.WiimoteState.ButtonState.A And dLeftClick Then

            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0)
            dLeftClick = False

        End If


        If e.WiimoteState.ButtonState.B And Not dRightClick Then

            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0)
            dRightClick = True

        ElseIf Not e.WiimoteState.ButtonState.B And dRightClick Then

            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0)
            dRightClick = False

        End If

        If e.WiimoteState.ButtonState.Plus Then

            WM.Disconnect()

        End If

    End Sub

    Private Sub SetIRCursorPosition(ByVal e As WiimoteChangedEventArgs)
        Dim PosX As Integer
        Dim PosY As Integer

        If e.WiimoteState.IRState.IRSensors(0).Found Then
            PosX = CInt(Math.Round(Screen.PrimaryScreen.Bounds.Width - (e.WiimoteState.IRState.IRSensors(0).Position.X * Screen.PrimaryScreen.Bounds.Width), MidpointRounding.ToEven))
            PosY = CInt(Math.Round((e.WiimoteState.IRState.IRSensors(0).Position.Y * Screen.PrimaryScreen.Bounds.Height), MidpointRounding.ToEven))

            Cursor.Position = FindMeanOfVector(New System.Drawing.PointF(CInt(Math.Round(PosX)), CInt(Math.Round(PosY))), IRPosD, 0.0001)

            IRPosD = New Drawing.PointF
            IRPosD.X = PosX
            IRPosD.Y = PosY

        End If
      
    End Sub

    Private Function FindMeanOfVector(ByVal pt1 As System.Drawing.PointF, ByVal pt2 As System.Drawing.PointF, Optional ByVal Scalar As Double = 0.5) As System.Drawing.Point
        Dim dx As Integer
        Dim dy As Integer
        dx = CInt(Math.Max(pt1.X, pt2.X) - Math.Min(pt1.X, pt2.X))
        dy = CInt(Math.Max(pt1.Y, pt2.Y) - Math.Min(pt1.Y, pt2.Y))
        Dim MeanPoint As System.Drawing.Point
        MeanPoint = New System.Drawing.Point
        MeanPoint.X = CInt(Math.Round(((dx * Scalar) + pt1.X)))
        MeanPoint.Y = CInt(Math.Round(((dy * Scalar) + pt1.Y)))

        Return MeanPoint

    End Function

End Class


