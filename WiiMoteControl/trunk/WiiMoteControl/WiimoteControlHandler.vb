Imports System
Imports System.Threading
Imports System.Drawing

Imports WiimoteLib

Public Class WiimoteControlHandler

    Private dLeftClick As Boolean
    Private dRightClick As Boolean

    Dim ParentMote As Wiimote

    Private Declare Sub mouse_event Lib "user32.dll" (ByVal dwFlags As Int32, ByVal dx As Int32, ByVal dy As Int32, ByVal cButtons As Int32, ByVal dwExtraInfo As Int32)
    Private Const MOUSEEVENTF_ABSOLUTE As Integer = &H8000 ' absolute move
    Private Const MOUSEEVENTF_LEFTDOWN As Integer = &H2 ' left button down
    Private Const MOUSEEVENTF_LEFTUP As Integer = &H4 ' left button up
    Private Const MOUSEEVENTF_MOVE As Integer = &H1 ' mouse move
    Private Const MOUSEEVENTF_MIDDLEDOWN As Integer = &H20 ' middle button down
    Private Const MOUSEEVENTF_MIDDLEUP As Integer = &H40 ' middle button up
    Private Const MOUSEEVENTF_RIGHTDOWN As Integer = &H8 ' right button down
    Private Const MOUSEEVENTF_RIGHTUP As Integer = &H10 ' right button up

    Private IRPosD As System.Drawing.PointF

    Private IREmitterOnTop As Boolean ' Not used properly yet

    Dim _Threshold As Double = 0

    Public Sub New(ByRef _WiiMote As Wiimote)
        ParentMote = _WiiMote
    End Sub

    Protected Friend Sub UpdateWiimoteState(ByVal sender As Object, ByVal e As WiimoteChangedEventArgs)

        SetIRCursorPosition(e)
        e.WiimoteState.ExtensionType = ExtensionType.Nunchuk

        Dim X As Double = 0.0
        Dim Y As Double = 0.0

        X = Math.Round(e.WiimoteState.NunchukState.Joystick.X, 2, MidpointRounding.ToEven)
        Y = Math.Round(e.WiimoteState.NunchukState.Joystick.Y, 2, MidpointRounding.ToEven)

        Console.WriteLine("(" & FallOffThreshold(X, _Threshold) & " , " & FallOffThreshold(Y, _Threshold) & ")")

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
            ParentMote.Disconnect()
        End If

    End Sub

    Protected Friend Sub UpdateWiimoteExtension(ByVal sender As Object, ByVal e As WiimoteExtensionChangedEventArgs)
        If e.ExtensionType = ExtensionType.Nunchuk And e.Inserted Then
            MsgBox("The Nunchuck has been attached")
        Else
            MsgBox("The Nunchuck has been removed")
        End If
    End Sub

    Private Function FallOffThreshold(ByRef Dbl As Double, ByVal Threshold As Double) As Double
        Dim New_Dbl As Double = 0.0

        If Threshold < 0 Then
            Throw New Exception("Threshold cannot be negative.")
        End If

        If Dbl < 0 Then 'Negative input value
            If Dbl + Threshold > 0 Then
                Return 0
            Else
                Return Dbl + Threshold
            End If
        ElseIf Dbl > 0 Then 'Positve input value
            If Dbl - Threshold < 0 Then
                Return 0
            Else
                Return Dbl - Threshold
            End If
        End If

        Return New_Dbl
    End Function

    Private Sub SetIRCursorPosition(ByVal e As WiimoteChangedEventArgs)

        Dim PosX As Integer
        Dim PosY As Integer

        If e.WiimoteState.IRState.IRSensors(0).Found Then

            PosX = CInt(Math.Round(Screen.PrimaryScreen.Bounds.Width - (e.WiimoteState.IRState.IRSensors(0).Position.X * Screen.PrimaryScreen.Bounds.Width), MidpointRounding.ToEven))
            PosY = CInt(Math.Round((e.WiimoteState.IRState.IRSensors(0).Position.Y * Screen.PrimaryScreen.Bounds.Height), MidpointRounding.ToEven))

            IRPosD = New Drawing.PointF
            IRPosD.X = PosX
            IRPosD.Y = PosY

        End If

    End Sub

    Private Sub CursorPosition(ByVal X As Integer, ByVal Y As Integer)

        Cursor.Position = New System.Drawing.Point(X, Y)
        
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
